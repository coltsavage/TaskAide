using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace zCompany.TaskAide.UiTests
{
    internal class UiComboBox : UiElement
    {
        // Class Fields
        public static string ClassName = "ComboBox";

        // Fields
        private VolatileList<UiElement> listItems;

        // Constructors
        public UiComboBox(UiElement element)
            :base(element)
        {
            this.listItems = new VolatileList<UiElement>(
                this.GetListItems,
                (item) => item);
        }
        
        // Destructors
        public override void Dispose()
        {
            base.Dispose();
        }

        // Properties
        public IReadOnlyCollection<string> ItemNames { get => this.ConvertToItemNames(this.Items); }

        private IReadOnlyCollection<UiElement> Items { get => this.listItems.Value; }
               
        // Methods
        public new UiComboBox Refresh()
        {
            base.Refresh();
            this.listItems.Invalidate();
            return this;
        }

        public bool Select(string itemName)
        {
            base.Click();
            var item = this.Items.First((i) => i.Text == itemName);
            item?.Click();
            return (item != null);
        }

        // Helpers
        private IReadOnlyCollection<string> ConvertToItemNames(IReadOnlyCollection<UiElement> items)
        {
            var list = new List<string>();
            foreach (var item in items)
            {
                list.Add(item.Text);
            }
            return list;
        }

        private IReadOnlyCollection<UiElement> GetListItems()
        {
            base.Click();
            return base.FindAll(By.ClassName("ComboBoxItem"));
        }
    }
}
