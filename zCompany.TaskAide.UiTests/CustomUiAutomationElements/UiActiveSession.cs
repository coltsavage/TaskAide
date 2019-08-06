using OpenQA.Selenium;
using System;
using zCompany.TaskAide.WindowsApp;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class UiActiveSession : UiElement
    {
        // Class Fields
        public static string ClassName = typeof(ActiveSession).Name;
        public static string NavigationEntryName = ActiveSessionNavigationViewItem.EntryName;

        // Fields
        private VolatileState<UiChart> chart;
        private VolatileState<UiDevTimeBar> devTimeBar;
        private VolatileState<UiComboBox> taskSelector;

        // Constructors
        public UiActiveSession(IUiElement element)
            : base(element)
        {
            this.chart = new VolatileState<UiChart>(
                () => new UiChart(base.Find(By.ClassName(UiChart.ClassName))));

            this.devTimeBar = new VolatileState<UiDevTimeBar>(
                () => new UiDevTimeBar(base.Find(By.ClassName(UiDevTimeBar.ClassName))));

            this.taskSelector = new VolatileState<UiComboBox>(
                () => new UiComboBox(base.Find("TaskListView")));
        }

        // Properties
        public UiDevTimeBar DevTimeBar { get => this.devTimeBar.Value; }

        public UiChart Session { get => this.chart.Value; }

        public UiComboBox TaskSelector { get => this.taskSelector.Value; }

        // Methods
        public UiActiveSession Navigate()
        {
            return this.Refresh();
        }

        public new UiActiveSession Refresh()
        {
            base.Refresh();
            this.chart.Invalidate();
            this.devTimeBar.Invalidate();
            this.taskSelector.Invalidate();
            return this;
        }
    }
}
