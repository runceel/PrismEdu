using Microsoft.Practices.Unity;
using ModuleB.Views;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ModuleB
{
    public class ModuleBModule : IModule
    {
        private IRegionManager RegionManager { get; }

        public ModuleBModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.RegionManager = regionManager;
        }

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion("MainRegion", typeof(ModuleBView));
            this.RegionManager.RequestNavigate("MainRegion", "ModuleBView");
        }
    }
}