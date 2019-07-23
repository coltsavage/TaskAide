using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace zCompany.Windows.Charts
{
    internal sealed partial class Interval : UserControl, Interval.Resizer.ViewWidthProvider
    {
        // Class Fields
        private static int counter = 0;

        // Fields
        private IChartState chartState;
        private Resizer resizer;
        private bool pointerExited;

        // Constructors
        public Interval(IntervalViewModel vm, IChartState chartState)
        {
            this.InitializeComponent();

            this.InstanceNumber = counter++;
            this.ViewModel = vm;
            this.chartState = chartState;

            this.resizer = new Resizer(this, chartState);
        }

        // Destructors
        ~Interval()
        {

        }

        // Events
        public event EventHandler<IntervalResizedArgs> IntervalResized;

        // Properties
        public int InstanceNumber { get; private set; }

        private bool isSelected;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                if (value)
                    { this.Select(); }
                else
                    { this.Deselect(); }
            }
        }

        public IntervalViewModel ViewModel { get; private set; }

        // Event Handlers
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new IntervalAutomationPeer(this);
        }

        private void OnPointerCaptureLost(object sender, PointerRoutedEventArgs args)
        {
            // may fire without PointerReleased?
            this.OnPointerReleased(sender, args);
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            this.pointerExited = false;
        }

        private void OnPointerExit(object sender, PointerRoutedEventArgs args)
        {
            this.pointerExited = true;
            if (!this.resizer.IsResizing)
            {
                this.resizer.ShowOriginalCursor();
            }
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            if (this.isSelected)
            {
                if (this.resizer.IsResizing)
                {
                    args.Handled = true;
                    int delta_tick = this.resizer.CalculatePointerDelta_abs(args.GetCurrentPoint(null));

                    this.IntervalResized?.Invoke(this, new IntervalResizedArgs(
                        this.ViewModel.OriginalModel,
                        (this.resizer.ResizedStart) ? delta_tick : 0,
                        (this.resizer.ResizedEnd) ? delta_tick : 0
                        ));
                }
                else
                {
                    if (this.resizer.PointerIsOnEdge_rel(args.GetCurrentPoint(this)))
                    {
                        this.resizer.ShowResizeCursor();
                    }
                    else
                    {
                        this.resizer.ShowOriginalCursor();
                    }
                }
            }
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            if (this.resizer.IsShowingResizeCursor)
            {
                args.Handled = true;
                this.CapturePointer(args.Pointer);
                this.resizer.InitiateResizing(args.GetCurrentPoint(null), args.GetCurrentPoint(this));
            }
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            if (this.resizer.IsResizing)
            {
                this.resizer.CeaseResizing();
                this.ReleasePointerCapture(args.Pointer);

                if (this.pointerExited)
                {
                    this.resizer.ShowOriginalCursor();
                }
            }
        }

        // Helpers
        private void Deselect()
        {
            this.Visual.StrokeThickness = 0;

            this.resizer.ShowOriginalCursor();
        }

        private void Select()
        {
            this.Visual.StrokeThickness = 4;
        }

        // Classes
        private class Resizer
        {
            // Parent-Implementing Interface
            public interface ViewWidthProvider
            {
                double Width { get; }
            }

            // Fields
            private IChartState chartState;
            private Edge edge;
            private CoreCursor originalCursor;
            private Point priorPosition_abs;
            private CoreCursor resizeCursor;
            private ViewWidthProvider widthProvider;

            // Constructors
            public Resizer(ViewWidthProvider widthProvider, IChartState chartState)
            {
                this.widthProvider = widthProvider;
                this.chartState = chartState;

                this.resizeCursor = new CoreCursor(CoreCursorType.SizeWestEast, 0);
            }

            // Enums
            private enum Edge { NA, Start, End };

            // Properties
            public bool IsResizing { get; private set; }

            public bool IsShowingResizeCursor { get; private set; }

            public bool ResizedStart
            {
                get => this.edge == Edge.Start;
            }

            public bool ResizedEnd
            {
                get => this.edge == Edge.End;
            }

            // Methods
            public int CalculatePointerDelta_abs(PointerPoint pointer_abs)
            {
                double delta_pixel = pointer_abs.Position.X - this.priorPosition_abs.X;
                this.priorPosition_abs = pointer_abs.Position;

                return Convert.ToInt32(Math.Round(delta_pixel / this.chartState.PixelsPerTick, MidpointRounding.AwayFromZero));
            }

            public void CeaseResizing()
            {
                this.IsResizing = false;
                this.edge = Edge.NA;
            }

            public void InitiateResizing(PointerPoint pointer_abs, PointerPoint pointer_rel)
            {
                this.IsResizing = true;
                this.edge = CursorLocation(pointer_rel.Position);
                this.priorPosition_abs = pointer_abs.Position;
            }

            public bool PointerIsOnEdge_rel(PointerPoint pointer_rel)
            {
                return this.CursorLocation(pointer_rel.Position) != Edge.NA;
            }

            public void ShowOriginalCursor()
            {
                if (this.IsShowingResizeCursor)
                {
                    Window.Current.CoreWindow.PointerCursor = this.originalCursor;
                    this.IsShowingResizeCursor = false;
                }
            }

            public void ShowResizeCursor()
            {
                if (!this.IsShowingResizeCursor)
                {
                    this.originalCursor = Window.Current.CoreWindow.PointerCursor;
                    Window.Current.CoreWindow.PointerCursor = this.resizeCursor;
                    this.IsShowingResizeCursor = true;
                }
            }

            // Helpers
            private Edge CursorLocation(Point point_rel)
            {
                Edge location = Edge.NA;
                if (point_rel.X < 8)
                {
                    location = Edge.Start;
                }
                else if (point_rel.X > this.widthProvider.Width - 8)
                {
                    location = Edge.End;
                }
                return location;
            }
        }
    }
}
