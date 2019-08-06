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

        // Constructors
        public UiApp(IUiSession uiSession)
        {
            this.externalUiSession = uiSession;

            this.activeSession = new VolatileState<UiActiveSession>(
                () => new UiActiveSession(this.externalUiSession.Find(By.ClassName(UiActiveSession.ClassName))));
        }

        // Properties
        public UiActiveSession ActiveSession { get => this.activeSession.Value; }

        // Methods
        public void Refresh()
        {
            this.activeSession.Invalidate();
        }
    }
}
