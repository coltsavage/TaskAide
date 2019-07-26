using System;

namespace zCompany.TaskAide.UiTests
{
    public class AppFixtureBase : IDisposable
    {
        // Constructors
        public AppFixtureBase()
        {
            this.UiSession = new UiSession();
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
