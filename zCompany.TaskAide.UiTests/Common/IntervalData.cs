using System;
using System.Collections.Generic;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal struct IntervalData
    {
        // Class Methods
        internal static List<IntervalData> Copy(IReadOnlyCollection<UiInterval> uiIntervals)
        {
            return IntervalData.Copy(uiIntervals, uiIntervals.Count);
        }

        internal static List<IntervalData> Copy(IReadOnlyCollection<UiInterval> uiIntervals, int numToCopy)
        {
            var list = new List<IntervalData>();
            foreach (var i in uiIntervals)
            {
                if (numToCopy-- <= 0)
                {
                    break;
                }
                list.Add(new IntervalData(i.Start, i.Span, i.Name));
            }
            return list;
        }

        // Fields
        internal string Name;
        internal TimeSpan Span;
        internal TimeSpan Start;

        // Constructors
        internal IntervalData(TimeSpan start, TimeSpan span)
            : this(start, span, null)
        {

        }

        internal IntervalData(TimeSpan start, TimeSpan span, string name)
        {
            this.Start = start;
            this.Span = span;
            this.Name = name;
        }

        // Methods

        public bool Equals(IntervalData other)
        {
            return ((this.Start == other.Start) && (this.Span == other.Span));
        }

        public override bool Equals(object obj)
        {
            return (obj != null && obj is IntervalData) ? this.Equals((IntervalData)obj) : false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.Name}: Start({this.Start}) Span({this.Span})";
        }

        public static bool operator ==(IntervalData i1, IntervalData i2)
        {
            if (((object)i1) == null || ((object)i2) == null)
            {
                return Object.Equals(i1, i2);
            }

            return i1.Equals(i2);
        }

        public static bool operator !=(IntervalData i1, IntervalData i2)
        {
            return !(i1 == i2);
        }
    }
}
