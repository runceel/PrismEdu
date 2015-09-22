using Microsoft.Practices.Unity;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace DisposeBehaviorSampleApp.ModuleA.Views
{
    /// <summary>
    /// ContentView.xaml の相互作用ロジック
    /// </summary>
    public partial class ContentView : UserControl, INavigationAware, IDisposable
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public ContentView()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            Debug.WriteLine("ContentView#Dispose");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.RegionManager.Regions["MainRegion"].Remove(this);
        }
    }
}
