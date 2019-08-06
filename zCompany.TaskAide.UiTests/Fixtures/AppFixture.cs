using System;
using Xunit;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    public class AppFixture : IDisposable
    {
        // Fields
        IUiSession uiSession;

        // Constructors
        public AppFixture()
        {
            this.uiSession = new AppiumUiSession();

            this.App = new UiApp(this.uiSession);
            this.Common = new Common(this.App);
        }

        // Destructors
        public void Dispose()
        {
            this.uiSession.Dispose();
        }

        // Properties
        internal UiApp App { get; private set; }

        internal Common Common { get; private set; }
    }


    [CollectionDefinition(UiTestsCollectionFixture.Name)]
    public class UiTestsCollectionFixture : ICollectionFixture<AppFixture>
    {
        public const string Name = "UiTestsCollection";
    }
}
