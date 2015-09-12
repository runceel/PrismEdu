using Microsoft.Practices.Unity;
using MVVMBasicApp.Module.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMBasicApp.Module
{
    public class MVVMBasicModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            // Viewを登録
            this.Container.RegisterTypes(
                types: AllClasses.FromLoadedAssemblies().Where(x => x.Namespace == "MVVMBasicApp.Module.Views"),
                getFromTypes: _ => new[] { typeof(object) },
                getName: WithName.TypeName);

            this.RegionManager.RegisterViewWithRegion("MainRegion", typeof(BindableBaseSampleView));
            this.RegionManager.RegisterViewWithRegion("MainRegion", typeof(DelegateCommandSampleView));
            this.RegionManager.RegisterViewWithRegion("MainRegion", typeof(ErrorsContainerSampleView));
        }
    }
}
