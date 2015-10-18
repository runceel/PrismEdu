using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ValidatableBindableBaseApp.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        public InputDataViewModel InputData { get; } = new InputDataViewModel();

        public async void Alert()
        {
            if (this.InputData.GetAllErrors().Any())
            {
                var dlg = new MessageDialog("HasError");
                await dlg.ShowAsync();
            }
            else
            {
                var dlg = new MessageDialog("NoError");
                await dlg.ShowAsync();
            }
        }
    }
}
