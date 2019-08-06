using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using zCompany.Utilities;

namespace zCompany.TaskAide.WindowsApp
{
    sealed partial class App : Application
    {
        // Class Properties
        internal static AppEvents Events { get; private set; }
        internal static AppSettings Settings { get; private set; }
        internal static ITaskAide State { get; private set; }

        // Constructors
        public App()
        {
            this.InitializeComponent();

            this.EnteredBackground += OnEnteredBackground;
            this.LeavingBackground += OnLeavingBackground;
            this.Resuming += OnResuming;
            this.Suspending += OnSuspending;
        }

        // Event Handlers - Activations
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        // Occurrence
        //  - when user launches app (ie. tile selection)
        //  - during prelaunch, when system launches app
        // Assumptions
        //  - prelaunch is disabled by default (post v1607)
        //      - when enabled, may be prelaunch or may have already prelaunched
        //  - app may not immediately be visible (eg. prelaunch)
        //  - previous state unknown (eg. user terminated, system terminated, crashed/hasn't-run, suspended, running)
        //  - app will feel unresponsive if execution exceeds a few seconds
        // Actions
        //  - initial app resources and state
        //  - register event handlers
        //  - configure and set initial page (on user launch)
        //  - display splash screen (on user launch)
        //      - (utilize custom SplashScreen if initialization requires more time) 
        {
            if (App.State == null)
            {
                var systemTime = new SystemTimeDev(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));

                var taskAide = new TaskAide(new Database("Filename=TaskAide.db"), systemTime, new TimerDev(systemTime));
                systemTime.Rewound += taskAide.OnSystemRewound;

                var appEvents = new AppEvents();
                appEvents.TaskAdded += (s, args) => taskAide.AddTask(args.Name);
                appEvents.TaskNameChanged += (s, args) => taskAide.RenameTask(args.Task, args.NewName);
                appEvents.TaskRemoved += (s, args) => taskAide.RemoveTask(args.Task);
                appEvents.UserChangedInterval += (s, args) => taskAide.UserChangedInterval(args.Interval, args.StartDelta, args.SpanDelta);
                appEvents.UserSwitchedTasks += (s, args) => taskAide.SwitchTasks(args.Task);

                if ((e.PreviousExecutionState == ApplicationExecutionState.ClosedByUser) ||
                    (e.PreviousExecutionState == ApplicationExecutionState.Terminated))
                {
                    // restore previous user session state
                }

                App.Events = appEvents;
                App.Settings = new AppSettings();
                App.State = taskAide;
            }

            if (!e.PrelaunchActivated)
            {
                if (Window.Current.Content == null)
                {
                    Window.Current.Content = new ActiveSession();
                }
                Window.Current.Activate();
            }
        }

        // Event Handlers - Changing State
        private void OnEnteredBackground(object sender, EnteredBackgroundEventArgs args)
        // State change: Running in foreground > Running in background
        // Occurrence
        //  - when app loses visibility
        //  - when user terminated
        // Assumptions
        //  - UI losing visibility
        //  - may avoid suspension
        //      - if below memory threshold
        //      - if performing ongoing background task (ie. audio playback)
        //  - system may terminate to free resources without first moving to Suspended state
        //  - execution time is limited (may request extension (may not be granted))
        // Actions
        //  - cease UI behavior
        //  - perform some/all OnSuspending() actions (should be async due to time constraints?)
        //  - begin/continue ongoing background tasks (ie. audio playback)
        //  - free UI memory resources until below AppMemoryUsageLimit threshold
        {
            var deferral = args.GetDeferral();

            // async calls

            deferral.Complete();
        }

        private void OnLeavingBackground(object sender, LeavingBackgroundEventArgs args)
        // State change: Running in background > Running in foreground
        // Occurrence
        //  - when app becomes visible
        // Assumptions
        //  - UI not yet visible
        //  - app will feel unresponsive if execution isn't immediate
        // Actions
        //  - update and commence UI behavior
        //  - kick-off async calls to long-running tasks
        {

        }

        private void OnResuming(object sender, object args)
        // State change: Suspended > Running in background
        // Occurrence
        //  - when app becomes visible following extended non-visibility
        //  - when activated via app contract/extension
        // Assumptions
        //  - state was preserved
        //  - when app is activated, occurs prior Activated event
        //  - does not execute on UI thread
        //  - app will feel unresponsive if execution exceeds a few seconds
        // Actions
        //  - restore state released when suspending
        {

        }

        private void OnSuspending(object sender, SuspendingEventArgs args)
        // State change: Running in background > Suspended
        // Occurrence
        //  - when app is not visible (beyond a few seconds) and no ongoing background tasks
        //  - during prelaunch, following system's OnLaunched()
        //  - when user terminated
        // Assumptions
        //  - may be terminated to free system resources
        //  - not all resources may yet be allocated (eg. prelaunch)
        //  - execution time is limited (may request extension (may not be granted))
        //  - not called if system terminated while in "Running in background" state
        // Actions
        //  - preserve user session state
        //  - release handles/locks on resources
        //  - reduce memory usage
        //  - save app state
        {
            var deferral = args.SuspendingOperation.GetDeferral();

            // async calls

            deferral.Complete();
        }
    }
}
