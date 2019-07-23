using System;
using System.Linq;

namespace zCompany.TaskAide.UiTests
{
    public class FiveIntervalAppFixture : IDisposable
    {
        // Constructors
        public FiveIntervalAppFixture()
        {
            this.App = new UiApp();

            this.MakeIntervals(5);
        }

        // Destructors
        public void Dispose()
        {
            this.App.Dispose();
        }

        // Properties
        internal UiApp App { get; }

        // Helpers
        private void MakeIntervals(int count)
        {
            var tasks = this.App.TaskSelector.ItemNames;
            for (int i = 0; i < count; i++)
            {
                var task = tasks.ElementAt(i % tasks.Count);
                this.App.TaskSelector.Select(task);
                this.App.Progress(60);
            }
        }
    }
}
