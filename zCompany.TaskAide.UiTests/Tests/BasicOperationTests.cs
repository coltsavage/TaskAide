using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace zCompany.TaskAide.UiTests
{
    public class BasicOperationTests : IClassFixture<NoIntervalAppFixture>
    {
        // Fields
        private UiApp app;
        private ITestOutputHelper output;

        // Constructors
        public BasicOperationTests(NoIntervalAppFixture fixture, ITestOutputHelper output)
        {
            this.app = fixture.App;
            this.output = output;
        }

        // Destructors
        ~BasicOperationTests()
        {

        }

        // Tests
        [Fact]
        public void AddInterval_NewSession()
        {
            // Configure
            int chosenTaskIndex = 0;
            int minsProgressed = 30;

            this.output.WriteLine($"Index of Task getting interval: {chosenTaskIndex}");
            this.output.WriteLine($"Planned interval span: {minsProgressed}");

            // Setup
            var noIntervals = this.app.Chart.Intervals;
            Assert.Empty(noIntervals);

            this.output.WriteLine($"Tasks:");
            var taskNames = this.app.TaskSelector.ItemNames;
            foreach (var name in taskNames)
            {
                this.output.WriteLine($"    {name}");
            }
            Assert.NotEmpty(taskNames);

            var chosenTaskName = taskNames.ElementAt(chosenTaskIndex);
            this.output.WriteLine($"Task getting interval: {chosenTaskName}");

            // Test
            var success = this.app.TaskSelector.Select(chosenTaskName);
            Assert.True(success);

            // Confirm
            this.output.WriteLine($"---Post Test");

            var expectedStart = this.app.GetSystemDateTime().TimeOfDay;
            this.app.Progress(minsProgressed);
            var expectedSpan = this.app.GetSystemDateTime().TimeOfDay - expectedStart;

            this.app.Chart.Refresh();

            this.output.WriteLine($"Intervals:");
            foreach (var i in this.app.Chart.Intervals)
            {
                this.output.WriteLine($"    {i}");
            }

            this.output.WriteLine($"Intervals belonging to {chosenTaskName}:");
            var intervals =
                from i in this.app.Chart.Intervals
                where i.Name.Contains(chosenTaskName)
                select i;
            foreach (var i in intervals)
            {
                this.output.WriteLine($"    {i}");
            }
            Assert.NotEmpty(intervals);

            var interval = intervals.ElementAt(0);
            this.output.WriteLine($"Added interval: {interval.Name}");
            this.output.WriteLine($"  Start: {expectedStart} :: {interval.Start}");
            this.output.WriteLine($"  Span:  {expectedSpan} :: {interval.Span}");

            Assert.Equal(expectedStart, interval.Start);
            Assert.Equal(expectedSpan, interval.Span);
        }
    }
}
