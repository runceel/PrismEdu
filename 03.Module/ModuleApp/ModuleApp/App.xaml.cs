using ModuleApp.HelloWorld;
using ModuleApp.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace ModuleApp
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell() => Container.Resolve<Shell>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<HelloWorldModule>();
        }
    }
}
