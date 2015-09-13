using Microsoft.Practices.Unity;
using ModuleB.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleB
{
    public class BModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            Debug.WriteLine("BModule initialized.");
            this.Container.RegisterType<object, BView>(nameof(BView));
            this.RegionManager.RequestNavigate("MainRegion", nameof(BView));
        }
    }
}
