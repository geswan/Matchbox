using System.Configuration;
namespace MatchBoxIoc
{
  public  class ModuleElement : ConfigurationElement
    {
        //assemblyName is the attribute in the config file
        [ConfigurationProperty("assemblyName", IsKey = true, IsRequired = true)]
        //AssemblyName is the ModuleElement property associated with the assemblyName attribute
        public string AssemblyName
        {
            get
            {
                return (string)this["assemblyName"];
            }
            set
            {
                this["assemblyName"] = value;
            }
        }
        [ConfigurationProperty("moduleType", IsKey = false, IsRequired = true)]
        public string ModuleType
        {
            get
            {
                return (string)this["moduleType"];
            }
            set
            {
                this["moduleType"] = value;
            }
        }
    }

  public class ModuleElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleElement)element).AssemblyName;
        }
    }

  public class MatchBoxModules : ConfigurationSection
    {
        [ConfigurationProperty("modules")]
        //Modules is the collection of ModuleElements
        public ModuleElementCollection Modules
        {
            get { return (ModuleElementCollection)this["modules"]; }
        }
    }
}
