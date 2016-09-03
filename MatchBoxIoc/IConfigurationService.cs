namespace MatchBoxIoc
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;

    public interface IConfigurationService
    {
        NameValueCollection AppSettings { get; }

        ConnectionStringSettingsCollection ConnectionStrings { get; }

        T GetSection<T>();

        List<ModuleElement> GetModuleElements();
    }
}