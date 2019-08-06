using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class AppiumUiPointer : IUiPointer
    {
        // Fields
        private WindowsDriver<WindowsElement> driver;

        // Constructors
        public AppiumUiPointer(WindowsDriver<WindowsElement> driver)
        {
            this.driver = driver;
        }

        // Destructors
        public void Dispose()
        {

        }

        // Properties
        public Point AbsolutePosition { get => AppiumUiPointer.ScreenCursor.Position; }

        // Methods
        public IUiPointer Click()
        {
            new Actions(this.driver)
                .Click()
                .Perform();
            return this;
        }

        public IUiPointer DoubleClick()
        {
            new Actions(this.driver)
                .DoubleClick()
                .Perform();
            return this;
        }

        public IUiPointer DoubleClick(IUiElement element)
        {
            new Actions(this.driver)
                .MoveToElement(((AppiumUiElement)element.External).AppiumElement)
                .DoubleClick()
                .Perform();
            return this;
        }

        public IUiPointer Drag(Point offset)
        {
            new Actions(this.driver)
                .ClickAndHold()
                .MoveByOffset(offset.X, offset.Y)
                .Release()
                .Perform();
            return this;
        }

        public IUiPointer MoveOffset(Point offset)
        {
            new Actions(this.driver)
                .MoveByOffset(offset.X, offset.Y)
                .Perform();
            return this;
        }

        public IUiPointer MoveTo(IUiElement element)
        {
            new Actions(this.driver)
                .MoveToElement(((AppiumUiElement)element.External).AppiumElement)
                .Perform();
            return this;
        }

        public IUiPointer MoveTo(IUiElement element, Point offset)
        {
            new Actions(this.driver)
                .MoveToElement(((AppiumUiElement)element.External).AppiumElement, offset.X, offset.Y)
                .Perform();
            return this;
        }

        // Classes
        private static class ScreenCursor
        {
            // Class Properties
            public static Point Position
            {
                get
                {
                    var p = new User32Point();
                    ScreenCursor.GetCursorPos(ref p);
                    return new Point(p.x, p.y);
                }
            }
            
            // Structs
            private struct User32Point
            {
                public int x;
                public int y;
            }

            // Helpers
            [DllImport("user32.dll")]
            private static extern int GetCursorPos(ref User32Point pointRef);
        }
    }
}
