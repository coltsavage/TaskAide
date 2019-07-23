using System;

namespace zCompany.Windows.Charts
{
    public interface IAxisLabelProvider
    {
        // Properties
        int FirstLabelOffsetFromStartInTicks { get; }

        int LabelFrequencyInTicks { get; }

        string LabelText { get; }
        
        // Methods
        void NextLabel();
    }
}
