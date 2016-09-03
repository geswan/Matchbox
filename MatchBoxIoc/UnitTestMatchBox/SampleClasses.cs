namespace UnitTestMatchBox
{
    using System;

    public interface ISimpleClass
    {
        #region Public Properties

        IDependency Dependency { get; set; }

        bool IsConstructorCalled { get; set; }
        int Index { get; set; }

        #endregion
    }

    public sealed class SimpleClass : ISimpleClass
    {
        #region Constructors and Destructors

        public SimpleClass()
        {
            this.IsConstructorCalled = true;
        }

        #endregion

        #region Public Properties

        public IDependency Dependency { get; set; }

        public bool IsConstructorCalled { get; set; }
        public int Index { get; set; }

        #endregion
    }

    public interface IDependency
    {
        #region Public Methods and Operators

        void Write();

        #endregion
    }

    public class Dependency : IDependency
    {
        #region Public Methods and Operators

        public virtual void Write()
        {
            Console.WriteLine("Hello from dependency");
        }

        #endregion
    }

    public class ClassWithDependency
    {
        #region Constants and Fields

        public IDependency Dependency;

        #endregion

        #region Constructors and Destructors

        public ClassWithDependency(IDependency dependency)
        {
            this.Dependency = dependency;
            this.Dependency.Write();
        }

        #endregion
    }
}
