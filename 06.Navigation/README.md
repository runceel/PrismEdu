# ナビゲーション時の挙動

PrismのRegionを使ったナビゲーションについて詳細に説明します。
Prismのナビゲーションは、デフォルトでは一度追加された画面を再利用するように動作します。
これは、画面Aから画面Bに遷移して、画面Aに戻ってきたときに、前の画面Aがそのまま使われることを意味します。

そのため、画面遷移時にきちんとデータの初期化などを行う必要があります。
画面遷移の処理をフックする方法として、Prismでは、Prism.Regions.INavigationAwareインターフェースを提供しています。
このインターフェースは以下のメソッドを定義しています。

- bool IsNavigationTarget(NavigationContext)
	- 引数で渡されたコンテキストのターゲットとなる画面かどうかを返します。
- void OnNavigatedFrom(NavigationContext)
	- 画面から離れるときに呼び出されます。
- void OnNavigatedTo(NavigationContext)
	- 画面に遷移してきたときに呼び出されます。

つまり、OnNavigatedToで初期化処理をすれば良いことになります。

## 新しいインスタンスを作りたい

場合によっては同じインスタンスを使いまわされたくないケースもあります。
例えば、TabControlをRegionにしているときに、同じViewだけど違うコンテンツを表示したいといったケースが考えられます。
このときには、IsNavigationTargetメソッドが役に立ちます。

このメソッドがtrueを返すと、そのViewが再利用される仕組みになっています。（ViewModelがINavigationAwareを実装してないときは常にtrueとみなされてるので再利用される）
このメソッドで、NavigationContextのパラメータなどを見て適切にtrueやfalseを返すことで画面遷移自に新しいViewを作るか作らないか制御できます。

NavigationSampleAppでは、Navigationのパラメータを使うことでAViewというViewを2つインスタンス化して管理しています。
AViewModelのコードを以下に示します。

```cs
using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;

namespace NavigationSampleApp.ViewModels
{
    class AViewModel : BindableBase, INavigationAware
    {
        private string id;

        public string Id
        {
            get { return this.id; }
            set { this.SetProperty(ref this.id, value); }
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return this.Id == navigationContext.Parameters["id"] as string;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Debug.WriteLine("NavigatedFrom");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Debug.WriteLine("NavigatedTo");
            this.Id = navigationContext.Parameters["id"] as string;
        }
    }
}
```

ナビゲーションする側では、以下のようにパラメータを渡して画面遷移するようにしています。

```cs
this.RegionManager.RequestNavigate("MainRegion", nameof(AView), new NavigationParameters($"id={x}"));
```

このようにすることで、細かなViewの制御が出来ます。

## Viewを破棄する

一度生成されたViewを破棄するには、Prism.Regions.IRegionMemberLifetimeインターフェースを実装します。
このインターフェースに定義されているKeepAliveをfalseにすることで、RegionのアクティブなViewが変わるタイミングで
Viewの破棄が行われます。

多少無理矢理ですが、ボタンが押された（Commandが実行された）ら自分自身をViewModelから殺す方法を紹介します。
KeepAliveをfalseに設定して、自分のいるRegionから自分自身のViewを探して非アクティブにしています。

```cs
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Common;
using Prism.Regions;
using System.Diagnostics;
using System.Linq;

namespace NavigationSampleApp.ViewModels
{
    class BViewModel : IRegionMemberLifetime, INavigationAware
    {
        public DelegateCommand CloseCommand { get; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public bool KeepAlive { get; set; } = true;

        public BViewModel()
        {
            this.CloseCommand = new DelegateCommand(() =>
            {
                this.KeepAlive = false;
                // find view by region
                var view = this.RegionManager.Regions["MainRegion"]
                    .ActiveViews
                    .Where(x => MvvmHelpers.GetImplementerFromViewOrViewModel<BViewModel>(x) == this)
                    .First();
                // deactive view
                this.RegionManager.Regions["MainRegion"].Deactivate(view);
            });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Debug.WriteLine("BViewModel NavigatedTo");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Debug.WriteLine("BViewModel NavigatedFrom");
        }
    }
}
```

ここまで書いてViewが直接とれるならRegionから素直にRemoveしてもいい気がしてきました。今回のケースではお好みの方法でどうぞ。
別画面に遷移する前に、この画面は破棄したいときなどは、このKeepAliveを使う方法がおすすめです。