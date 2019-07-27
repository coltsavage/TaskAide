using System;

namespace zCompany.UiAutomation
{
    public class VolatileState<T>
    {
        // Fields
        private bool isValid;
        private RetrieveState retrievaState;
        private T state;

        // Constructors
        public VolatileState(RetrieveState stateRetrieval)
        {
            this.retrievaState = stateRetrieval;

            this.isValid = false;
        }

        // Delegates
        public delegate T RetrieveState();

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
            this.state = this.retrievaState();
            this.isValid = true;
            return this.state;
        }
    }
}
