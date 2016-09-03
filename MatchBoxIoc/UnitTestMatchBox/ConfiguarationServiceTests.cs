using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatchBoxIoc;
namespace UnitTestMatchBox
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;

    //Itegration tests as reads app.config file
    [TestClass]
    public class ConfiguarationServiceTests
    {
        [TestMethod]
        public void GetModuleElementsReturnsModuleElements()
        {
            var expectedElements = new List<ModuleElement>
            {
                new ModuleElement { AssemblyName = "TestModuleA.dll", ModuleType = "ModuleA" },
                new ModuleElement { AssemblyName = "TestModuleB.dll", ModuleType = "ModuleB" }
            };
         ConfigurationService configurationService = new ConfigurationService();
            var elements = configurationService.GetModuleElements();
           CollectionAssert.AreEqual(elements,expectedElements);
        }

        [TestMethod]
        public void AppSettingsGetsNameValueCollection()
        {
            NameValueCollection expectedNameValueCollection=new NameValueCollection
            {
                { "FirstKey", "FirstValue" },
                { "SecondKey", "SecondValue" }
            };
            ConfigurationService configurationService = new ConfigurationService();

            var nameValueCollection = configurationService.AppSettings;
           // Assert.IsTrue(nameValueCollection["FirstKey"]=="FirstValue");
            CollectionAssert.AreEqual(expectedNameValueCollection, nameValueCollection);
        }

        [TestMethod]
        public void ConnectionStringsGetsConnectionStringSettingsCollection()
        {
             ConfigurationService configurationService = new ConfigurationService();
            var expectedConnectionStringsSettingsCollection = new ConnectionStringSettingsCollection
            {
                new ConnectionStringSettings
                {
                    Name = "ContextA",
                    ConnectionString = "Data Source=MyServerA;Initial Catalog=MyDbA;Integrated Security=SSPI;"
                },
                new ConnectionStringSettings
                {
                    Name = "ContextB",
                    ConnectionString = "Data Source=MyServerB;Initial Catalog=MyDbB;Integrated Security=SSPI;"
                },
            };
            var connectionStringsSettingsCollection = configurationService.ConnectionStrings;
            CollectionAssert.AreEqual(connectionStringsSettingsCollection, expectedConnectionStringsSettingsCollection);
        }
    }
}
