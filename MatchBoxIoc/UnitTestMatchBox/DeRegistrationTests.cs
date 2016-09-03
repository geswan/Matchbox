using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMatchBox
{
    using MatchBoxIoc;

    [TestClass]
    public class DeRegistrationTests
    {
        [TestMethod]

        public void DeRegister_Object_Removes_Object_From_Container()
        {
            var ioc = new MatchBox();
            ioc.RegisterViewAsSingleton<ISimpleClass, SimpleClass>(() => new SimpleClass());
            ioc.DeRegister<ISimpleClass>();
            Assert.IsFalse(ioc.Dictionary.ContainsKey(typeof(ISimpleClass).Name));

        }



        [TestMethod]
        public void DeRegister_Object_Returns_True_If_Successful()
        {
            var ioc = new MatchBox();
            ioc.RegisterViewAsSingleton<ISimpleClass, SimpleClass>(() => new SimpleClass());
            bool isSuccessful = ioc.DeRegister<ISimpleClass>();
            Assert.IsTrue(isSuccessful);

        }


        [TestMethod]
        public void DeRegister_Object_Returns_False_If_Unsuccessful()
        {
            var ioc = new MatchBox();
            bool isSuccessful = ioc.DeRegister<ISimpleClass>();
            Assert.IsFalse(isSuccessful);

        }
    }
}
