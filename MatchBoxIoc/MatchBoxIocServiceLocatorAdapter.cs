namespace MatchBoxIoc
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ServiceLocation;

    public class MatchBoxIocServiceLocatorAdapter : ServiceLocatorImplBase
    {
        #region Constants and Fields

        private readonly IContainer ioc;

        #endregion

        #region Constructors and Destructors

        public MatchBoxIocServiceLocatorAdapter(IContainer kernel)
        {
            this.ioc = kernel;
        }

        #endregion

        #region Methods

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return this.ioc.DoGetAllInstances(serviceType);
        }

        //Prism usually accesses this method by Type 'serviceType' with the string parameter 'key' being null
        //But the RequestNavigate method uses the 'key' parameter to access the concrete Type
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (key != null)
            {
                return this.ioc.GetInstanceFromString(key);
            }

            object obj = this.ioc.DoGetInstance(serviceType);
            if (obj == null)
            {
                return null;
            }
            return obj;
        }

        #endregion
    }
}