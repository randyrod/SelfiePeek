using SelfiePeek.Common;
using SelfiePeek.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SelfiePeek
{
    public sealed partial class ShowImage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private SelfiePeekDataModel dataContext;
        public ShowImage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);

        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationHelper.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                dataContext = e.Parameter as SelfiePeekDataModel;
                this.DataContext = dataContext;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }
    }
}
