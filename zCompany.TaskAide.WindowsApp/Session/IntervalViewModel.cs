using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using zCompany.Windows.Charts;

namespace zCompany.TaskAide.WindowsApp
{
    internal class IntervalViewModel : IIntervalViewModel
    {
        // Constructors
        public IntervalViewModel(IInterval model)
        {
            this.Model = model;
            this.Model.PropertyChanged += this.OnModelPropertyChanged;

            this.Start = (int)Math.Ceiling(model.Start.TotalMinutes);
            this.Span = (int)Math.Ceiling(model.Span.TotalMinutes);
        }

        // Destructors
        ~IntervalViewModel()
        {
            this.Model.PropertyChanged -= this.OnModelPropertyChanged;
        }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties

        public IInterval Model { get; private set; }

        private int span;
        public int Span
        {
            get { return this.span; }
            set
            {
                this.span = value;
                this.RaisePropertyChanged();
            }
        }

        private int start;
        public int Start
        {
            get { return this.start; }
            set
            {
                this.start = value;
                this.RaisePropertyChanged();
            }
        }

        // Event Handlers
        protected virtual void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Start")
            {
                var newStart = (int)Math.Ceiling(this.Model.Start.TotalMinutes);
                if (newStart != this.Start)
                {
                    this.Start = newStart;
                }
            }
            if (args.PropertyName == "Span")
            {
                var newSpan = (int)Math.Ceiling(this.Model.Span.TotalMinutes);
                if (newSpan != this.Span)
                {
                    this.Span = newSpan;
                }
            }
        }

        // Helpers
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
