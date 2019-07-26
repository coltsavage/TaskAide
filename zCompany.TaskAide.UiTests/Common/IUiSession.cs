using OpenQA.Selenium;
using System;

namespace zCompany.TaskAide.UiTests
{
    internal interface IUiSession : IDisposable
    {
        // Properties
        IUiElement Find(By by);

        IUiElement Find(string automationId);
    }
}