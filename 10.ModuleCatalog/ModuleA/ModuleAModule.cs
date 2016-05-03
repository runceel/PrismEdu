using Microsoft.Practices.Unity;
using ModuleA.Models;
using Prism.Modularity;

namespace ModuleA
{
    public class ModuleAModule : IModule
    {
        private IUnityContainer Container { get; }

        public ModuleAModule(IUnityContainer container)
        {
            this.Container = container;
        }

        public void Initialize()
        {
            this.Container.RegisterType<Calc>(new ContainerControlledLifetimeManager());
        }
    }
}