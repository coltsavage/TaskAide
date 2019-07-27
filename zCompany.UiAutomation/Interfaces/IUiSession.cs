using OpenQA.Selenium;
using System;

namespace zCompany.UiAutomation
{
    public interface IUiSession : IDisposable
    {
        // Properties
        IUiElement Find(By by);

        IUiElement Find(string automationId);
    }
}