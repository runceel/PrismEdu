using ModuleC.Views;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ModuleC
{
    public class ModuleCModule : IModule
    {
        private IRegionManager RegionManager { get; }

        public ModuleCModule(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;
        }

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion("MainRegion", typeof(ModuleCView));
        }
    }
}