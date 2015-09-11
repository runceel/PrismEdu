using Prism.Unity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ModuleApp.Views;
using Prism.Modularity;

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

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = (ModuleCatalog) base.CreateModuleCatalog();
            catalog.AddModule(typeof(HelloWorldModule.HelloWorldModule));
            return catalog;
        }

    }
}
