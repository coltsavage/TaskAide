using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    public class IntervalResizeTests : IClassFixture<FiveIntervalAppFixture>, IDisposable
    {
        // Fields
        private UiApp app;
        private ITestOutputHelper output;

        // Constructors
        public IntervalResizeTests(FiveIntervalAppFixture fixture, ITestOutputHelper output)
        {
            this.app = fixture.App;
            this.output = output;
        }

        // Destructors
        public void Dispose()
        {
            this.Restore?.Invoke();
        }

        // Delegates
        private Action Restore;

        // Structs
        private struct IntervalData
        {
            public TimeSpan Start;
            public TimeSpan Span;

            public IntervalData(TimeSpan start, TimeSpan span)
            {
                this.Start = start;
                this.Span = span;
            }
        }

        // Tests
        [Fact]
        public void SlideEnd()
        {
            // Configure
            int indexSlidingInterval = 2;
            int slideOffset = 10;

            this.output.WriteLine($"interval acted on: {indexSlidingInterval}");
            this.output.WriteLine($"slideDelta: {slideOffset}");

            // Setup
            this.app.Chart.Refresh();
            var intervals = this.app.Chart.Intervals;

            this.output.WriteLine($"Intervals:");
            var initialValues = new List<IntervalData>();
            foreach (var i in intervals)
            {
                this.output.WriteLine($"    {i}");
                initialValues.Add(new IntervalData(i.Start, i.Span));
            }
            var slidingInterval = intervals.ElementAt(indexSlidingInterval);

            // Test
            slidingInterval.Click();
            slidingInterval.Resize(UiElement.Part.End, slideOffset);

            // Define Restore
            this.Restore = () =>
            {
                slidingInterval.Refresh();
                slidingInterval.Resize(UiElement.Part.End, -slideOffset);
                slidingInterval.Click();
            };

            // Confirm
            this.output.WriteLine($"---Post Test");
            var slideTimeSpan = new TimeSpan(0, slideOffset, 0);

            this.app.Chart.Refresh();

            this.output.WriteLine($"Intervals:");
            intervals = this.app.Chart.Intervals;
            for (int i = 0; i < intervals.Count; i++)
            {
                var interval = intervals.ElementAt(i);
                TimeSpan expectedStart = initialValues[i].Start;
                TimeSpan expectedSpan = initialValues[i].Span;

               if (i == indexSlidingInterval)
                {
                    expectedSpan += slideTimeSpan;
                }
                else if (i == indexSlidingInterval + 1)
                {
                    expectedStart += slideTimeSpan;
                    expectedSpan -= slideTimeSpan;
                }

                this.output.WriteLine($"    {interval}");
                this.output.WriteLine($"    ^^Expected: Start({expectedStart}) Span({expectedSpan})");

                var oneMinute = TimeSpan.FromMinutes(1);
                Assert.InRange(interval.Start, expectedStart - oneMinute, expectedStart + oneMinute);
                Assert.InRange(interval.Span, expectedSpan - oneMinute, expectedSpan + oneMinute);
            }
            this.output.WriteLine($"---Note: Appium cursor moves occasionally result in +/- 1 pixel requested");
        }

        [Fact]
        public void SlideFront()
        {
            // Configure
            int indexSlidingInterval = 2;
            int slideOffset = 10;

            this.output.WriteLine($"interval acted on: {indexSlidingInterval}");
            this.output.WriteLine($"slideDelta: {slideOffset}");

            // Setup
            this.app.Chart.Refresh();
            var intervals = this.app.Chart.Intervals;

            this.output.WriteLine($"Intervals:");
            var initialValues = new List<IntervalData>();
            foreach (var i in intervals)
            {
                this.output.WriteLine($"    {i}");
                initialValues.Add(new IntervalData(i.Start, i.Span));
            }
            var slidingInterval = intervals.ElementAt(indexSlidingInterval);
            
            // Test
            slidingInterval.Click();
            slidingInterval.Resize(UiElement.Part.Front, slideOffset);

            // Define Restore
            this.Restore = () =>
            {
                slidingInterval.Refresh();
                slidingInterval.Resize(UiElement.Part.Front, -slideOffset);
                slidingInterval.Click();
            };

            // Confirm
            this.output.WriteLine($"---Post Test");
            var slideTimeSpan = new TimeSpan(0, slideOffset, 0);

            this.app.Chart.Refresh();

            this.output.WriteLine($"Intervals:");
            intervals = this.app.Chart.Intervals;
            for (int i = 0; i < intervals.Count; i++)
            {
                var interval = intervals.ElementAt(i);
                TimeSpan expectedStart = initialValues[i].Start;
                TimeSpan expectedSpan = initialValues[i].Span;

                if (i == indexSlidingInterval)
                {
                    expectedStart += slideTimeSpan;
                    expectedSpan -= slideTimeSpan;
                }
                else if (i == indexSlidingInterval - 1)
                {
                    expectedSpan += slideTimeSpan;
                }

                this.output.WriteLine($"    {interval}");
                this.output.WriteLine($"    ^^Expected: Start({expectedStart}) Span({expectedSpan})");

                var oneMinute = TimeSpan.FromMinutes(1);
                Assert.InRange(interval.Start, expectedStart - oneMinute, expectedStart + oneMinute);
                Assert.InRange(interval.Span, expectedSpan - oneMinute, expectedSpan + oneMinute);
            }
            this.output.WriteLine($"---Note: Appium cursor moves occasionally result in +/- 1 pixel requested");
        }
    }
}
