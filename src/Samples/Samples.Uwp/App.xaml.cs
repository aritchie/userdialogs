using Acr.UserDialogs;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.LockScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;
using Application = Windows.UI.Xaml.Application;
using Frame = Windows.UI.Xaml.Controls.Frame;


namespace Samples.Uwp
{
    sealed partial class App : Application
    {
        /// <summary>
        /// Boolean used to store the assigned access mode value
        /// </summary>
        public static bool IsAssignedAccess { get; private set; }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            try
            {
                IsAssignedAccess = LockApplicationHost.GetForCurrentView() != null;
            }
            catch
            {
                IsAssignedAccess = false;
            }

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += this.OnNavigationFailed;
                Forms.Init(e);

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

            UserDialogs.Init(IsAssignedAccess);

            Window.Current.Activate();
        }


        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }


        void OnSuspending(object sender, SuspendingEventArgs e) => e.SuspendingOperation.GetDeferral().Complete();
    }
}
