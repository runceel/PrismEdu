# ModuleとRegion

Prismには、Moduleと呼ばれる機能があります。これはアプリケーションを複数の機能に分割して開発するための仕組みです。Prismでは最終的にModuleに分割したアプリケーションをまとめ上げる機能があります。

## Moduleの使い方

ここでは簡単なModuleの使い方を示します。まず、Moduleを作るにはクラスライブラリプロジェクトを作成します。WPFのクラス群が最初から使えるようにカスタムコントロールかユーザコントロール用のライブラリを作成するのが手間が少なくていいです。クラスライブラリを作成したら、初期状態で作成されているクラスを削除してPrism.CoreとPrism.UnityをNuGetから追加します。
そして、Prism.Modularity.IModuleインターフェースを実装したクラスを作成します。このクラスがPrismのモジュールのエントリポイントになります。

作成したModuleは、Bootstrapperクラスのあるプロジェクトにプロジェクト参照を追加して、ConfigureModuleCatalogメソッドをオーバーライドして以下のようなコードでPrismにModuleがあることを伝えます。

```cs
protected override void ConfigureModuleCatalog()
{
    base.ConfigureModuleCatalog();

    var catalog = (ModuleCatalog)this.ModuleCatalog;
    catalog.AddModule(typeof(HelloWorldModule.HelloWorldModule));
}
```

ここでは、HelloWorldModule.HelloWorldModuleクラスがIModuleを実装したクラスになります。

### IModuleインターフェース

IModuleインターフェースは、Initializeメソッドを持つだけのシンプルなインターフェースです。ここでModuleの初期化処理を行います。

## Regionについて

Moduleを作成した具体的なコードの前にRegionについて触れておきたいと思います。PrismはModuleを組み立てることで柔軟にアプリケーションを構築できます。
Module間は疎結合に作られるのが理想的で、Module内のクラスは、何かしらのメッセージング機構を使って連携するのが理想的です。（Prismには、メッセージング機構も用意されています）

柔軟にModuleを組み合わせてアプリケーションを作るための画面側の仕組みとしてRegionというものがあります。れは、Shellをいくつかの区画（Region）にわけて、そこに対して画面部品を流し込むことで画面側も複数Moduleで構成されたときにも柔軟に対応できるようにしています。

Regionを使うには、画面の区画として扱いたいところにRegionNameを付けます。

```xml
xmlns:prism="http://prismlibrary.com/"

<ContentControl prism:RegionManager.RegionName="MainRegion" />
```

RegionNameをつけれるコントロールは、ContentControlのほかにItemsControl(を継承したコントロール)があります。
ContentControlは、Region内でアクティブになれるViewが1度に1つなのに対して、ItemsControlは複数のViewに対応している点が異なります。

このようにRegionを作成したら、Prismの提供するIRegionManagerのRequestNavigateを呼び出すことでViewを表示できます。


```cs
this.RegionManager.RequestNavigate("MainRegion", nameof(HelloWorldView));
```

## ModuleとRegionを使ったプログラム例

ということで、ModuleとRegionを使った簡単なプログラムを組んでいきたいと思います。ModuleAppという名前でWPFアプリケーションを作成して、Prism.CoreとPrism.UnityをNuGetから追加します。
そして、Shell.xamlを作成してBootstrapperクラスを作成して、表示するところまで作成します。

### Moduleの作成

ModuleApp.HelloWorldModuleという名前でWPF カスタムコントロール ライブラリを作成して、カスタムコントロールのコードを削除します。そこにPrism.CoreとPrism.UnityをNuGetから追加します。
ここに各種クラスを作っていきます。

#### Models, ViewModels, Views名前空間

こんにちは世界と表示するViewを作成します。まず、Models名前空間に以下のようなメッセージを提供するクラスを作成します。

```cs
namespace ModuleApp.HelloWorldModule.Models
{
    class MessageProvider
    {
        public string Message { get; } = "こんにちは世界";
    }
}
```

