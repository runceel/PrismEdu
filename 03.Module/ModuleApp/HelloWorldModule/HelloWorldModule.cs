using Microsoft.Practices.Unity;
using ModuleApp.HelloWorldModule.Models;
using ModuleApp.HelloWorldModule.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleApp.HelloWorldModule
{
    class HelloWorldModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            this.Container.RegisterInstance(new MessageProvider());
            this.Container.RegisterType<object, HelloWorldView>(nameof(HelloWorldView));

            this.RegionManager.RequestNavigate("MainRegion", nameof(HelloWorldView));
        }
    }
}
