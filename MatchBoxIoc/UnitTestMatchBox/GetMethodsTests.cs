namespace UnitTestMatchBox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MatchBoxIoc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetMethodsTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void DoGetAllInstances_Gets_Expected_IEmumerable()
        {
            var ioc = new MatchBox();
            ioc.RegisterCollection(
               new List<Func<ISimpleClass>>
                {
                    () => new SimpleClass { Index = 1 },
                    () => new SimpleClass { Index = 2 },
                    () => new SimpleClass { Index = 3 }
                });
        
           var simpleClasses = ioc.DoGetAllInstances(typeof(ISimpleClass)).Cast<ISimpleClass>().ToList();
           Assert.IsTrue(simpleClasses.Count == 3
         && simpleClasses[0].Index == 1
         && simpleClasses[1].Index == 2
         && simpleClasses[2].Index == 3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DoGetAllInstances_Throws_InvalidOperationException_When_Instance_Is_not_Registered()
        {
            var ioc = new MatchBox();
            var simpleClasses = ioc.DoGetAllInstances(typeof(ISimpleClass));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DoGetInstance_Throws_InvalidOperationException_When_Instance_Is_not_Registered()
        {
            var ioc = new MatchBox();

            ioc.DoGetInstance(typeof(ISimpleClass));
        }

        [TestMethod]
        public void GetInstanceFromStringReturnsInstanceWithSameTypeName()
        {
            var ioc = new MatchBox();
            ioc.Register(() => new SimpleClass());
            const string className = "SimpleClass";
            object simpleClass = ioc.GetInstanceFromString(className);
            Type classType = simpleClass.GetType();
            Assert.IsTrue(className == classType.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetInstanceFromStringThrowsInvalidOperationExceptionWhenStringIsNotFound()
        {
            var ioc = new MatchBox();
            ioc.GetInstanceFromString("SimpleClass");
        }

        [TestMethod]
        public void Get_I_T_Returns_TypeOf_T()
        {
            var ioc = new MatchBox();
            ioc.Register<ISimpleClass>(() => new SimpleClass());
            var simpleClass = ioc.Get<ISimpleClass>();
            Assert.IsInstanceOfType(simpleClass, typeof(SimpleClass));
        }

        [TestMethod]
        public void Get_T_Returns_T()
        {
            var ioc = new MatchBox();
            ioc.Register(() => new SimpleClass());
            var simpleClass = ioc.Get<SimpleClass>();
            Assert.IsInstanceOfType(simpleClass, typeof(SimpleClass));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Get_Throws_InvalidOperationException_When_Instance_Is_not_Registered()
        {
            var ioc = new MatchBox();

            ioc.Get<ISimpleClass>();
        }

        #endregion
    }
}