using DisposeBehaviorSampleApp.ModuleA.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposeBehaviorSampleApp.ModuleA
{
    public class Module : IModule
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            this.Container
                .RegisterTypes(
                    AllClasses.FromAssemblies(typeof(Module).Assembly)
                        .Where(x => x.Namespace.EndsWith(".Views")),
                    getFromTypes: _ => new[] { typeof(object) },
                    getName: WithName.TypeName);

            this.RegionManager.RequestNavigate("CommandRegion", nameof(AddItemsCommand));
        }
    }
}
