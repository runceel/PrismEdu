using Microsoft.Practices.Unity;
using ModuleApp.HelloWorldModule.Models;
using ModuleApp.HelloWorldModule.Views;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleApp.HelloWorldModule
{
    public class HelloWorldModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            this.Container.RegisterType<MessageProvider>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<object, HelloWorldView>(nameof(HelloWorldView));

            this.RegionManager.RequestNavigate("MainRegion", nameof(HelloWorldView));
        }
    }
}
