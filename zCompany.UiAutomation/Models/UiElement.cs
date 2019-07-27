using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace zCompany.UiAutomation
{
    public class UiElement : IUiElement
    {
        // Fields
        private IUiElement externalUiElement;
        private VolatileState<int> height;
        private VolatileState<string> text;
        private VolatileState<int> width;
        private VolatileState<int> x;
        private VolatileState<int> y;

        // Constructors
        public UiElement(IUiElement element)
        {
            this.externalUiElement = (element is UiElement) ? ((UiElement)element).externalUiElement : element;

            this.height = new VolatileState<int>(() => this.externalUiElement.Height);
            this.text = new VolatileState<string>(() => this.externalUiElement.Text);
            this.width = new VolatileState<int>(() => this.externalUiElement.Width);
            this.x = new VolatileState<int>(() => this.externalUiElement.X);
            this.y = new VolatileState<int>(() => this.externalUiElement.Y);
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
        public int Height { get => this.height.Value; }

        public string Name { get => this.text.Value; }

        public string Text { get => this.text.Value; }

        public int Width { get => this.width.Value; }

        public int X { get => this.x.Value; }

        public int Y { get => this.y.Value; }

        // Methods
        public void Click()
        {
            this.externalUiElement.Click();
        }

        public void DoubleClick()
        {
            this.externalUiElement.DoubleClick();
        }

        public void Drag(Point start, Point offset)
        {
            this.externalUiElement.Drag(start, offset);
        }

        public void EnterText(string text)
        {
            this.externalUiElement.EnterText(text);
        }

        public IUiElement Find(By by)
        {
            return this.externalUiElement.Find(by);
        }

        public IUiElement Find(string automationId)
        {
            return this.externalUiElement.Find(automationId);
        }

        public IReadOnlyCollection<IUiElement> FindAll(By by)
        {
            return this.externalUiElement.FindAll(by);
        }

        public IReadOnlyCollection<IUiElement> FindAll(string automationId)
        {
            return this.externalUiElement.FindAll(automationId);
        }

        public virtual UiElement Refresh()
        {
            this.height.Invalidate();
            this.text.Invalidate();
            this.width.Invalidate();
            this.x.Invalidate();
            this.y.Invalidate();
            return this;
        }

        public void Resize(UiElement.Part part, int offset_ticks)
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

            this.Drag(start_pixels, offset_pixels);
        }
    }
}
