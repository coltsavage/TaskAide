using System;

namespace zCompany.TaskAide.UiTests
{
    public class NoIntervalAppFixture : IDisposable
    {
        // Constructors
        public NoIntervalAppFixture()
        {
            this.App = new UiApp();
        }

        // Destructors
        public void Dispose()
        {
            this.App.Dispose();
        }

        // Properties
        internal UiApp App { get; }
    }
}
