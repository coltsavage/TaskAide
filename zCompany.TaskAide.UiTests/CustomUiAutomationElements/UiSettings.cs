using OpenQA.Selenium;
using System;
using zCompany.TaskAide.WindowsApp;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class UiSettings : UiElement
    {
        // Class Fields
        public static string ClassName = typeof(Settings).Name;
        public static string NavigationEntryName = SettingsNavigationViewItem.EntryName;

        // Fields
        //private VolatileState<UiTaskSettings> taskSettings;

        // Constructors
        public UiSettings(IUiElement element)
            : base(element)
        {
            //this.taskSettings = new VolatileState<UiTaskSettings>(
            //    () => new UiTaskSettings(base.Find(By.ClassName(UiTaskSettings.ClassName))));
        }

        // Properties
        //public UiTaskSettings Tasks { get => this.taskSettings.Value; }

        // Methods
        public UiSettings Navigate()
        {
            return this.Refresh();
        }

        public new UiSettings Refresh()
        {
            base.Refresh();
            //this.taskSettings.Invalidate();
            return this;
        }
    }
}
