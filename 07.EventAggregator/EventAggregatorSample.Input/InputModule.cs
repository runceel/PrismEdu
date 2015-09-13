using EventAggregatorSample.Infrastructure;
using EventAggregatorSample.Input.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAggregatorSample.Input
{
    public class InputModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            // register models(singleton)
            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(InputModule).Assembly)
                    .Where(x => x.Namespace.EndsWith(".Models")),
                getFromTypes: WithMappings.FromAllInterfaces,
                getLifetimeManager: WithLifetime.ContainerControlled);

            // register views
            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(InputModule).Assembly)
                    .Where(x => x.Namespace.EndsWith(".Views")),
                    getFromTypes: _ => new[] { typeof(object) });

            // register views @ region
            this.RegionManager.RegisterViewWithRegion(RegionNames.InputRegion, typeof(InputView));
        }
    }
}
