using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMatchBox
{
    using System.Collections.Generic;

    using MatchBoxIoc;

    using NSubstitute;

    using TestModuleA;

    using TestModuleB;

    [TestClass]
    public class ModuleTest
    {
        private MatchBox ioc;

        private IConfigurationService mockConfigurationService;

        private List<ModuleElement> elements;

        [TestInitialize]
        public void TestInitialise()
        {
             ioc = new MatchBox();
            mockConfigurationService = Substitute.For<IConfigurationService>();
            elements = new List<ModuleElement>
            {
                new ModuleElement { AssemblyName = "TestModuleA.dll", ModuleType = "ModuleA" },
                new ModuleElement { AssemblyName = "TestModuleB.dll", ModuleType = "ModuleB" }
            };
        }


        [TestMethod]
        public void LoadExternalModulesLoadsAndInitialisestheModule()
        {
           mockConfigurationService.GetModuleElements().Returns(elements);
            ioc.LoadExternalModules(mockConfigurationService);
            var moduleA = ioc.Get<ModuleA>();
            var moduleB = ioc.Get<ModuleB>();

            Assert.IsTrue(moduleB.IsInitialized && moduleA.IsInitialized);

           
        }

        [TestMethod]
        public void LoadExternalModulesSetsTheModulesIoc()
        {
            mockConfigurationService.GetModuleElements().Returns(elements);
            ioc.LoadExternalModules(mockConfigurationService);
            var moduleA = ioc.Get<ModuleA>();
            var moduleB = ioc.Get<ModuleB>();
            Assert.AreSame( moduleA.ioc,ioc);
            Assert.AreSame(moduleB.ioc, ioc);
        }
    }
}
