using System;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    public class AppFixtureBase : IDisposable
    {
        // Constructors
        public AppFixtureBase()
        {
            this.UiSession = new AppiumUiSession();
        }

        // Destructors
        public virtual void Dispose()
        {
            this.UiSession.Dispose();
        }

        // Properties
        internal IUiSession UiSession { get; private set; }
    }
}
