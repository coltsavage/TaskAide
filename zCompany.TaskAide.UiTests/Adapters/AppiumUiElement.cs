using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class AppiumUiElement : IUiElement
    {
        // Fields
        AppiumWebElement uiElement;
        AppiumUiSession uiSession;

        // Constructors
        public AppiumUiElement(AppiumWebElement element, AppiumUiSession uiSession)
        {
            this.uiElement = element;
            this.uiSession = (AppiumUiSession)uiSession;
        }

        // Destructors
        public virtual void Dispose()
        {

        }

        // Properties
        internal AppiumWebElement AppiumElement { get => uiElement; }

        public IUiElement External { get => this; }

        public int Height { get => this.uiElement.Size.Height; }

        public string Name { get => this.uiElement.Text; }

        public IUiPointer Pointer { get => this.uiSession.Pointer; }

        public string Text { get => this.uiElement.Text; }

        public int Width { get => this.uiElement.Size.Width; }

        public int X { get => this.uiElement.Location.X; }

        public int Y { get => this.uiElement.Location.Y; }

        // Methods
        public void Click()
        {
            this.uiElement.Click();
        }

        public void DoubleClick()
        {
            new Actions(this.uiSession.Driver)
                .MoveToElement(this.uiElement)
                .DoubleClick()
                .Perform();
        }

        public void Drag(Point start, Point offset)
        {
            new Actions(this.uiSession.Driver)
                .MoveToElement(this.uiElement, start.X, start.Y)
                .Perform();

            new Actions(this.uiSession.Driver)
                .ClickAndHold()
                .MoveByOffset(offset.X, offset.Y)
                .Release()
                .Perform();
        }

        public void EnterText(string text)
        {
            this.uiElement.SendKeys(text);
        }

        public IUiElement Find(By by)
        {
            var element = this.uiElement.FindElement(by);
            return new AppiumUiElement((AppiumWebElement)element, this.uiSession);
        }

        public IUiElement Find(string automationId)
        {
            var element = this.uiElement.FindElementByAccessibilityId(automationId);
            return new AppiumUiElement((AppiumWebElement)element, this.uiSession);
        }

        public IReadOnlyCollection<IUiElement> FindAll(By by)
        {
            var elements = this.uiElement.FindElements(by);
            return this.ConvertToAppiumElement(elements);
        }

        public IReadOnlyCollection<IUiElement> FindAll(string automationId)
        {
            var elements = this.uiElement.FindElementsByAccessibilityId(automationId);
            return this.ConvertToAppiumElement(elements);
        }

        // Helpers
        private IReadOnlyCollection<IUiElement> ConvertToAppiumElement(IReadOnlyCollection<AppiumWebElement> appiumWebElements)
        {
            var uiElements = new List<IUiElement>();
            foreach (var element in appiumWebElements)
            {
                uiElements.Add(new AppiumUiElement(element, this.uiSession));
            }
            return new ReadOnlyCollection<IUiElement>(uiElements);
        }
    }
}
