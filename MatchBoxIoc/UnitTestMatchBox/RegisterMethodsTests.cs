using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMatchBox
{
    using System;
    using System.Collections.Generic;

    using MatchBoxIoc;

    using NSubstitute;

    [TestClass]
    public class RegisterMethodsTests
    {
        [TestMethod]
        public void Register_As_Transient_Gets_A_Transient()
        {
            var ioc = new MatchBox();
            ioc.Register<ISimpleClass>(() => new SimpleClass());
            var transientA = ioc.Get<ISimpleClass>();
            var transientB = ioc.Get<ISimpleClass>();
            Assert.AreNotEqual(transientA, transientB);
        }


        [TestMethod]
        public void RegisterCollectionT_Gets_Expected_ListT()
        {
            var ioc = new MatchBox();
            ioc.RegisterCollection<ISimpleClass>(new List<Func<ISimpleClass>>
            {
                () => new SimpleClass{Index = 1},
                () => new SimpleClass{Index = 2},
                () => new SimpleClass{Index = 3}
            });
          List<ISimpleClass>simpleClasses=  ioc.GetCollection<ISimpleClass>();
          Assert.IsTrue(simpleClasses.Count == 3 
          && simpleClasses[0].Index == 1 
          && simpleClasses[1].Index == 2
          && simpleClasses[2].Index == 3);
        }

        [TestMethod]
        public void Register_Dependent_Type_As_Singleton_Gets_A_Singleton()
        {
            var ioc = new MatchBox();
            ioc.Register<IDependency>(() => new Dependency());
            ioc.RegisterSingleton<ClassWithDependency>();
            var classWithDepedencyA = ioc.Get<ClassWithDependency>();
            var classWithDepedencyB = ioc.Get<ClassWithDependency>();
            Assert.AreEqual(classWithDepedencyA, classWithDepedencyB);
        }

        [TestMethod]
        public void Register_Dependent_Type_As_Transient_Gets_A_Transient()
        {
            var ioc = new MatchBox();
            ioc.RegisterSingleton<IDependency>(() => new Dependency());
            ioc.Register<ClassWithDependency>();
            var classWithDepedencyA = ioc.Get<ClassWithDependency>();
            var classWithDepedencyB = ioc.Get<ClassWithDependency>();
            Assert.AreNotEqual(classWithDepedencyA, classWithDepedencyB);
        }

        [TestMethod]
        public void Register_View_As_Singleton_Gets_A_Singleton()
        {
            var ioc = new MatchBox();
            ioc.RegisterViewAsSingleton<ISimpleClass, SimpleClass>(() => new SimpleClass());
            var simpleClassA = ioc.Get<ISimpleClass>();
            var simpleClassB = ioc.Get<ISimpleClass>();
            Assert.IsTrue(simpleClassA == simpleClassB);
        }

        [TestMethod]
        public void Register_View_RegistersBothPublicAndImplementationType()
        {
            var ioc = new MatchBox();
            ioc.RegisterView<ISimpleClass, SimpleClass>(() => new SimpleClass());
            var simpleClassA = ioc.Get<ISimpleClass>();
            var simpleClassB = ioc.Get<SimpleClass>();
          
        }

        [TestMethod]
        public void RegisterSingleton_Gets_A_singleton()
        {
            var ioc = new MatchBox();
            ioc.RegisterSingleton<ISimpleClass>(() => new SimpleClass());
            var singletonA = ioc.Get<ISimpleClass>();
            var singletonB = ioc.Get<ISimpleClass>();
            Assert.AreEqual(singletonA, singletonB);
        }

        [TestMethod]
        public void Register_singleton_DoesNot_Call_constructor()
        {
            var ioc = new MatchBox();
            var dependency = Substitute.For<IDependency>();

            ioc.RegisterSingleton(() => new ClassWithDependency(dependency));
            dependency.DidNotReceive().Write();
        }
        [TestMethod]
        public void singleton_Calls_constructor_when_first_used()
        {
            var ioc = new MatchBox();
            var dependency = Substitute.For<IDependency>();

            ioc.RegisterSingleton(() => new ClassWithDependency(dependency));

            ioc.Get<ClassWithDependency>();

            dependency.Received().Write();
        }

        [TestMethod]
        public void singleton_Calls_constructor_only_when_first_used()
        {
            var ioc = new MatchBox();
            var dependency = Substitute.For<IDependency>();

            ioc.RegisterSingleton(() => new ClassWithDependency(dependency));

            ioc.Get<ClassWithDependency>();
            ioc.Get<ClassWithDependency>();
            dependency.Received(1).Write();
        }

        [TestMethod]
        public void RegisterWithObjectInitialiserInjectsObjectCorrectly()
        {
            var ioc = new MatchBox();
            IDependency dependency = new Dependency();
            ioc.Register(() => dependency);
            ioc.RegisterSingleton(() => new SimpleClass { Dependency = ioc.Get<IDependency>() });

            var classWithObjectInjection = ioc.Get<SimpleClass>();
            Assert.AreEqual(classWithObjectInjection.Dependency, dependency);
        }

        [TestMethod]
        public void RegisterClassWithDependencyAsParameterInConstructorInitialisesCorrectly()
        {
            var ioc = new MatchBox();
            IDependency dependency = new Dependency();
            ioc.Register(() => dependency);
            ioc.RegisterSingleton(() => new ClassWithDependency(ioc.Get<IDependency>()));

            var classWithDependency = ioc.Get<ClassWithDependency>();
            Assert.AreEqual(classWithDependency.Dependency, dependency);
        }

        [TestMethod]
        public void RegisterDependentTypeInjectsDependency()
        {
            var ioc = new MatchBox();
            IDependency dependency = new Dependency();
            ioc.Register(() => dependency);
            ioc.Register<ClassWithDependency>();

            var classWithDependency = ioc.Get<ClassWithDependency>();
            Assert.AreEqual(classWithDependency.Dependency, dependency);
        }
        [TestMethod]
        public void RegisterDependentTypeAsSingletonInjectsDependency()
        {
            var ioc = new MatchBox();
            IDependency dependency = new Dependency();
            ioc.Register(() => dependency);
            ioc.RegisterSingleton<ClassWithDependency>();

            var classWithDependency = ioc.Get<ClassWithDependency>();
            Assert.AreEqual(classWithDependency.Dependency, dependency);
        }

    }
}
