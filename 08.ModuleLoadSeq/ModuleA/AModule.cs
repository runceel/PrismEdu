using Microsoft.Practices.Unity;
using ModuleA.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleA
{
    public class AModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            Debug.WriteLine("AModule initialized.");
            this.Container.RegisterType<object, AView>(nameof(AView));
            this.RegionManager.RegisterViewWithRegion("MainRegion", typeof(AView));
        }
    }
}
