# ViewModelLocator を使おう

Prism では、View と ViewModel を紐づけるための機能として ViewModelLocator というものを提供しています。この機能を有効にするには Window や UserControl などの View に以下の属性を追加するだけです。

```xml
xmlns:prism="http://prismlibrary.com/"
prism:ViewModelLocator.AutoWireViewModel="True"
```

この宣言を View の XAML に追加すると以下のようなルールで対応する ViewModel が探されます。

HogeProject.FooNamespace.Views.SampleWindow という View があった場合、HogeProject.FooNamespace.ViewModels.SampleWindowViewModel という名前のViewModelが自動的に DataContext に割り当てられます。（SampleView のように View で名前が終わる場合は SampleViewModel になります）

PrismApplication の使いかたで作ったように Views 名前空間に Shell クラスを作っているケースについて考えてみます。Shell に対して上記の属性を追加すると ViewModels 名前空間の ShellViewModel が DataContext に設定されます。以下のような ViewModel を定義してみました。

```cs
using System.ComponentModel;

namespace ViewModelLocatorSampleApp.ViewModels
{
    // WPF は INotifyPropertyChanged を実装していないものを DataContext に設定すると
    // メモリリークの原因になるので実装するようにしましょう
    // 後述しますが、Prism では自分で実装することはほとんどありませんが、ここではまだ自分で実装しておきます
    public class ShellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Message => "Hello world";

    }
}
```

Shell のほうで、Message プロパティをバインドするような TextBlock を設置します。

```xml
<Window
    x:Class="ViewModelLocatorSampleApp.Views.Shell"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:local="clr-namespace:ViewModelLocatorSampleApp.Views"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:prism="http://prismlibrary.com/"
    Title="Shell"
    Width="300"
    Height="300"
    prism:ViewModelLocator.AutoWireViewModel="True"
    mc:Ignorable="d">
    <Grid>
        <TextBlock Text="{Binding Message}" />
    </Grid>
</Window>
```

実行すると以下のようになります。

![Shell](Images/shell.png)

このとき、ShellViewModel は自動的に DI コンテナから取得されています。このシリーズでは Prism.Unity を使用しているので Unity からインスタンスが取得されています。そのため、以下のようにすることで、オブジェクトを外部からインジェクションすることが出来ます。

```cs
using System.ComponentModel;

namespace ViewModelLocatorSampleApp.Models
{
    public class MessageProvider : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Message => "Hello world";
    }
}
```

このクラスをインジェクションするように ViewModel を書き換えます。

```cs
using System.ComponentModel;
using ViewModelLocatorSampleApp.Models;

namespace ViewModelLocatorSampleApp.ViewModels
{
    // WPF は INotifyPropertyChanged を実装していないものを DataContext に設定すると
    // メモリリークの原因になるので実装するようにしましょう
    // 後述しますが、Prism では自分で実装することはほとんどありませんが、ここではまだ自分で実装しておきます
    public class ShellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MessageProvider MessageProvider { get; }

        public ShellViewModel(MessageProvider messageProvider)
        {
            MessageProvider = messageProvider;
        }
    }
}
```

XAML の Binding も MessageProvider を使うように書き換えます。

```xml
<Window x:Class="ViewModelLocatorSampleApp.Views.Shell"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:ViewModelLocatorSampleApp.Views"
        xmlns:prism="http://prismlibrary.com/"
        prism:ViewModelLocator.AutoWireViewModel="True"
        mc:Ignorable="d"
        Title="Shell" Height="300" Width="300">
    <Grid>
        <TextBlock Text="{Binding MessageProvider.Message}" />
    </Grid>
</Window>
```

実行結果は先ほどと変わらないため省略します。このように View と ViewModel の紐づけと、ViewModel への外部クラスの注入などが使えるのが Prism を使ったときの便利な点です。

後程の章で紹介しますが View と ViewModel の紐づけルールはカスタマイズ可能です。あくまでデフォルトの挙動では、ここで説明したルールでの紐づけになります。
