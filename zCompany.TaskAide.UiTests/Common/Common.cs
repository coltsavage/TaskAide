using System;
using System.Collections.Generic;
using System.Linq;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class Common
    {
        // Fields
        private UiApp app;

        // Constructors
        internal Common(UiApp app)
        {
            this.app = app;
        }

        // Methods
        internal string IntervalsTaskName(string intervalName)
        {
            return intervalName?.Split('#').First();
        }

        internal string IntervalsTaskName(IntervalData interval)
        {
            return this.IntervalsTaskName(interval.Name);
        }

        internal string IntervalsTaskName(UiInterval interval)
        {
            return this.IntervalsTaskName(interval.Name);
        }

        internal void MakeInterval(int span)
        {
            var tasks = this.app.ActiveSession.TaskSelector.ItemNames;
            this.MakeInterval(new Random().Next(tasks.Count), span);
        }

        internal void MakeInterval(int taskIndex, int span)
        {
            var tasks = this.app.ActiveSession.TaskSelector.ItemNames;
            this.app.ActiveSession.TaskSelector.Select(tasks.ElementAt(taskIndex % tasks.Count));
            this.app.ActiveSession.DevTimeBar.FastForward(span);
        }

        internal void MakeIntervals(int count, int span)
        {
            for (int i = 0; i < count; i++)
            {
                this.MakeInterval(i, span);
            }
        }

        internal void RemoveIntervals()
        {
            this.app.ActiveSession.Session.Refresh();
            this.RemoveIntervals(this.app.ActiveSession.Session.Intervals.Count);
        }

        internal void RemoveIntervals(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.app.ActiveSession.DevTimeBar.RemoveLastInterval();
            }
        }
    }
}
