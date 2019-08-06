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
    public class SessionTests : IDisposable
    {
        // Fields
        private readonly UiApp app;
        private readonly Common common;

        // Constructors
        public SessionTests(AppFixture fixture, ITestOutputHelper output)
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
        public void IntervalAdd()
        {
            // Configure
            int config_IntervalCount = 1;
            int config_IntervalSpan = 60;
            int config_TaskIndex = 1;

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");
            TestLog.WriteLine($"Task index to create interval: {config_TaskIndex}");

            // Impacted State
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));
            TestState<string> intervalTask = new TestState<string>(nameof(intervalTask));

            // Setup
            this.app.ActiveSession.Navigate();

            TestLog.WriteLine("---Define expected post-test state");
            {
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                }
                {
                    var taskNames = this.app.ActiveSession.TaskSelector.ItemNames;
                    TestLog.OutputCollection("Tasks", taskNames);
                    Assert.NotEmpty(taskNames);
                    intervalTask.Expected = taskNames.ElementAt(config_TaskIndex);
                }
                {
                    var intervalData = new List<IntervalData>();
                    intervalData.Add(new IntervalData(
                        this.app.ActiveSession.DevTimeBar.DateTime.TimeOfDay,
                        TimeSpan.FromMinutes(config_IntervalSpan)));
                    intervals.Expected = intervalData;
                }
            }

            // Test
            var success = this.app.ActiveSession.TaskSelector.Select(intervalTask.Expected);
            Assert.True(success);
            this.app.ActiveSession.DevTimeBar.FastForward(config_IntervalSpan);

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
                intervalTask.Actual = this.common.IntervalsTaskName(intervals.Actual.LastOrDefault());
            }

            Assert.Equal(intervalTask.Expected, intervalTask.Actual);
            Assert.Equal(intervals.Expected, intervals.Actual);
        }

        [Fact]
        public void IntervalResize_SlideEnd()
        {
            // Configure
            int config_IntervalCount = 3;
            int config_IntervalSpan = 60;
            int config_IndexSlidingInterval = 1;
            int config_SlideOffset = 10;

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");
            TestLog.WriteLine($"Interval index to slide: {config_IndexSlidingInterval}");
            TestLog.WriteLine($"Slide delta: {config_SlideOffset}");

            // Impacted State
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));

            // Setup
            this.app.ActiveSession.Navigate();
            this.common.MakeIntervals(config_IntervalCount, config_IntervalSpan);

            TestLog.WriteLine("---Define expected post-test state");
            {
                List<IntervalData> intervalData;
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                    intervalData = IntervalData.Copy(initialIntervals);
                }

                var slideTimeSpan = TimeSpan.FromMinutes(config_SlideOffset);
                {
                    var index = config_IndexSlidingInterval;
                    intervalData[index] = new IntervalData(
                        intervalData[index].Start,
                        intervalData[index].Span + slideTimeSpan,
                        intervalData[index].Name);

                    index++;
                    intervalData[index] = new IntervalData(
                        intervalData[index].Start + slideTimeSpan,
                        intervalData[index].Span - slideTimeSpan,
                        intervalData[index].Name);
                }
                intervals.Expected = intervalData;
            }

            // Test
            var slidingInterval = this.app.ActiveSession.Session.Intervals.ElementAt(config_IndexSlidingInterval);
            slidingInterval.Click();
            slidingInterval.Resize(UiElement.Part.End, config_SlideOffset);

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
            }

            TestLog.WriteLine($"---Note: Appium cursor moves occasionally result in +/- 1 pixel of requested");
            var oneMinute = TimeSpan.FromMinutes(1);
            for (int i = 0; i < intervals.Actual.Count; i++)
            {
                Assert.InRange(
                    intervals.Actual.ElementAt(i).Start,
                    intervals.Expected.ElementAt(i).Start - oneMinute,
                    intervals.Expected.ElementAt(i).Start + oneMinute);

                Assert.InRange(
                    intervals.Actual.ElementAt(i).Span,
                    intervals.Expected.ElementAt(i).Span - oneMinute,
                    intervals.Expected.ElementAt(i).Span + oneMinute);
            }
        }

        [Fact]
        public void IntervalResize_SlideFront()
        {
            // Configure
            int config_IntervalCount = 3;
            int config_IntervalSpan = 60;
            int config_IndexSlidingInterval = 1;
            int config_SlideOffset = 10;

            TestLog.WriteLine("---Test config:");
            TestLog.WriteLine($"Intervals: {config_IntervalCount}");
            TestLog.WriteLine($"Interval spans: {config_IntervalSpan}");
            TestLog.WriteLine($"Interval index to slide: {config_IndexSlidingInterval}");
            TestLog.WriteLine($"Slide delta: {config_SlideOffset}");

            // Impacted State
            TestStateList<IntervalData> intervals = new TestStateList<IntervalData>(nameof(intervals));

            // Setup
            this.app.ActiveSession.Navigate();
            this.common.MakeIntervals(config_IntervalCount, config_IntervalSpan);

            TestLog.WriteLine("---Define expected post-test state");
            {
                List<IntervalData> intervalData;
                {
                    var initialIntervals = this.app.ActiveSession.Session.Intervals;
                    TestLog.OutputCollection<UiInterval>("Initial intervals", initialIntervals);
                    intervalData = IntervalData.Copy(initialIntervals);
                }

                var slideTimeSpan = TimeSpan.FromMinutes(config_SlideOffset);
                {
                    var index = config_IndexSlidingInterval - 1;
                    intervalData[index] = new IntervalData(
                        intervalData[index].Start,
                        intervalData[index].Span + slideTimeSpan,
                        intervalData[index].Name);

                    index++;
                    intervalData[index] = new IntervalData(
                        intervalData[index].Start + slideTimeSpan,
                        intervalData[index].Span - slideTimeSpan,
                        intervalData[index].Name);
                }
                intervals.Expected = intervalData;
            }
            // Test
            var slidingInterval = this.app.ActiveSession.Session.Intervals.ElementAt(config_IndexSlidingInterval);
            slidingInterval.Click();
            slidingInterval.Resize(UiElement.Part.Front, config_SlideOffset);

            // Define Restore
            this.Restore = () => this.common.RemoveIntervals();

            // Confirm
            this.app.ActiveSession.Refresh();

            TestLog.WriteLine($"---Post-test state");
            {
                intervals.Actual = IntervalData.Copy(this.app.ActiveSession.Session.Intervals);
            }

            TestLog.WriteLine($"---Note: Appium cursor moves occasionally result in +/- 1 pixel of requested");
            var oneMinute = TimeSpan.FromMinutes(1);
            for (int i = 0; i < intervals.Actual.Count; i++)
            {
                Assert.InRange(
                    intervals.Actual.ElementAt(i).Start,
                    intervals.Expected.ElementAt(i).Start - oneMinute,
                    intervals.Expected.ElementAt(i).Start + oneMinute);

                Assert.InRange(
                    intervals.Actual.ElementAt(i).Span,
                    intervals.Expected.ElementAt(i).Span - oneMinute,
                    intervals.Expected.ElementAt(i).Span + oneMinute);
            }
        }
    }
}
