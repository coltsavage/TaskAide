using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using zCompany.UiAutomation;
using zCompany.UnitTestExtensions;

namespace zCompany.TaskAide.UiTests
{
    [Collection(UiTestsCollectionFixture.Name)]
    public class DevTimeBarTests : IDisposable
    {
        // Fields
        private readonly UiApp app;
        private readonly Common common;

        // Constructors
        public DevTimeBarTests(AppFixture fixture, ITestOutputHelper output)
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
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void RemoveLastInterval_1(int remove)
        {
            // Configure
            int config_IntervalCount = 2;
            int config_IntervalSpan = 60;
            int config_IntervalsRemoved = remove;

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");
            TestLog.WriteLine($"Num of intervals removed: {config_IntervalsRemoved}");

            // Impacted State
            TestState<string> activeTaskName = new TestState<string>(nameof(activeTaskName));
            TestState<DateTimeOffset> appTime = new TestState<DateTimeOffset>(nameof(appTime));
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));

            // Setup
            this.app.ActiveSession.Refresh();
            this.common.MakeIntervals(config_IntervalCount, config_IntervalSpan);

            TestLog.WriteLine("---Define expected post-test state");
            {
                {
                    var initialTime = this.app.ActiveSession.DevTimeBar.DateTime;
                    TestLog.WriteLine($"Initial time: {initialTime}");
                    var multiplier = (config_IntervalsRemoved >= config_IntervalCount) ? config_IntervalCount : config_IntervalsRemoved;
                    appTime.Expected = initialTime - TimeSpan.FromMinutes(config_IntervalSpan * multiplier);
                }
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                    var copyCount = config_IntervalCount - config_IntervalsRemoved;
                    intervals.Expected = IntervalData.Copy(initialIntervals, copyCount);
                }
                {
                    activeTaskName.Expected = this.common.IntervalsTaskName(intervals.Expected.LastOrDefault());
                }
            }

            // Test
            for (int i = 0; i < config_IntervalsRemoved; i++)
            {
                this.app.ActiveSession.DevTimeBar.RemoveLastInterval();
            }

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                appTime.Actual = this.app.ActiveSession.DevTimeBar.DateTime;
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
                activeTaskName.Actual = this.app.ActiveSession.TaskSelector.ItemSelected;
            }

            Assert.Equal(appTime.Expected, appTime.Actual);
            Assert.Equal(activeTaskName.Expected, activeTaskName.Actual);
            Assert.Equal(intervals.Expected, intervals.Actual);
        }

        [Fact]
        public void RemoveLastInterval_2_Continue()
        {
            // Configure
            int config_IntervalCount = 2;
            int config_IntervalSpan = 60;
            int config_ActiveIntervalContinuedProgression = 30;

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");
            TestLog.WriteLine($"Active interval progression post successor's removal: {config_ActiveIntervalContinuedProgression}");

            // Impacted State
            TestState<string> activeTaskName = new TestState<string>(nameof(activeTaskName));
            TestState<DateTimeOffset> appTime = new TestState<DateTimeOffset>(nameof(appTime));
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));

            // Setup
            this.app.ActiveSession.Refresh();
            this.common.MakeIntervals(config_IntervalCount, config_IntervalSpan);

            TestLog.WriteLine("---Define expected post-test state");
            {
                {
                    var initialTime = this.app.ActiveSession.DevTimeBar.DateTime;
                    TestLog.WriteLine($"Initial time: {initialTime}");
                    var mins = config_ActiveIntervalContinuedProgression - config_IntervalSpan;
                    appTime.Expected = initialTime + TimeSpan.FromMinutes(mins);
                }
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                    var intervalData = IntervalData.Copy(initialIntervals, initialIntervals.Count - 1);

                    var index = config_IntervalCount - 2;
                    intervalData[index] = new IntervalData(
                        intervalData[index].Start,
                        intervalData[index].Span + TimeSpan.FromMinutes(config_ActiveIntervalContinuedProgression),
                        intervalData[index].Name);
                    intervals.Expected = intervalData;
                }
                {
                    activeTaskName.Expected = this.common.IntervalsTaskName(intervals.Expected.Last().Name);
                }
            }

            // Test
            this.app.ActiveSession.DevTimeBar.RemoveLastInterval();
            this.app.ActiveSession.DevTimeBar.FastForward(config_ActiveIntervalContinuedProgression);

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                appTime.Actual = this.app.ActiveSession.DevTimeBar.DateTime;
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
                activeTaskName.Actual = this.app.ActiveSession.TaskSelector.ItemSelected;
            }

            Assert.Equal(appTime.Expected, appTime.Actual);
            Assert.Equal(activeTaskName.Expected, activeTaskName.Actual);
            Assert.Equal(intervals.Expected, intervals.Actual);
        }

        [Fact]
        public void Rewind_1_WithinInterval()
        {
            // Configure
            int config_IntervalCount = 2;
            int config_IntervalSpan = 60;
            int config_RewindAmount = (int)(config_IntervalSpan * 0.5);

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");
            TestLog.WriteLine($"Amount rewound: {config_RewindAmount}");

            // Impacted State
            TestState<string> activeTaskName = new TestState<string>(nameof(activeTaskName));
            TestState<DateTimeOffset> appTime = new TestState<DateTimeOffset>(nameof(appTime));
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));

            // Setup
            this.app.ActiveSession.Refresh();
            this.common.MakeIntervals(config_IntervalCount, config_IntervalSpan);

            TestLog.WriteLine("---Define expected post-test state");
            {
                {
                    var initialTime = this.app.ActiveSession.DevTimeBar.DateTime;
                    TestLog.WriteLine($"Initial time: {initialTime}");
                    appTime.Expected = initialTime - TimeSpan.FromMinutes(config_RewindAmount);
                }
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                    var intervalData = IntervalData.Copy(initialIntervals);

                    var index = config_IntervalCount - 1;
                    intervalData[index] = new IntervalData(
                        intervalData[index].Start,
                        intervalData[index].Span - TimeSpan.FromMinutes(config_RewindAmount),
                        intervalData[index].Name);
                    intervals.Expected = intervalData;
                }
                {
                    activeTaskName.Expected = this.common.IntervalsTaskName(intervals.Expected.Last().Name);
                }
            }

            // Test
            this.app.ActiveSession.DevTimeBar.Rewind(config_RewindAmount);

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                appTime.Actual = this.app.ActiveSession.DevTimeBar.DateTime;
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
                activeTaskName.Actual = this.app.ActiveSession.TaskSelector.ItemSelected;
            }

            Assert.Equal(appTime.Expected, appTime.Actual);
            Assert.Equal(activeTaskName.Expected, activeTaskName.Actual);
            Assert.Equal(intervals.Expected, intervals.Actual);
        }

        [Fact]
        public void Rewind_2_BeyondInterval()
        {
            // Configure
            int config_IntervalCount = 2;
            int config_IntervalSpan = 60;
            int config_RewindAmount = (int)(config_IntervalSpan * 1.5);

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");
            TestLog.WriteLine($"Amount rewound: {config_RewindAmount}");

            // Impacted State
            TestState<string> activeTaskName = new TestState<string>(nameof(activeTaskName));
            TestState<DateTimeOffset> appTime = new TestState<DateTimeOffset>(nameof(appTime));
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));

            // Setup
            this.app.ActiveSession.Refresh();
            this.common.MakeIntervals(config_IntervalCount, config_IntervalSpan);

            TestLog.WriteLine("---Define expected post-test state");
            {
                {
                    var initialTime = this.app.ActiveSession.DevTimeBar.DateTime;
                    TestLog.WriteLine($"Initial time: {initialTime}");
                    appTime.Expected = initialTime - TimeSpan.FromMinutes(config_RewindAmount);
                }
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                    var intervalData = IntervalData.Copy(initialIntervals, initialIntervals.Count - 1);

                    var index = config_IntervalCount - 2;
                    intervalData[index] = new IntervalData(
                        intervalData[index].Start,
                        intervalData[index].Span - TimeSpan.FromMinutes(config_RewindAmount - config_IntervalSpan),
                        intervalData[index].Name);
                    intervals.Expected = intervalData;
                }
                {
                    activeTaskName.Expected = this.common.IntervalsTaskName(intervals.Expected.Last().Name);
                }
            }

            // Test
            this.app.ActiveSession.DevTimeBar.Rewind(config_RewindAmount);

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                appTime.Actual = this.app.ActiveSession.DevTimeBar.DateTime;
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
                activeTaskName.Actual = this.app.ActiveSession.TaskSelector.ItemSelected;
            }

            Assert.Equal(appTime.Expected, appTime.Actual);
            Assert.Equal(activeTaskName.Expected, activeTaskName.Actual);
            Assert.Equal(intervals.Expected, intervals.Actual);
        }

        [Fact]
        public void Rewind_3_BeyondAllIntervals()
        {
            // Configure
            int config_IntervalCount = 2;
            int config_IntervalSpan = 60;
            int config_RewindAmount = (int)(config_IntervalSpan * (config_IntervalCount + 0.5));

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");
            TestLog.WriteLine($"Amount rewound: {config_RewindAmount}");

            // Impacted State
            TestState<string> activeTaskName = new TestState<string>(nameof(activeTaskName));
            TestState<DateTimeOffset> appTime = new TestState<DateTimeOffset>(nameof(appTime));
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));

            // Setup
            this.app.ActiveSession.Refresh();
            this.common.MakeIntervals(config_IntervalCount, config_IntervalSpan);
            
            TestLog.WriteLine("---Define expected post-test state");
            {
                {
                    var initialTime = this.app.ActiveSession.DevTimeBar.DateTime;
                    TestLog.WriteLine($"Initial time: {initialTime}");
                    appTime.Expected = initialTime - TimeSpan.FromMinutes(config_IntervalCount * config_IntervalSpan);
                }
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                    intervals.Expected = IntervalData.Copy(initialIntervals, 0);
                }
                {
                    activeTaskName.Expected = null;
                }
            }

            // Test
            this.app.ActiveSession.DevTimeBar.Rewind(config_RewindAmount);

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                appTime.Actual = this.app.ActiveSession.DevTimeBar.DateTime;
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
                activeTaskName.Actual = this.app.ActiveSession.TaskSelector.ItemSelected;
            }

            Assert.Equal(appTime.Expected, appTime.Actual);
            Assert.Equal(activeTaskName.Expected, activeTaskName.Actual);
            Assert.Equal(intervals.Expected, intervals.Actual);
        }
    }
}
