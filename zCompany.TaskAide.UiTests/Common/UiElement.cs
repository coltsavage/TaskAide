using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace zCompany.TaskAide.UiTests
{
    internal class UiElement : IDisposable
    {
        // Fields
        private VolatileState<int> height;
        private VolatileState<string> text;
        private VolatileState<int> width;
        private VolatileState<int> x;

        // Constructors
        public UiElement(UiElement element)
            : this(element.Element, element.UiSession) {}

        public UiElement(AppiumWebElement element, UiSession uiSession)
        {
            this.Element = element;
            this.UiSession = uiSession;

            this.height = new VolatileState<int>(() => this.Element.Size.Height);
            this.text = new VolatileState<string>(() => this.Element.Text);
            this.width = new VolatileState<int>(() => this.Element.Size.Width);
            this.x = new VolatileState<int>(() => this.Element.Location.X);
        }

        // Destructors
        public virtual void Dispose()
        {

        }

        // Enums
        public enum Part
        {
            Front,
            End
        }

        // Properties
        //public string AutomationId { get; private set; }

        public AppiumWebElement Element { get; private set; }

        public int Height { get => this.height.Value; }

        public string Name { get => this.text.Value; }

        public string Text { get => this.text.Value; }

        public UiSession UiSession { get; private set; }

        public int Width { get => this.width.Value; }

        public int X { get => this.x.Value; }

        //public int Y { get => this.Element.Location.Y; }

        // Methods
        public void Click()
        {
            this.Element.Click();
        }

        public void DoubleClick()
        {
            new Actions(this.UiSession.Driver)
                .MoveToElement(this.Element)
                .DoubleClick()
                .Perform();
        }

        public void EnterText(string text)
        {
            this.Element.SendKeys(text);
        }

        public UiElement Find(By by)
        {
            var appiumElement = this.Element.FindElement(by);
            return new UiElement((AppiumWebElement)appiumElement, this.UiSession);
        }

        public UiElement Find(string automationId)
        {
            var appiumElement = this.Element.FindElementByAccessibilityId(automationId);
            return new UiElement((AppiumWebElement)appiumElement, this.UiSession);
        }

        public IReadOnlyCollection<UiElement> FindAll(By by)
        {
            var appiumElements = this.Element.FindElements(by);
            return this.ConvertToUiElement(appiumElements);
        }

        public IReadOnlyCollection<UiElement> FindAll(string automationId)
        {
            var appiumElements = this.Element.FindElementsByAccessibilityId(automationId);
            return this.ConvertToUiElement(appiumElements);
        }

        public virtual UiElement Refresh()
        {
            this.height.Invalidate();
            this.text.Invalidate();
            this.width.Invalidate();
            this.x.Invalidate();
            return this;
        }

        public virtual void Resize(UiElement.Part part, int offset_ticks)
        {
            var bufferFromEdge_pixels = 4;
            var pixelsPerTick = 2;

            Point start_pixels;
            Point offset_pixels;
            switch (part)
            {
                case Part.Front:
                    start_pixels = new Point(bufferFromEdge_pixels, this.Height / 2);
                    offset_pixels = new Point(offset_ticks * pixelsPerTick, 0);
                    break;
                case Part.End:
                    start_pixels = new Point(this.Width - bufferFromEdge_pixels, this.Height / 2);
                    offset_pixels = new Point(offset_ticks * pixelsPerTick, 0);
                    break;
                default:
                    break;
            }

            this.Drag(this, start_pixels, offset_pixels);
        }

        // Helpers
        private ReadOnlyCollection<UiElement> ConvertToUiElement(ReadOnlyCollection<AppiumWebElement> appiumElements)
        {
            var uiElements = new List<UiElement>();
            foreach (var element in appiumElements)
            {
                uiElements.Add(new UiElement(element, this.UiSession));
            }
            return new ReadOnlyCollection<UiElement>(uiElements);
        }

        private void Drag(UiElement element, Point start_pixels, Point offset_pixels)
        {
            new Actions(this.UiSession.Driver)
                .MoveToElement(element.Element, start_pixels.X, start_pixels.Y)
                .Perform();

            var before = Util.ScreenCursorPosition;

            new Actions(this.UiSession.Driver)
                .ClickAndHold()
                .MoveByOffset(offset_pixels.X, offset_pixels.Y)
                .Release()
                .Perform();

            var after = Util.ScreenCursorPosition;
        }
    }
}
