using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.Drawing;
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
    }
}
