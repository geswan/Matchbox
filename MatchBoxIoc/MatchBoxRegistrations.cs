namespace MatchBoxIoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public partial class MatchBox
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Removes a registered Type
        /// </summary>
        /// <typeparam name="T">The Registered Type</typeparam>
        /// <returns>bool isSuccessful</returns>
        public bool DeRegister<T>()
        {
            return this.Dictionary.Remove(typeof(T).Name);
        }

        /// <summary>
        ///     Registers a Type as a transient
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
        /// <param name="getter">The delegate used to create the Type</param>
        public void Register<T>(Func<T> getter)
        {
            if (getter == null)
            {
                throw new ArgumentNullException("getter");
            }
            //check that it can be invoked
            var invokeList = getter.GetInvocationList();
            invokeList[0].DynamicInvoke(null);
            Type type = typeof(T);
            this.Dictionary[type.Name] = getter;
         
        }
        /// <summary>
        /// Registers a Collection of implementation Types of Type T
        /// Will overwrite any existing registrations of Type T
        /// </summary>
        /// <typeparam name="T">The Type of the collection</typeparam>
        /// <param name="collection">The IEnumerable of Func T to be added</param>
        public void RegisterCollection<T>(IEnumerable<Func<T>> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            Type type = typeof(T);
           

            // ReSharper disable once CoVariantArrayConversion
               this.Dictionary[type.Name] =  Delegate.Combine(collection.ToArray());
          
        }

        /// <summary>
        ///     Registers a Type as a singleton
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
        /// <param name="getter">The delegate used to create the Type</param>
        public void RegisterSingleton<T>(Func<T> getter)
        {
            if (getter == null)
            {
                throw new ArgumentNullException("getter");
            }

            var lazy = new Lazy<T>(getter.Invoke);
            Delegate lazyGetter = new Func<T>(() => lazy.Value);
            this.Dictionary[typeof(T).Name] = lazyGetter;
        }

        /// <summary>
        ///     Registers a Type that is dependent upon another Type
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
         public void Register<T>()
        {
            Func<T> factoryMethod = this.GetFactoryMethod<T, T>();
            this.Register(factoryMethod);
        }

        /// <summary>
        ///     Registers a Type t and its implementation
        /// </summary>
        /// <typeparam name="TPublic">The Type to register</typeparam>
        /// <typeparam name="TImplementation">The implementation of the registered Type</typeparam>
        public void Register<TPublic, TImplementation>()
        {
            Func<TPublic> factoryMethod = this.GetFactoryMethod<TPublic, TImplementation>();
            this.Register(factoryMethod);
        }

        /// <summary>
        ///     Registers a Type, as a singleton, that is dependent upon another Type
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
         public void RegisterSingleton<T>()
        {
            Func<T> factoryMethod = this.GetFactoryMethod<T, T>();
            this.RegisterSingleton(factoryMethod);
        }

        /// <summary>
        ///     Registers a Type, and registers the implementation as a singleton
        /// </summary>
        /// <typeparam name="TPublic">The Type to register</typeparam>
        /// <typeparam name="TImplementation">The implementation of the registered Type</typeparam>
       public void RegisterSingleton<TPublic, TImplementation>()
        {
            Func<TPublic> factoryMethod = this.GetFactoryMethod<TPublic, TImplementation>();
            this.RegisterSingleton(factoryMethod);
        }

        public void RegisterType(Type type, Delegate getter)
        {
            this.Dictionary[type.Name] = getter;
        }

        /// <summary>
        ///     Registers a View, as a transient
        /// </summary>
        /// <typeparam name="TPublic">The Type to register</typeparam>
        /// <typeparam name="TImplementation">The implementation of the registered Type</typeparam>
        public void RegisterView<TPublic, TImplementation>(Func<TImplementation> getter)
        {
            if (getter == null)
            {
                throw new ArgumentNullException("getter");
            }
            Type concreteType = typeof(TImplementation);
            Type type = typeof(TPublic);

            this.Dictionary[type.Name] = getter;
            this.Dictionary[concreteType.Name] = getter;
        }

        /// <summary>
        ///     Registers a View, as a singleton
        /// </summary>
        /// <typeparam name="TPublic">The Type to register</typeparam>
        /// <typeparam name="TImplementation">The implementation of the registered Type</typeparam>
        public void RegisterViewAsSingleton<TPublic, TImplementation>(Func<TImplementation> getter)
        {
            if (getter == null)
            {
                throw new ArgumentNullException("getter");
            }
            Type concreteType = typeof(TImplementation);
            var lazy = new Lazy<TImplementation>(getter.Invoke);
            Delegate lazyGetter = new Func<TImplementation>(() => lazy.Value);
            Type type = typeof(TPublic);

            this.Dictionary[type.Name] = lazyGetter;
            this.Dictionary[concreteType.Name] = lazyGetter;
        }

        #endregion

        #region Methods
        /// <summary>
        /// For each parameter in the constructor.
        ///  Builds an expression that represents calling the ioc.Get_Tparam() method
       /// </summary>
        /// <param name="parameters">ParameterInfo[]</param>
        /// <returns>Expression[]</returns>
        private Expression[] GetArgumentExpressions(ParameterInfo[] parameters)
        {
            int parameterCount = parameters.Length;
            var argumentExpressions = new Expression[parameterCount];
            ConstantExpression iocContainerExpression = Expression.Constant(this);
          
            MethodInfo get_T_MethodInfo = typeof(MatchBox).GetMethod("Get");

            for (int i = 0; i < parameterCount; i++)
            {
                ParameterInfo parameter = parameters[i];
                // ioc.Get<T>() is a generic method, we need to set T to the parameter Type
                MethodInfo get_MethodInfo = get_T_MethodInfo.MakeGenericMethod(parameter.ParameterType);
                //now build an expression that represents  ioc<T>.Get()
                MethodCallExpression argument = Expression.Call(iocContainerExpression, get_MethodInfo);
               argumentExpressions[i] = argument;
            }
            return argumentExpressions;
        }
        /// <summary>
        /// Gets the ConstructorInfo for the required Type
        /// If there are more than one constructors, it returns the one with the most
        /// parameters
        /// </summary>
        /// <param name="implementationType">The required Type</param>
        /// <returns>ConstructorInfo</returns>
        private ConstructorInfo GetConstructorInfo(Type implementationType)
        {
            ConstructorInfo[] constructorInfos = implementationType.GetConstructors();
            int infoLength = constructorInfos.Length;
            if (infoLength == 0)
            {
                throw new InvalidOperationException(
                    "The type " + implementationType.FullName + " does not have a public constructor.");
            }
            if (infoLength == 1)
            {
                return constructorInfos[0];
            }
            //if there is more than one constructor
            //use the constructor with the most parameters
            int maxParameters = constructorInfos.Select(c => c.GetParameters().Length).Max();

            return constructorInfos.Single(c => c.GetParameters().Length == maxParameters);
        }
        /// <summary>
        /// Generates a delegate that creates an instance of TPublic
        /// </summary>
        /// <typeparam name="TPublic">The Type to generate</typeparam>
        /// <typeparam name="TImplementation">The particular instance of TPublic to generate</typeparam>
        /// <returns>The getter delegate for TPublic</returns>
        private Func<TPublic> GetFactoryMethod<TPublic, TImplementation>()
        {
            Type implementationType = typeof(TImplementation);
            ConstructorInfo constructorInfo = this.GetConstructorInfo(implementationType);
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            Expression[] argumentExpressions = this.GetArgumentExpressions(parameters);
           //build an expression that represents calling the constructor. The NodeType is 'New'
            NewExpression constructorCallingExpression = Expression.New(constructorInfo, argumentExpressions);
            // Can only compile expression trees that represent  lambda expressions.
            //So need to call  the following
            Expression<Func<TPublic>> lambdaExpression = Expression.Lambda<Func<TPublic>>(constructorCallingExpression);
            //Finally, compile the lambdaExpression into a Delegate and return it
          //  string checker = lambdaExpression.ToString();
            return lambdaExpression.Compile();
        }

        #endregion
    }
}