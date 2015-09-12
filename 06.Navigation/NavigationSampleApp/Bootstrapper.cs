using NavigationSampleApp.Views;
using Prism.Unity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Modularity;

namespace NavigationSampleApp
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

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.RegisterTypes(
                AllClasses.FromLoadedAssemblies()
                    .Where(x => x.Namespace.EndsWith(".Views")),
                getFromTypes: _ => new[] { typeof(object) },
                getName: WithName.TypeName);
        }
    }
}
