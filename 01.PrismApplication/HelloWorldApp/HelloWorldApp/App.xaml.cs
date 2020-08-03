using HelloWorldApp.Views;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace HelloWorldApp
{
    public partial class App : PrismApplication
    {
        // IContainerProvider が Container で取得できるので、そこから Shell を作成する
        protected override Window CreateShell() => Container.Resolve<Shell>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
