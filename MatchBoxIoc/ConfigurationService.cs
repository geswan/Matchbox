namespace MatchBoxIoc
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;

    public class ConfigurationService : IConfigurationService
    {
        #region Public Properties

        public NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                return ConfigurationManager.ConnectionStrings;
            }
        }

        #endregion

        #region Public Methods and Operators

        public T GetSection<T>()
        {
            return (T)ConfigurationManager.GetSection(typeof(T).Name);
        }

       public List<ModuleElement> GetModuleElements()
       {
            var matchboxModules = GetSection<MatchBoxModules>();
           return matchboxModules.Modules.OfType<ModuleElement>().ToList();
        }

        #endregion
    }
}