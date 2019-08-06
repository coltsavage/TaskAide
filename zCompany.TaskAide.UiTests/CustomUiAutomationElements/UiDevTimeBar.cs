using OpenQA.Selenium;
using System;
using zCompany.UiAutomation;

namespace zCompany.TaskAide.UiTests
{
    internal class UiDevTimeBar : UiElement
    {
        // Class Fields
        public static string ClassName = "DevTimeBar";

        // Fields
        private VolatileState<UiElement> fastForwardButton;
        private VolatileState<UiElement> jumpAmountButtonTextBox;
        private VolatileState<UiElement> removeLastIntervalButton;
        private VolatileState<UiElement> rewindButton;

        // Constructors
        public UiDevTimeBar(IUiElement element)
            : base(element)
        {
            this.fastForwardButton = new VolatileState<UiElement>(
                () => new UiElement(base.Find("FastForwardButton")));

            this.jumpAmountButtonTextBox = new VolatileState<UiElement>(
                () => new UiElement(base.Find("JumpAmountTextBox")));

            this.removeLastIntervalButton = new VolatileState<UiElement>(
                () => new UiElement(base.Find("RemoveLastIntervalButton")));

            this.rewindButton = new VolatileState<UiElement>(
                () => new UiElement(base.Find("RewindButton")));
        }

        // Properties
        public DateTimeOffset DateTime { get => this.GetDateTime(); }

        private UiElement FastForwardButton { get => this.fastForwardButton.Value; }

        private UiElement JumpAmountButtonTextBox { get => this.jumpAmountButtonTextBox.Value; }

        private UiElement RemoveLastIntervalButton { get => this.removeLastIntervalButton.Value; }

        private UiElement RewindButton { get => this.rewindButton.Value; }

        // Methods
        public void FastForward(int minutes)
        {
            this.JumpAmountButtonTextBox.EnterText(minutes.ToString());
            this.FastForwardButton.Click();
        }

        public void RemoveLastInterval()
        {
            this.RemoveLastIntervalButton.Click();
        }

        public new UiDevTimeBar Refresh()
        {
            base.Refresh();
            this.fastForwardButton.Invalidate();
            this.jumpAmountButtonTextBox.Invalidate();
            this.removeLastIntervalButton.Invalidate();
            this.rewindButton.Invalidate();
            return this;
        }

        public void Rewind(int minutes)
        {
            this.JumpAmountButtonTextBox.EnterText(minutes.ToString());
            this.RewindButton.Click();
        }

        // Helpers
        private DateTimeOffset GetDateTime()
        {
            var systemTime = base.Find("TimeDisplay");
            var dateTime = DateTimeOffset.Parse(systemTime.Text);
            return dateTime - new TimeSpan(0, 0, dateTime.Second);
        }
    }
}
