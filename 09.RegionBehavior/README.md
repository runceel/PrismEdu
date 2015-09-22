# RegionBehavior

PrismのRegionは、WPFのBehaviorみたいな機能を提供するRegionBehaviorというものを提供しています。
あまり自分で作ることはないと思いますが、Regionから削除されたときにIDisposableを実装したVやVMに対してはDisposeを呼ぶというのは需要ありそうだと思ったので書いてみます。

## RegionBehavior

RegionBehaviorは単純で、RegionプロパティとAttachメソッドを持っただけのシンプルなインターフェースを実装するだけです。
Attachで、Regionのイベントを購読してごにょごにょするだけです。Disposeを呼ぶBehaviorはこんな感じになります。

```cs
using Prism.Common;
using Prism.Regions;
using System;
using System.Collections.Specialized;

namespace DisposeBehaviorSampleApp.RegionBehaviors
{
    class DisposeBehavior : IRegionBehavior
    {
        public const string Key = nameof(DisposeBehavior);

        public IRegion Region { get; set; }

        public void Attach()
        {
            this.Region.Views.CollectionChanged += this.Views_CollectionChanged;
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Action<IDisposable> callDispose = d => d.Dispose();
                foreach (var o in e.OldItems)
                {
                    MvvmHelpers.ViewAndViewModelAction(o, callDispose);
                }
            }
        }
    }
}
```

RegionのすべてのViewを格納しているViewsコレクションの変更を購読しています。
ViewsはViewsCollectionという型で、CollectionChangedはAddかRemoveしか飛ばさないのでどちらかを処理すればいいです。
今回は削除自にDisposeを呼ぶという処理なのでRemoveを処理すればいいことになります。

PrismにはMvvmHelpersというクラスがあって、VとVMに対して、メソッドが呼べたら呼ぶというヘルパーがあります。
こいつをつかって削除されたViewに対してDisposeを呼び出しています。

作ったRegionBehaviorは、BootstrapperのConfigureDefaultRegionBehaviorsメソッドで登録します。以下のような感じです。

```cs
using DisposeBehaviorSampleApp.RegionBehaviors;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System.Windows;

namespace DisposeBehaviorSampleApp
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

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            var c = (ModuleCatalog)this.ModuleCatalog;
            c.AddModule(typeof(ModuleA.Module));
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var f = base.ConfigureDefaultRegionBehaviors();
            f.AddIfMissing(DisposeBehavior.Key, typeof(DisposeBehavior));
            return f;
        }
    }
}
```

こうすることで、Regionに対してRegionBehaviorが自動的に追加されるようになります。デフォルトでもいくつものRegionBehaviorが登録されてるので興味のある方はPrismのソースを見てみるといいと思います。
以下のようにIDisposableを実装して、いるView（やViewModel）でRegionからRemoveすると自動的にDisposeが呼ばれることが確認できます。サンプルではデバッグウィンドウにメッセージを出しています。

```cs
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
```


