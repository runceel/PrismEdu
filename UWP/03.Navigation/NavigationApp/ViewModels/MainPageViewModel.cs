using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Diagnostics;

namespace NavigationApp.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; }

        // コンストラクタを定義することでUnityからインスタンスが自動で渡される
        public MainPageViewModel(INavigationService navigationService)
        {
            // Unityから渡されたインスタンスを保持
            this.NavigationService = navigationService;
        }

        // INavigationServiceを使って画面遷移を行う
        public void NavigateNextPage()
        {
            this.NavigationService.Navigate("Next", null);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Debug.WriteLine("MainPageに来ました");
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            Debug.WriteLine("MainPageから離れます");
        }
    }
}
