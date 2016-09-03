namespace MatchBoxIoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class MatchBox : IContainer
    {
        #region Constants and Fields

        private const string UNABLE_TO_FIND = "Unable to find an instance of the type: ";

        #endregion

        #region Constructors and Destructors

        public MatchBox()
        {
            this.Dictionary = new Dictionary<string, Delegate>();
            this.Register(() => new ModuleDescription());
        }

        #endregion

        #region Public Properties

        public Dictionary<string, Delegate> Dictionary { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets all instances of a given Type
        ///     Used by prism ServiceLocator
        /// </summary>
        /// <param name="serviceType">The Type to return</param>
        /// <returns>IEnumerable of objects</returns>
        public IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            Delegate getter;
            if (!this.Dictionary.TryGetValue(serviceType.Name, out getter))
            {
                throw new InvalidOperationException(UNABLE_TO_FIND + serviceType.Name);
            }
            return getter.GetInvocationList().Select(ob => ob.DynamicInvoke()).ToList();
        }

        /// <summary>
        ///     Returns an instance of the Type.
        ///     used by Prism Service Locator
        /// </summary>
        /// <param name="serviceType">The Type to return</param>
        /// <returns>The Type as an object</returns>
        public object DoGetInstance(Type serviceType)
        {
            Delegate getter;
            if (!this.Dictionary.TryGetValue(serviceType.Name, out getter))
            {
                throw new InvalidOperationException(UNABLE_TO_FIND + serviceType.FullName);
            }

            return getter.DynamicInvoke();
        }

        /// <summary>
        ///     Returns an instance of the Type
        /// </summary>
        /// <typeparam name="T">The Type to return</typeparam>
        /// <returns>The Type</returns>
        public T Get<T>()
        {
            Delegate getter;
            if (!this.Dictionary.TryGetValue(typeof(T).Name, out getter))
            {
                throw new InvalidOperationException(UNABLE_TO_FIND + typeof(T).FullName);
            }

            return (T)getter.DynamicInvoke();
        }

        /// <summary>
        ///     Gets a List  "T" of all types "T" in the container
        /// </summary>
        /// <typeparam name="T">The Type to find</typeparam>
        /// <returns>List  "T"</returns>
        public List<T> GetCollection<T>()
        {
            Delegate getter;
            if (!this.Dictionary.TryGetValue(typeof(T).Name, out getter))
            {
                throw new InvalidOperationException(UNABLE_TO_FIND + typeof(T).FullName);
            }
            return getter.GetInvocationList().Select(ob => (T)ob.DynamicInvoke()).ToList();
        }

        /// <summary>
        ///     Gets the main container items count
        /// </summary>
        /// <returns>int count</returns>
        public int GetDictionaryCount()
        {
            return this.Dictionary.Count;
        }

        /// <summary>
        ///     Returns a concrete Type
        /// </summary>
        /// <param name="key">the Type name as a string</param>
        /// <returns>The Type as an object</returns>
        public object GetInstanceFromString(string key)
        {
            Delegate getter;
            if (!this.Dictionary.TryGetValue(key, out getter))
            {
                throw new InvalidOperationException(UNABLE_TO_FIND + key);
            }

            return getter.DynamicInvoke();
        }

        #endregion
    }
}