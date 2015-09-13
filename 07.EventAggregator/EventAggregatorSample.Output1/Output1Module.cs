using EventAggregatorSample.Infrastructure;
using EventAggregatorSample.Output1.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAggregatorSample.Output1
{
    public class Output1Module : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            // register models
            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(Output1Module).Assembly)
                    .Where(x => x.Namespace.EndsWith(".Models")),
                getFromTypes: WithMappings.FromAllInterfaces,
                getLifetimeManager: WithLifetime.ContainerControlled);

            // register views
            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(Output1Module).Assembly)
                    .Where(x => x.Namespace.EndsWith(".Views")),
                getFromTypes: _ => new[] { typeof(object) },
                getName: WithName.TypeName);

            // register views @ region
            this.RegionManager.RegisterViewWithRegion(RegionNames.OututRegion, typeof(Output1View));
        }
    }
}
