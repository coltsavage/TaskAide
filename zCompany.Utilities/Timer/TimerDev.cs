using System;

namespace zCompany.Utilities
{
    public class TimerDev : ITimer
    {
        // Fields
        private int counter;
        private bool listening;
        private readonly SystemTimeDev systemTime;

        // Constructors
        public TimerDev(SystemTimeDev systemTime)
        {
            this.systemTime = systemTime;
            this.systemTime.SecondElapsed += this.OnSecondElapsed;
            this.systemTime.Rewound += this.OnSystemRewound;
            this.counter = 0;
        }

        // Destructors
        ~TimerDev()
        {
            this.systemTime.SecondElapsed -= this.OnSecondElapsed;
            this.systemTime.Rewound -= this.OnSystemRewound;
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

        public void OnSystemRewound(object sender, RewoundEventArgs args)
        {
            var amount = args.Amount;
            this.counter -= (int)args.Amount.TotalSeconds;
        }
    }
}
