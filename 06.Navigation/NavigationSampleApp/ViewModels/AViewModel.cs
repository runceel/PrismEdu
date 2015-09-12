using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;

namespace NavigationSampleApp.ViewModels
{
    class AViewModel : BindableBase, INavigationAware
    {
        private string id;

        public string Id
        {
            get { return this.id; }
            set { this.SetProperty(ref this.id, value); }
        }

        public bool KeepAlive { get; set; } = true;

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return this.Id == navigationContext.Parameters["id"] as string;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Debug.WriteLine("NavigatedFrom");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Debug.WriteLine("NavigatedTo");
            this.Id = navigationContext.Parameters["id"] as string;
        }
    }
}
