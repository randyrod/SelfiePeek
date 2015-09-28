using SelfiePeek.Utilities;
using SelfiePeek.ViewModels;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace SelfiePeek
{
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        private static PeekViewModel _PeekVM;
        public static PeekViewModel PeekVM
        {
            get { return _PeekVM; }
        }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            _PeekVM = new PeekViewModel();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Background = new SolidColorBrush(Colors.WhiteSmoke);
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(SharedStrings.AccessTokenKeyValue))
                {
                    ApplicationData.Current.LocalSettings.Values.Remove(SharedStrings.AccessTokenKeyValue);
                }
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(SharedStrings.AccessTokenKeyValue))
                {
                    SharedStrings.CurrentToken = ApplicationData.Current.LocalSettings.Values[SharedStrings.AccessTokenKeyValue].ToString();
                    if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else
                {
                    if (!rootFrame.Navigate(typeof(LoginPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
            }
            

            Window.Current.Activate();
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }
    }
}