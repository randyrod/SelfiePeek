using SelfiePeek.Common;
using SelfiePeek.Utilities;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SelfiePeek
{
    /// <summary>
    /// View used to get authorization from the user on the Instagram Account
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private bool fromMain = false;
        public LoginPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                //Handle if the call has been made from the main page, if so make it so we can go back to main
                var param = e.Parameter as string;
                if (param == "Main")
                {
                    fromMain = true;
                }
            }
            this.navigationHelper.OnNavigatedTo(e);
            loginWeb.Navigate(new Uri(SharedStrings.AuthenticationURI));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (!fromMain)
            {
                Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);   
            }
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void loginWeb_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri.AbsoluteUri.Contains("#"))
            {
                if (args.Uri.Fragment.StartsWith("#access_token="))
                {
                    loginWeb.Stop();
                    string token = args.Uri.Fragment.Replace("#access_token=", string.Empty);
                    ApplicationData.Current.LocalSettings.Values[SharedStrings.AccessTokenKeyValue] = token;
                    SharedStrings.CurrentToken = token;
                    if (!fromMain)
                    {
                        if (!Frame.Navigate(typeof(MainPage)))
                        {
                        }   
                    }
                    else
                    {
                        if (Frame.CanGoBack)
                        {
                            Frame.GoBack();
                        }
                    }
                }
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            loginWeb.Navigate(new Uri(@"https://instagram.com/oauth/authorize/?client_id=c79a7451cfc64faf8b081dbea8ac365a&redirect_uri=http://instagram.com&response_type=token"));
        }
    }
}
