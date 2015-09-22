using DisposeBehaviorSampleApp.RegionBehaviors;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System.Windows;

namespace DisposeBehaviorSampleApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            ((Window)this.Shell).Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            var c = (ModuleCatalog)this.ModuleCatalog;
            c.AddModule(typeof(ModuleA.Module));
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var f = base.ConfigureDefaultRegionBehaviors();
            f.AddIfMissing(DisposeBehavior.Key, typeof(DisposeBehavior));
            return f;
        }
    }
}
