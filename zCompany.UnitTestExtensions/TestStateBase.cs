using System;

namespace zCompany.UnitTestExtensions
{
    public abstract class TestStateBase<T>
    {
        // Fields
        private bool testDefined;

        // Constructors
        public TestStateBase()
            : this(null)
        {

        }

        public TestStateBase(string name)
        {
            this.Name = name;
        }

        // Enums
        public enum Phase { Actual, Expected };

        // Properties
        private T actual;
        public virtual T Actual
        {
            get
            {
                return this.actual;
            }
            set
            {
                this.actual = value;
                this.OnAdd(TestStateBase<T>.Phase.Actual);
            }
        }

        private T expected;
        public virtual T Expected
        {
            get
            {
                return this.expected;
            }
            set
            {
                this.expected = value;
                this.OnAdd(TestStateBase<T>.Phase.Expected);
            }
        }

        protected string Name { get; private set; }

        private Action assert;
        public Action Assert
        {
            get
            {
                return this.assert;
            }
            set
            {
                this.assert = value;
                if (this.assert != null)
                {
                    this.testDefined = true;
                }
                else
                {
                    this.testDefined = false;
                }
            }
        }

        // Methods
        protected virtual void OnAdd(TestStateBase<T>.Phase phase)
        {

        }

        public void Test()
        {
            if (this.testDefined)
            {
                this.Assert();
            }
            else
            {
                Xunit.Assert.Equal<T>(this.Expected, this.Actual);
            }
        }
    }
}
