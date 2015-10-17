using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceApp.ViewModels
{
    class NextPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; }

        public NextPageViewModel(INavigationService ns)
        {
            this.NavigationService = ns;
        }

        public void GoBack()
        {
            this.NavigationService.GoBack();
        }
    }
}
