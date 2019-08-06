using System;
using System.Collections.Generic;

namespace zCompany.UnitTestExtensions
{
    public class TestStateList<T> : TestStateBase<List<T>>
    {
        // Constructors
        public TestStateList()
            : this(null)
        {

        }

        public TestStateList(string name)
            : base(name)
        {

        }

        // Helpers
        protected override void OnAdd(TestStateBase<List<T>>.Phase phase)
        {
            if (this.Name != null)
            {
                string phaseString = (phase == TestStateBase<List<T>>.Phase.Actual) ? "Actual" : "Expected";
                List<T> list = (phase == TestStateBase<List<T>>.Phase.Actual) ? base.Actual : base.Expected;
                TestLog.WriteLine($"{phaseString} {this.Name}:{((list.Count == 0) ? " <none>" : "")}");
                foreach(var i in list)
                {
                    TestLog.WriteLine($"    {i}");
                }
            }
        }
    }
}
