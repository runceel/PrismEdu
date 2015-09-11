using Microsoft.Practices.Unity;
using Prism.Unity;
using System.Windows;
using ViewModelLocatorSampleApp.Views;

namespace ViewModelLocatorSampleApp
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
    }
}
