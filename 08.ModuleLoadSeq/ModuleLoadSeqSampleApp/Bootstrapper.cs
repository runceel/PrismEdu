using Microsoft.Practices.Unity;
using ModuleA;
using ModuleB;
using ModuleCommon;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace ModuleLoadSeqSampleApp
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
            // ondemand
            c.AddModule(typeof(BModule), InitializationMode.OnDemand);
            // depend CommonModule
            c.AddModule(typeof(AModule), nameof(CommonModule));
            c.AddModule(typeof(CommonModule));
        }
    }
}
