using System;
using Windows.Storage;
using Windows.UI;

namespace zCompany.TaskAide.WindowsApp
{
    internal class AppSettings
    {
        // Class Fields
        static private Color[] Colors = { global::Windows.UI.Colors.Blue,
                                          global::Windows.UI.Colors.Orange,
                                          global::Windows.UI.Colors.Green,
                                          global::Windows.UI.Colors.Yellow,
                                          global::Windows.UI.Colors.Red };
        static private int ColorIndex = 0;

        // Fields
        private ApplicationDataContainer roamingSettings;

        // Constructors
        public AppSettings()
        {
            this.roamingSettings = ApplicationData.Current.RoamingSettings;
        }

        // Methods
        public Color GetTaskColor(ITask task)
        {
            Color color;

            var value = this.roamingSettings.Values[task.TID.ToString()];
            if (value == null)
            {
                color = AppSettings.Colors[AppSettings.ColorIndex++];
                this.SetTaskColor(task, color);
            }
            else
            {
                UInt32 colorHex = (UInt32)value;
                color.A = (byte)(colorHex >> 24);
                color.R = (byte)(colorHex >> 16);
                color.G = (byte)(colorHex >> 8);
                color.B = (byte)colorHex;
            }
            return color;
        }

        public void RemoveTask(ITask task)
        {
            this.roamingSettings.Values.Remove(task.TID.ToString());
        }

        public void SetTaskColor(ITask task, Color color)
        {
            UInt32 colorHex = 0;
            colorHex += (uint)color.A << 24;
            colorHex += (uint)color.R << 16;
            colorHex += (uint)color.G << 8;
            colorHex += (uint)color.B;
            this.roamingSettings.Values[task.TID.ToString()] = colorHex;
        }
    }
}
