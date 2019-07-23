using System;
using zCompany.Utilities;
using zCompany.Windows.Charts;

namespace zCompany.TaskAide.WindowsApp
{
    internal class TimeLabelProvider : IAxisLabelProvider
    {
        // Fields
        IDateTimeZone currentTimeLabel;

        // Constructors
        public TimeLabelProvider(IDateTimeZone dateTime, int labelFrequencyInTicks)
        {
            this.FirstLabelOffsetFromStartInTicks = -(dateTime.LocalTime.Minutes % 30);
            this.currentTimeLabel = dateTime.Add(new TimeSpan(0, this.FirstLabelOffsetFromStartInTicks, -dateTime.LocalTime.Seconds));
            this.LabelFrequencyInTicks = labelFrequencyInTicks;
        }

        // Properties
        public int FirstLabelOffsetFromStartInTicks { get; private set; }

        public int LabelFrequencyInTicks { get; private set; }

        public string LabelText
        {
            get => this.currentTimeLabel.LocalTimeString("H:mm");
        }
        
        // Methods
        public void NextLabel()
        {
            currentTimeLabel = currentTimeLabel.Add(new TimeSpan(0, this.LabelFrequencyInTicks, 0));
        }
    }
}
