using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Common;
using Prism.Regions;
using System.Diagnostics;
using System.Linq;

namespace NavigationSampleApp.ViewModels
{
    class BViewModel : IRegionMemberLifetime, INavigationAware
    {
        public DelegateCommand CloseCommand { get; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public bool KeepAlive { get; set; } = true;

        public BViewModel()
        {
            this.CloseCommand = new DelegateCommand(() =>
            {
                this.KeepAlive = false;
                // find view by region
                var view = this.RegionManager.Regions["MainRegion"]
                    .ActiveViews
                    .Where(x => MvvmHelpers.GetImplementerFromViewOrViewModel<BViewModel>(x) == this)
                    .First();
                // deactive view
                this.RegionManager.Regions["MainRegion"].Deactivate(view);
            });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Debug.WriteLine("BViewModel NavigatedTo");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Debug.WriteLine("BViewModel NavigatedFrom");
        }
    }
}
