﻿using System;
using System.Collections.Generic;
using System.Drawing;
using OpenQA.Selenium;

namespace zCompany.UiAutomation
{
    public interface IUiElement : IDisposable
    {
        // Properties
        IUiElement External { get; }

        int Height { get; }

        string Name { get; }

        IUiPointer Pointer { get; }

        string Text { get; }

        int Width { get; }

        int X { get; }

        int Y { get; }

        // Methods
        void Click();

        void DoubleClick();

        void EnterText(string text);

        IUiElement Find(By by);

        IUiElement Find(string automationId);

        IReadOnlyCollection<IUiElement> FindAll(By by);

        IReadOnlyCollection<IUiElement> FindAll(string automationId);
    }
}