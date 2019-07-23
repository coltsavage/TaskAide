﻿using System;

namespace zCompany.TaskAide.UiTests
{
    internal class UiInterval : UiElement
    {
        // Class Fields
        public static string ClassName = "Interval";

        // Fields
        private VolatileState<TimeSpan> start;
        private VolatileState<TimeSpan> span;

        // Constructors
        public UiInterval(UiElement element, UiChart chart)
            : base(element)
        {
            this.Chart = chart;

            this.start = new VolatileState<TimeSpan>(this.GetStart);
            this.span = new VolatileState<TimeSpan>(this.GetSpan);
        }

        // Destructors
        public override void Dispose()
        {
            base.Dispose();
        }

        // Properties
        public UiChart Chart { get; private set; }

        public TimeSpan Start { get => this.start.Value; }

        public TimeSpan Span { get => this.span.Value; }
               
        // Methods
        public new UiInterval Refresh()
        {
            base.Refresh();
            this.start.Invalidate();
            this.span.Invalidate();
            return this;
        }

        public override string ToString()
        {
            return $"{base.Name}: Start({this.Start}) Span({this.Span})";
        }

        // Helpers
        private TimeSpan GetSpan()
        {
            int pixelsPerMinute = 2;

            return TimeSpan.FromMinutes(base.Width / pixelsPerMinute);
        }

        private TimeSpan GetStart()
        {
            int pixelsPerMinute = 2;
            string timeFormat = "h\\:mm";

            var timeAtChartOrigin = TimeSpan.ParseExact(this.Chart.OriginLabel.Text, timeFormat, null);
            var intervalStartTimeOffset = TimeSpan.FromMinutes((this.X - this.Chart.X) / pixelsPerMinute);
            return timeAtChartOrigin + intervalStartTimeOffset;
        }
    }
}