using System;
using System.Collections.Generic;

namespace zCompany.UiAutomation
{
    public class VolatileList<T> : List<T>
    {
        // Fields
        private InstantiateItem instantiateItem;
        private bool isValid;
        private RetrieveList retrieveList;

        // Constructors
        public VolatileList(RetrieveList listRetrieval, InstantiateItem itemInstantiation)
            :base()
        {
            this.retrieveList = listRetrieval;
            this.instantiateItem = itemInstantiation;

            this.isValid = false;            
        }

        // Delegates
        public delegate T InstantiateItem(IUiElement item);
        public delegate IReadOnlyCollection<IUiElement> RetrieveList();

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
            var readOnlyList = this.retrieveList();
            foreach (var item in readOnlyList)
            {
                base.Add(this.instantiateItem(item));
            }
            this.isValid = true;
            return this;
        }
    }
}
