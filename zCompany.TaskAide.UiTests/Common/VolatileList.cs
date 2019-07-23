﻿using System;
using System.Collections.Generic;

namespace zCompany.TaskAide.UiTests
{
    internal class VolatileList<T> : List<T>
    {
        // Fields
        private bool isValid;

        // Constructors
        public VolatileList(Func<IReadOnlyCollection<UiElement>> listRetrieval, Func<UiElement, T> itemInstantiation)
            :base()
        {
            this.RetrieveList = listRetrieval;
            this.InstantiateItem = itemInstantiation;

            this.isValid = false;            
        }

        // Delegates
        private Func<UiElement, T> InstantiateItem;
        private Func<IReadOnlyCollection<UiElement>> RetrieveList;

        // Properties
        public IReadOnlyCollection<T> Value
        {
            get => (this.isValid) ? this : this.GetList();
        }

        // Methods
        public void Invalidate()
        {
            this.isValid = false;
            base.Clear();
        }

        // Helpers
        private IReadOnlyCollection<T> GetList()
        {
            var readOnlyList = this.RetrieveList();
            foreach (var item in readOnlyList)
            {
                base.Add(this.InstantiateItem(item));
            }
            this.isValid = true;
            return this;
        }
    }
}