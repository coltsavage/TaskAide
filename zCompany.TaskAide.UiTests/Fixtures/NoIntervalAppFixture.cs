using System;

namespace zCompany.TaskAide.UiTests
{
    public class NoIntervalAppFixture : AppFixtureBase
    {
        // Constructors
        public NoIntervalAppFixture()
        {
            this.App = new UiApp(base.UiSession);
        }

        // Destructors
        public override void Dispose()
        {
            this.App.Dispose();
            base.Dispose();
        }

        // Properties
        internal UiApp App { get; }
    }
}
