using NavigationApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace NavigationApp.Views
{
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();
        }
        
    }
}
