# MVVMの基本クラス

ここでは、Prismで提供されているMVVMの基本クラスについて説明します。

## BindableBaseクラス

BindableBaseクラスはシンプルなINotifyPropertyChangedの実装クラスです。SetPropertyメソッドを使って、シンプルにプロパティを定義出来ます。
例えば、Inputというプロパティがあり、OutputというInputをすべて大文字にしたプロパティを持つクラスの定義は以下のようになります。

```cs
using Prism.Mvvm;

namespace MVVMBasicApp.Module.ViewModels
{
    class BindableBaseSampleViewModel : BindableBase
    {
        public string HeaderText { get; } = "BindableBaseSample";

        private string input;

        public string Input
        {
            get { return this.input; }
            set
            {
                if (this.SetProperty(ref this.input, value))
                {
                    this.Output = this.Input?.ToUpper();
                }
            }
        }

        private string output;

        public string Output
        {
            get { return this.output; }
            private set { this.SetProperty(ref this.output, value); }
        }

    }
}
```

BindableBaseクラスでは、上記のようにフィールドを定義して、setメソッドの中でSetProperty(ref, this.fieldName, value)という形で値の設定を行います。
これだけで、PropertyChangedイベントまで発行してくれます。

## DelegateCommandクラス

MVVMで使用するCommandの実装クラスもPrismでは提供しています。
DelegateCommandがそのクラスになります。
DelegateCommandは、コンストラクタの引数にExecute時に実行される処理と、CanExecute時に実行される処理を渡します。CanExecute時の処理は省略可能で、省略した場合は常に実行可能なコマンドになります。

Prism 6.0のDelegateCommandの新機能として、CanExecuteChangedの呼び出しをプロパティの変更を監視して呼び出してくれる機能が追加されました。
ObservePropertyメソッドがそれになります。以下のように使用します。

```cs
command.ObserveProperty(() => this.Hoge);
```

これで、Hogeプロパティを監視してHogeプロパティに変更があったらCanExecuteChangedを呼び出してくれます。このほかに、bool型のプロパティを受け取り、その値をCanExecuteの結果として返しつつプロパティの変更監視をするObserveCanExecuteメソッドも提供されています。

Commandを実行したら、Inputプロパティの値を大文字にしてOutputプロパティに変換するコマンド（Inputが未入力の場合は実行できない）を持ったViewModelは以下のようになります。

```cs
using Prism.Commands;
using Prism.Mvvm;

namespace MVVMBasicApp.Module.ViewModels
{
    class DelegateCommandSampleViewModel : BindableBase
    {
        public string HeaderText { get; } = "DelegateCommandSample";

        private string input;

        public string Input
        {
            get { return this.input; }
            set { this.SetProperty(ref this.input, value); }
        }

        private string output;

        public string Output
        {
            get { return this.output; }
            set { this.SetProperty(ref this.output, value); }
        }

        public DelegateCommand ToUpperCommand { get; }

        public DelegateCommandSampleViewModel()
        {
            // create command
            this.ToUpperCommand = new DelegateCommand(() =>
                {
                    this.Output = this.Input.ToUpper();
                },
                () => !string.IsNullOrWhiteSpace(this.Input));

            // CanExecuteChanged trigger
            this.ToUpperCommand.ObservesProperty(() => this.Input);
        }
    }
}
```

