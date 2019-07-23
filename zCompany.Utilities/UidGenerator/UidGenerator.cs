using System;
using Windows.Storage;

namespace zCompany.Utilities
{
    public class UidGenerator
    {
        // Fields
        private ApplicationDataContainer container;
        private Func<int, bool> isUidAvailable;
        private string key;

        // Constructors
        public UidGenerator(ApplicationDataContainer container, string key, Func<int, bool> isUidAvailable)
        {
            this.container = container;
            this.key = key;
            this.isUidAvailable = isUidAvailable;

            if (this.container.Values[key] == null)
            {
                this.container.Values[key] = 1;
            }            
        }

        // Methods
        public int NextUid()
        {
            var uid = (int)this.container.Values[this.key];
            while (!this.isUidAvailable(uid))
            {
                uid++;
            }
            this.container.Values[key] = uid;
            return uid;
        }
    }
}
