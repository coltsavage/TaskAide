using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace zCompany.UiAutomation
{
    public static class Util
    {
        // Class Methods
        public static Point ScreenCursorPosition
        {
            get
            {
                var p = new User32Point();
                Util.GetCursorPos(ref p);
                return new Point(p.x, p.y);
            }
        }

        // Helpers
        [DllImport("user32.dll")]
        private static extern int GetCursorPos(ref User32Point pointRef);

        // Structs
        private struct User32Point
        {
            public int x;
            public int y;
        }
    }
}
