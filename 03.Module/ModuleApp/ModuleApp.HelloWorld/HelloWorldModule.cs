using ModuleApp.HelloWorld.Models;
using ModuleApp.HelloWorld.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleApp.HelloWorld
{
    public class HelloWorldModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate("MainRegion", nameof(HelloWorldView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // MessageProvider はシングルトン
            containerRegistry.RegisterSingleton<MessageProvider>();

            // HelloWorldView を Region に表示するようにコンテナに登録
            containerRegistry.RegisterForNavigation<HelloWorldView>();
        }
    }
}
