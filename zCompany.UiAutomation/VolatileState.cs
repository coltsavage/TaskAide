using System;

namespace zCompany.UiAutomation
{
    public class VolatileState<T>
    {
        // Fields
        private bool isValid;
        private T state;

        // Constructors
        public VolatileState(Func<T> stateRetrieval)
        {
            this.RetrieveState = stateRetrieval;

            this.isValid = false;
        }

        // Delegates
        private Func<T> RetrieveState;

        // Properties
        public T Value
        {
            get => (this.isValid) ? this.state : this.GetState();
        }

        // Methods
        public void Invalidate()
        {
            this.isValid = false;
        }

        // Helpers
        private T GetState()
        {
            this.state = this.RetrieveState();
            this.isValid = true;
            return this.state;
        }
    }
}
