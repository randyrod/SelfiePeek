using SelfiePeek.Utilities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SelfiePeek
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.DataContext = App.PeekVM;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.PeekVM.Loaded)
            {
                App.PeekVM.InitAsync();   
            }
        }

        private void SortBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selfiesGridView.ReorderMode == ListViewReorderMode.Disabled)
            {
                selfiesGridView.ReorderMode = ListViewReorderMode.Enabled;   
            }
            else
            {
                selfiesGridView.ReorderMode = ListViewReorderMode.Disabled;   
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SharedStrings.CurrentToken != null)
            {
                App.PeekVM.InitAsync();
            }
            else
            {
                if (!Frame.Navigate(typeof(LoginPage), "Main"))
                {
                    
                }
            }
        }

        private void GridViewItem_Click(object sender, ItemClickEventArgs e)
        {
            if (!Frame.Navigate(typeof(ShowImage), e.ClickedItem))
            {
                
            }
        }
    }
}
