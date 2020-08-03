using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using ViewModelLocatorSampleApp.Views;

namespace ViewModelLocatorSampleApp
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell() => Container.Resolve<Shell>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
