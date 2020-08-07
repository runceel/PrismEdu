using MvvmBasicApp.HelloWorld.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MvvmBasicApp.HelloWorld
{
    public class HelloWorldModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var rm = containerProvider.Resolve<IRegionManager>();
            rm.RequestNavigate("Left", nameof(BindableBaseSampleView));
            rm.RequestNavigate("Center", nameof(DelegateCommandSampleView));
            rm.RequestNavigate("Right", nameof(ErrorsContainerSampleView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BindableBaseSampleView>();
            containerRegistry.RegisterForNavigation<DelegateCommandSampleView>();
            containerRegistry.RegisterForNavigation<ErrorsContainerSampleView>();
        }
    }
}
