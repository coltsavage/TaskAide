using System;

namespace zCompany.UnitTestExtensions
{
    public class TestState<T> : TestStateBase<T>
    {
        // Constructors
        public TestState()
            : this(null)
        {

        }

        public TestState(string name)
            : base(name)
        {

        }

        // Methods
        protected override void OnAdd(TestStateBase<T>.Phase phase)
        {
            if (base.Name != null)
            {
                string phaseString = (phase == Phase.Actual) ? "Actual" : "Expected";
                T state = (phase == Phase.Actual) ? base.Actual : base.Expected;
                string stateStr = (state == null) ? "<none>" : state.ToString();
                TestLog.WriteLine($"{phaseString} {base.Name}: {stateStr}");
            }
        }
    }
}
