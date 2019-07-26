using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;

namespace zCompany.TaskAide.UiTests
{
    internal class UiSession : IUiSession
    {
        // Constructors
        public UiSession()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "65512fbe-1595-484c-a9cb-54cba610a267_gxpjec3yzjwte!App");
            appCapabilities.SetCapability("deviceName", "dn");
            appCapabilities.SetCapability("newCommandTimeout", 180);

            this.Driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723/wd/hub"), appCapabilities);
        }

        // Destructors
        public void Dispose()
        {
            this.Driver.Quit();
        }

        // Properties
        public WindowsDriver<WindowsElement> Driver { get; private set; }

        // Methods
        public IUiElement Find(By by)
        {
            var element = this.Driver.FindElement(by);
            return new AppiumUiElement((AppiumWebElement)element, this);
        }

        public IUiElement Find(string automationId)
        {
            var element = this.Driver.FindElementByAccessibilityId(automationId);
            return new AppiumUiElement((AppiumWebElement)element, this);
        }
    }
}