そして、HelloWorldViewModelクラスを作成します。先ほどのMessageProviderをインジェクションするようにしています。

```cs
using Microsoft.Practices.Unity;
using ModuleApp.HelloWorldModule.Models;

namespace ModuleApp.HelloWorldModule.ViewModels
{
    class HelloWorldViewModel
    {
        [Dependency]
        public MessageProvider MessageProvider { get; set; }
    }
}
```

最後に、HelloWorldViewを作成します。Views生空間にユーザーコントロールを作成して、XAMLを以下のように編集します。

```xml
<UserControl x:Class="ModuleApp.HelloWorldModule.Views.HelloWorldView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             xmlns:local="clr-namespace:ModuleApp.HelloWorldModule.Views"
             xmlns:prism="http://prismlibrary.com/"
             prism:ViewModelLocator.AutoWireViewModel="True"
             mc:Ignorable="d" 
             d:DesignHeight="300" d:DesignWidth="300">
    <Grid>
        <TextBlock Text="{Binding MessageProvider.Message}" />
    </Grid>
</UserControl>
```

prism名前空間の追加とViewModelLocatorの追加をして、先ほど作成したHelloWorldViewModelがDataContextに設定されるようにしています。
そして、TextBlockにMessageProviderのMessageを表示するようにしています。

#### IModuleインターフェースの実装

IModuleインターフェースを実装したクラスを作成します。HelloWorldModuleという名前のクラスをプロジェクト直下に作成します。
HelloWorldModuleクラスのInitializeメソッドでModuleで使用するクラスをIUnityContainerに登録したり、画面をRegionに表示したりといった処理を行います。

```cs
using Microsoft.Practices.Unity;
using ModuleApp.HelloWorldModule.Models;
using ModuleApp.HelloWorldModule.Views;
using Prism.Modularity;
using Prism.Regions;

namespace ModuleApp.HelloWorldModule
{
    public class HelloWorldModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            this.Container.RegisterType<MessageProvider>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<object, HelloWorldView>(nameof(HelloWorldView));

            this.RegionManager.RequestNavigate("MainRegion", nameof(HelloWorldView));
        }
    }
}
```

IUnityContainerとIRegionManagerを、PrismからインジェクションしてもらうためにDependency属性をつけたプロパティとして定義しています。
そして、InitializeメソッドでRegisterTypeをして必要なクラスを登録しています。

注意点としては、Viewの登録は必ずobject型として登録する必要がある点です。また名前は型名で登録するのが一般的です。

最後にRegionManagerのRequestNavigateメソッドで、Shellに定義されたMainRegionにHelloWorldView（上でUnityに登録しているやつ）を流し込んでいます。

ここまでで、プロジェクトは大体こんな形になっています。

- ModuleApp
	- Views
		- Shell.xaml
	- Bootstrapper.cs
- ModuleApp.HelloWorldModule
	- Models
		- MessageProvider.cs
	- ViewModels
		- HelloWorldViewModel.cs
	- Views
		- HelloWorldView.xaml
	- HelloWorldModule.cs

#### ModuleをBootstrapperで組み込む

最後に、Moduleを組み込むコードをBootstrapperに追加します。Moduleの構成は、BootstrapperのConfigureModuleCatalogメソッドで行います。
ここでModuleCatalog(IModuleCatalog型)をModuleCatalog型にキャストして、AddModuleで先ほど作成したHelloWorldModuleを追加します。

```cs
using Microsoft.Practices.Unity;
using ModuleApp.Views;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace ModuleApp
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

            var catalog = (ModuleCatalog)this.ModuleCatalog;
            catalog.AddModule(typeof(HelloWorldModule.HelloWorldModule));
        }

    }
}
```

実行すると、ModuleがShellにHelloWorldModuleで定義されたHelloWorldViewが表示されて、画面にこんにちは世界と表示されることが確認できます。

