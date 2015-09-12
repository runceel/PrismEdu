using Microsoft.Practices.Unity;
using ModuleApp.Views;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace ModuleApp
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

            var catalog = (ModuleCatalog)this.ModuleCatalog;
            catalog.AddModule(typeof(HelloWorldModule.HelloWorldModule));
        }

    }
}
