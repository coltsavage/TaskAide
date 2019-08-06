using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using zCompany.UiAutomation;
using zCompany.UnitTestExtensions;

namespace zCompany.TaskAide.UiTests
{
    [Collection(UiTestsCollectionFixture.Name)]
    public class ActiveSessionTests : IDisposable
    {
        // Fields
        private readonly UiApp app;
        private readonly Common common;

        // Constructors
        public ActiveSessionTests(AppFixture fixture, ITestOutputHelper output)
        {
            this.app = fixture.App;
            this.common = fixture.Common;

            TestLog.Output = output;
        }

        // Destructors
        public void Dispose()
        {
            this.Restore?.Invoke();
        }

        // Delegates
        private Action Restore;

        // Tests
        [Fact]
        public void StatePreservedOnReturnNavigation()
        {
            // Configure
            int config_IntervalCount = 2;
            int config_IntervalSpan = 60;

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");

            // Impacted State
            TestState<string> activeTaskName = new TestState<string>(nameof(activeTaskName));
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));

            // Setup
            this.app.ActiveSession.Navigate();
            this.common.MakeIntervals(config_IntervalCount, config_IntervalSpan);

            TestLog.WriteLine("---Define expected post-test state");
            {
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                    intervals.Expected = IntervalData.Copy(initialIntervals);
                }
                {
                    activeTaskName.Expected = this.app.ActiveSession.TaskSelector.ItemSelected;
                }
            }

            // Test
            this.app.Settings.Navigate();
            this.app.ActiveSession.Navigate();

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
                activeTaskName.Actual = this.app.ActiveSession.TaskSelector.ItemSelected;
            }

            Assert.Equal(activeTaskName.Expected, activeTaskName.Actual);
            Assert.Equal(intervals.Expected, intervals.Actual);
        }
    }
}
