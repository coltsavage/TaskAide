using System;
using System.Drawing;

namespace zCompany.UiAutomation
{
    public interface IUiPointer : IDisposable
    {
        // Properties
        Point AbsolutePosition { get; }

        // Methods
        IUiPointer Click();

        IUiPointer DoubleClick();

        IUiPointer DoubleClick(IUiElement element);

        IUiPointer Drag(Point offset);

        IUiPointer MoveOffset(Point offset);

        IUiPointer MoveTo(IUiElement element);

        IUiPointer MoveTo(IUiElement element, Point offset);
    }
}