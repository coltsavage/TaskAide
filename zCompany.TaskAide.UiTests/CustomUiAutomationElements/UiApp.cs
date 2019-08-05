using OpenQA.Selenium;
using System;
using System.Threading;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class UiApp : IDisposable
    {
        // Fields
        private VolatileState<UiChart> chart;
        private IUiSession externalUiSession;
        private VolatileState<UiComboBox> taskSelector;

        // Constructors
        public UiApp(IUiSession uiSession)
        {
            this.externalUiSession = uiSession;

            this.chart = new VolatileState<UiChart>(
                () => new UiChart(this.externalUiSession.Find(By.ClassName(UiChart.ClassName))));

            this.taskSelector = new VolatileState<UiComboBox>(
                () => new UiComboBox(this.externalUiSession.Find("TaskListView")));
        }

        // Destructors
        public void Dispose()
        {
            this.externalUiSession.Dispose();
        }

        // Properties
        public UiChart Chart { get => this.chart.Value; }

        public UiComboBox TaskSelector { get => this.taskSelector.Value; }

        // Methods
        public DateTimeOffset GetSystemDateTime()
        {
            var systemTime = this.externalUiSession.Find("TimeDisplay");
            var dateTime = DateTimeOffset.Parse(systemTime.Text);
            return dateTime - new TimeSpan(0, 0, dateTime.Second);
        }
        
        public void Progress(int minutes)
        {
            this.externalUiSession.Find("JumpAheadTextBox").EnterText(minutes.ToString());
            this.externalUiSession.Find("JumpAheadButton").Click();
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
