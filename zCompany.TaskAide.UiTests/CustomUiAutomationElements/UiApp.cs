using OpenQA.Selenium;
using System;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class UiApp
    {
        // Fields
        private VolatileState<UiActiveSession> activeSession;
        private IUiSession externalUiSession;
        private UiNavigation navigator;
        private VolatileState<UiSettings> settings;

        // Constructors
        public UiApp(IUiSession uiSession)
        {
            this.externalUiSession = uiSession;

            this.activeSession = new VolatileState<UiActiveSession>(
                () =>
                {
                    this.Navigate(UiNavigation.Content.ActiveSession);
                    return new UiActiveSession(this.externalUiSession.Find(By.ClassName(UiActiveSession.ClassName)));
                });

            this.settings = new VolatileState<UiSettings>(
                () =>
                {
                    this.Navigate(UiNavigation.Content.Settings);
                    return new UiSettings(this.externalUiSession.Find(By.ClassName(UiSettings.ClassName)));
                });
        }

        // Properties
        public UiActiveSession ActiveSession { get => this.activeSession.Value; }

        public UiSettings Settings { get => this.settings.Value; }

        // Methods
        public void Refresh()
        {
            this.activeSession.Invalidate();
            this.settings.Invalidate();
        }

        // Helpers
        private void Navigate(UiNavigation.Content content)
        {
            if (this.navigator == null)
            {
                this.navigator = new UiNavigation(this.externalUiSession.Find(By.ClassName(UiNavigation.ClassName)));
            }
            this.Refresh();
            this.navigator.Navigate(content);
        }
    }
}
