using Prism.Unity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Modularity;
using EventAggregatorSample.Input;
using EventAggregatorSample.Output1;
using EventAggregatorSample.Output2;

namespace EventAggregatorSampleApp
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
            c.AddModule(typeof(InputModule));
            c.AddModule(typeof(Output1Module));
            c.AddModule(typeof(Output2Module));
        }
    }
}
