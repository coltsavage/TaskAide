using System;

namespace zCompany.Utilities
{
    public class TimerDev : ITimer
    {
        // Fields
        int counter;
        bool listening;
        SystemTimeDev systemTime;

        // Constructors
        public TimerDev(SystemTimeDev systemTime)
        {
            this.systemTime = systemTime;
            this.systemTime.SecondElapsed += this.OnSecondElapsed;
            this.counter = 0;
        }

        // Destructors
        ~TimerDev()
        {
            this.systemTime.SecondElapsed -= this.OnSecondElapsed;
        }

        // Delegates
        public Action SecondElapsed { private get; set; }

        // Properties
        public TimeSpan SecondsElapsed
        {
            get => TimeSpan.FromSeconds(this.counter);
        }

        // Methods
        public void Start()
        {
            this.listening = true;
        }

        public void Stop()
        {
            this.listening = false;
        }

        // Event Handlers
        private void OnSecondElapsed(object sender, EventArgs args)
        {
            if (this.listening)
            {
                this.counter++;
                this.SecondElapsed?.Invoke();
            }
        }
    }
}
