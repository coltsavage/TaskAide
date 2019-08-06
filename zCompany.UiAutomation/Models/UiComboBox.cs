using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace zCompany.UiAutomation
{
    public class UiComboBox : UiElement
    {
        // Class Fields
        public static string ClassName = "ComboBox";

        // Fields
        private VolatileList<UiElement> listItems;

        // Constructors
        public UiComboBox(IUiElement element)
            :base(element)
        {
            this.listItems = new VolatileList<UiElement>(
                () => base.FindAll(By.ClassName("ComboBoxItem")),
                (item) => new UiElement(item));
        }

        // Properties
        public IReadOnlyCollection<string> ItemNames
        {
            get
            {
                this.Expand();
                var items = this.ConvertToItemNames(this.Items);
                this.Collapse();
                return items;
            }
        }

        public string ItemSelected { get => (base.Text != "") ? base.Text : null; }

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
            this.Expand();
            var item = this.Items.First((i) => i.Text == itemName);
            item?.Click();
            return (item != null);
        }

        // Helpers
        private void Collapse()
        {
            base.Pointer.MoveTo(base.External, new Point(-4, base.Height / 2)).Click();
        }

        private IReadOnlyCollection<string> ConvertToItemNames(IReadOnlyCollection<UiElement> items)
        {
            var list = new List<string>();
            foreach (var item in items)
            {
                list.Add(item.Text);
            }
            return list;
        }

        private void Expand()
        {
            base.Click();
        }
    }
}
