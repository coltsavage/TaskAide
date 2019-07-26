using OpenQA.Selenium;
using System;
using System.Threading;

namespace zCompany.TaskAide.UiTests
{
    internal class UiApp : IDisposable
    {
        // Fields
        private VolatileState<UiChart> chart;
        private VolatileState<UiComboBox> taskSelector;
        private IUiSession ui;

        // Constructors
        public UiApp(IUiSession uiSession)
        {
            this.ui = uiSession;

            this.chart = new VolatileState<UiChart>(
                () => new UiChart(this.ui.Find(By.ClassName(UiChart.ClassName))));

            this.taskSelector = new VolatileState<UiComboBox>(
                () => new UiComboBox(this.ui.Find("TaskListView")));
        }

        // Destructors
        public void Dispose()
        {
            this.ui.Dispose();
        }

        // Properties
        public UiChart Chart { get => this.chart.Value; }

        public UiComboBox TaskSelector { get => this.taskSelector.Value; }

        // Methods
        public DateTimeOffset GetSystemDateTime()
        {
            var systemTime = this.ui.Find("TimeDisplay");
            var dateTime = DateTimeOffset.Parse(systemTime.Text);
            return dateTime - new TimeSpan(0, 0, dateTime.Second);
        }
        
        public void Progress(int minutes)
        {
            this.ui.Find("JumpAheadTextBox").EnterText(minutes.ToString());
            this.ui.Find("JumpAheadButton").Click();
        }

        public void Refresh()
        {
            this.chart.Invalidate();
            this.taskSelector.Invalidate();
        }

        public void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}
