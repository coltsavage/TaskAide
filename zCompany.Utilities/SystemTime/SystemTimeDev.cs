using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;

namespace zCompany.Utilities
{
    public class SystemTimeDev : ISystemTime, INotifyPropertyChanged
    {
        // Fields
        private ThreadPoolTimer timer;
        private bool timerActive;

        // Constructors
        public SystemTimeDev(DateTimeOffset dateTimeUtc, TimeZoneInfo timeZone)
        {
            this.UtcNow = dateTimeUtc;
            this.LocalTimeZone = timeZone;
            this.SpeedMultiplier = 1;
        }

        // Destructors
        ~SystemTimeDev()
        {
            this.StopTimer();
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler SecondElapsed;

        // Properties
        public DateTimeOffset LocalDateTime
        {
            get => this.UtcNow.ToOffset(this.LocalTimeZone.BaseUtcOffset);
        }

        public TimeZoneInfo LocalTimeZone { get; private set; }

        private double speedMultiplier;
        public double SpeedMultiplier
        {
            get { return this.speedMultiplier; }
            set
            {
                this.speedMultiplier = value;
                this.RaisePropertyChanged();
            }
        }

        private DateTimeOffset utcNow;
        public DateTimeOffset UtcNow
        {
            get { return this.utcNow; }
            private set
            {
                this.utcNow = value;
                this.RaisePropertyChanged();
            }
        }

        // Methods
        public void FastForward(int mins)
        {
            bool wasActive = false;

            if (this.timerActive)
            {
                this.Pause();
                wasActive = true;
            }

            for (int i = 0; i < mins * 60; i++)
            {
                this.TimerElapsed();
            }

            if (wasActive)
            {
                this.Resume();
            }
        }

        public void Pause()
        {
            this.StopTimer();
        }

        public void Resume()
        {
            this.Start();
        }

        public void SlowDown()
        {
            this.SpeedMultiplier /= 2;
            if (this.timerActive)
            {
                this.Start();
            }
        }

        public void SpeedUp()
        {
            this.SpeedMultiplier *= 2;
            if (this.timerActive)
            {
                this.Start();
            }
        }

        public void Start()
        {
            this.StartTimer(TimeSpan.FromSeconds(1 / this.SpeedMultiplier));
        }

        // Helpers
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartTimer(TimeSpan period)
        {
            this.StopTimer();

            this.timer = ThreadPoolTimer.CreatePeriodicTimer(
                (timer) =>
                {
                    _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.High,
                            () =>
                            {
                                this.TimerElapsed();
                            });
                },
                period);
            this.timerActive = true;
        }

        private void StopTimer()
        {
            if (this.timer != null)
            {
                this.timer.Cancel();
            }
            this.timerActive = false;
        }

        private void TimerElapsed()
        {
            this.UtcNow += new TimeSpan(0, 0, 1);
            this.SecondElapsed?.Invoke(this, null);
        }
    }
}
