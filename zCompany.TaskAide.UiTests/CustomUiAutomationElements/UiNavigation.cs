using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using zCompany.TaskAide.WindowsApp;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class UiNavigation : UiElement
    {
        // Class Fields
        public static string ClassName = typeof(AppNavigation).Name;

        // Fields
        private VolatileList<UiElement> contents;

        // Constructors
        public UiNavigation(IUiElement element)
            : base(element)
        {
            this.contents = new VolatileList<UiElement>(
                () => base.FindAll(By.ClassName("ListViewItem")),
                (item) => new UiElement(item));
        }

        // Enums
        internal enum Content
        {
            ActiveSession,
            Settings
        }

        // Properties
        private IReadOnlyCollection<UiElement> Contents { get => this.contents.Value; }

        // Methods
        internal void Navigate(UiNavigation.Content content)
        {
            string contentName = null;
            switch (content)
            {
                case UiNavigation.Content.ActiveSession:
                    contentName = UiActiveSession.NavigationEntryName;
                    break;
                case UiNavigation.Content.Settings:
                    contentName = UiSettings.NavigationEntryName;
                    break;
                default:
                    break;
            }
            this.Contents.First((i) => i.Text == contentName).Click();
        }

        public new UiNavigation Refresh()
        {
            base.Refresh();
            this.contents.Invalidate();
            return this;
        }
    }
}
