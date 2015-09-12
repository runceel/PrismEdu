using InteractionRequestSampleModule.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionRequestSampleModule
{
    public class SampleModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            this.Container.RegisterType<object, InteractionRequestView>(nameof(InteractionRequestView));

            this.RegionManager.RequestNavigate("MainRegion", nameof(InteractionRequestView));
        }
    }
}
