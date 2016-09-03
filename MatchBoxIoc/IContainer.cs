
namespace MatchBoxIoc
{
    using System;
    using System.Collections.Generic;

    //using Prism.Modularity;
    public interface IContainer
    {
        /// <summary>
        ///     Removes a registered Type
        /// </summary>
        /// <typeparam name="T">The Registered Type</typeparam>
        /// <returns>bool isSuccessful</returns>
        bool DeRegister<T>();

        /// <summary>
        ///     Returns an instance of the Type
        /// </summary>
        /// <param name="serviceType">The Type to return</param>
        /// <returns>The Type as an object</returns>
        object DoGetInstance(Type serviceType);

        /// <summary>
        ///     Returns an instance of the Type
        /// </summary>
        /// <typeparam name="T">The Type to return</typeparam>
        /// <returns>The Type</returns>
        T Get<T>();

        /// <summary>
        ///     Gets the main container items count
        /// </summary>
        /// <returns>int count</returns>
        int GetDictionaryCount();

        /// <summary>
        ///     Returns a concrete Type
        /// </summary>
        /// <param name="key">the Type name as a string</param>
        /// <returns>The Type as an object</returns>
        object GetInstanceFromString(string key);

        Dictionary<string, Delegate> Dictionary { get; }

       void  RegisterType(Type type, Delegate getter);

        ///// <summary>
        /////     Stores the given Module class
        ///// </summary>
        ///// <param name="module">IModule</param>
        //void LoadModule(IModule module);

        /// <summary>
        /// Loads Module dlls from Modules.config
        /// </summary>
       void LoadExternalModules(IConfigurationService configurationService);

        /// <summary>
        ///     Registers a Type as a transient
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
        /// <param name="getter">The delegate used to create the Type</param>
        void Register<T>(Func<T> getter);

        /// <summary>
        ///     Registers a Type as a singleton
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
        /// <param name="getter">The delegate used to create the Type</param>
        void RegisterSingleton<T>(Func<T> getter);

        /// <summary>
        ///     Registers a Type 
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
       void Register<T>();

        /// <summary>
        ///     Registers a Type and its implementation
        /// </summary>
        /// <typeparam name="TPublic">The Type to register</typeparam>
        /// <typeparam name="TImplementation">The implementation of the registered Type</typeparam>
        void Register<TPublic, TImplementation>();

        /// <summary>
        ///     Registers a Type, as a singleton
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
       void RegisterSingleton<T>();

        /// <summary>
        ///     Registers a Type and registers its implementation as a singleton
        /// </summary>
        /// <typeparam name="TPublic">The Type to register</typeparam>
        /// <typeparam name="TImplementation">The implementation of the registered Type</typeparam>
       void RegisterSingleton<TPublic, TImplementation>();

        /// <summary>
        ///     Registers a View, as a transient
        /// </summary>
        /// <typeparam name="TPublic">The Type to register</typeparam>
        /// <typeparam name="TImplementation">The implementation of the registered Type</typeparam>
        void RegisterView<TPublic, TImplementation>(Func<TImplementation> getter);

        /// <summary>
        ///     Registers a View, as a singleton
        /// </summary>
        /// <typeparam name="TPublic">The Type to register</typeparam>
        /// <typeparam name="TImplementation">The implementation of the registered Type</typeparam>
        void RegisterViewAsSingleton<TPublic, TImplementation>(Func<TImplementation> getter);

        /// <summary>
        /// Registers a Collection of implementation Types of Type T
        /// Will overwrite any existing registrations of Type T
        /// </summary>
        /// <typeparam name="T">The Type of the collection</typeparam>
        /// <param name="collection"></param>
        void RegisterCollection<T>(IEnumerable<Func<T>> collection);
        
        /// <summary>
        /// Gets a List  of all registered Types T
        /// </summary>
        /// <typeparam name="T">The Type of Collection to get</typeparam>
        /// <returns>A list of type T</returns>
        List<T> GetCollection<T>();

        /// <summary>
        /// Gets a List  of all registered Type serviceType 
        /// </summary>
        /// <param name="serviceType">The Type to get</param>
        /// <returns>An enumerable of objects</returns>
        IEnumerable<object> DoGetAllInstances(Type serviceType);
    }
}
