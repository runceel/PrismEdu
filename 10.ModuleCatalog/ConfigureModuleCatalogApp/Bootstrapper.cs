using Microsoft.Practices.Unity;
using Prism.Unity;
using ConfigureModuleCatalogApp.Views;
using System.Windows;
using Prism.Modularity;

namespace ConfigureModuleCatalogApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
