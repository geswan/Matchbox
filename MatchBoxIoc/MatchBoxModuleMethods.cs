namespace MatchBoxIoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public partial class MatchBox
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Loads & initialises dlls using the modules app.config file
        /// </summary>
        public void LoadExternalModules(IConfigurationService configurationService)
        {
            IEnumerable<ModuleDescription> moduleDescriptions =
                this.GetModuleDescriptionsFromConfig(configurationService);

            foreach (ModuleDescription moduleDescription in moduleDescriptions)
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(@".\" + moduleDescription.AssemblyFileName);
                Assembly externalAssembly = Assembly.Load(assemblyName);
                Type[] types = externalAssembly.GetTypes();
                string key = moduleDescription.ModuleTypeName;
                Type type = this.GetModuleType(types, key);
                object obj = Activator.CreateInstance(type);
                var matchBoxModule = obj as MatchBoxModule;
                if (matchBoxModule == null)
                {
                    throw new InvalidOperationException("Unable to create an instance of "+key);
                }
            //    ConstantExpression newExpression = Expression.Constant(obj);
            //    this.RegisterModule(type, newExpression);
                matchBoxModule.SetContainer(this);
                matchBoxModule.Initialize();
            }
        }

        #endregion

        #region Methods

        private IEnumerable<ModuleDescription> GetModuleDescriptionsFromConfig(
            IConfigurationService configurationService)
        {
            var moduleElements = configurationService.GetModuleElements();
            var moduleDescriptions = new List<ModuleDescription>();

            foreach (var element in moduleElements)
            {
                
                    var moduleDescription = this.Get<ModuleDescription>();
                    moduleDescription.AssemblyFileName = element.AssemblyName;
                    moduleDescription.ModuleTypeName = element.ModuleType;
                    moduleDescriptions.Add(moduleDescription);
                
            }
            if (moduleDescriptions.Count == 0)
            {
                throw new InvalidOperationException("No valid 'MatchBoxModules' element found in app.config");
            }

            return moduleDescriptions;
        }

        private Type GetModuleType(IEnumerable<Type> types, string key)
        {
            Type type = types.FirstOrDefault(t => t.Name == key);
            if (type == null)
            {
                throw new InvalidOperationException("Unable to find module " + key);
            }
            return type;
        }

        //private void RegisterModule(Type type, ConstantExpression expression)
        //{

        //    ConstantExpression newExpression = expression;
        //    Type funcType = typeof(Func<>).MakeGenericType(type);
        //    //()=>object
        //    LambdaExpression lambda = Expression.Lambda(funcType, newExpression);
        //    string checker = lambda.ToString();
        //    Delegate getter = lambda.Compile();
        //    this.RegisterType(type, getter);
        //}

        #endregion
    }
}