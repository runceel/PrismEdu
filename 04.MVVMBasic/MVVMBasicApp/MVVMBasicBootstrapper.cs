using Microsoft.Practices.Unity;
using MVVMBasicApp.Module;
using MVVMBasicApp.Views;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace MVVMBasicApp
{
    class MVVMBasicBootstrapper : UnityBootstrapper
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
            catalog.AddModule(typeof(MVVMBasicModule));
        }
    }
}
