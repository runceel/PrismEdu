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

## ErrorsContainerクラス(WPFのみ)

Prismには、INotifyDataErrorInfoインターフェースの実装を補助するErrorsContainerクラスがあります。このクラスを使うことで簡単にINotifyDataErrorInfoを使った入力値の検証機能を持ったクラスを作成できます。
ErrorsContainerクラスは、型引数にエラーの型(大体string)を指定して使います。
コンストラクタには、ErrorsChangedイベントの呼び出し処理を渡します。
そして、INotifyDataErrorInfoインターフェースのHasErrsプロパティと、GetErrorsメソッドの処理をやってくれるメソッドを持っています。そのため、INotifyDataErrorInfoの実装クラスでは
ErrorsContainerに移譲するだけですみます。

あとは、任意のタイミングでSetErrors(propertyName, errorInfo)でエラー情報を設定して、ClearErrors(propertyName)でエラーのクリアをするだけです。以下に簡単にINotifyDataErrorInfoを実装したクラスの例を示します。

```cs
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MVVMBasicApp.Module.ViewModels
{
    class ErrorsContainerSampleViewModel : BindableBase, INotifyDataErrorInfo
    {
        public string HeaderText { get; } = "ErrorContainerSample";

        private string input;

        [Required(ErrorMessage = "入力してください")]
        public string Input
        {
            get { return this.input; }
            set { this.SetProperty(ref this.input, value); }
        }


        public ErrorsContainerSampleViewModel()
        {
            this.ErrorsContainer = new ErrorsContainer<string>(
                x => this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(x)));
        }

        #region Validation
        private ErrorsContainer<string> ErrorsContainer { get; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if(!base.SetProperty<T>(ref storage, value, propertyName))
            {
                return false;
            }

            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            var errors = new List<ValidationResult>();
            if (!Validator.TryValidateProperty(value, context, errors))
            {
                this.ErrorsContainer.SetErrors(propertyName, errors.Select(x => x.ErrorMessage));
            }
            else
            {
                this.ErrorsContainer.ClearErrors(propertyName);
            }

            return true;
        }

        public bool HasErrors
        {
            get
            {
                return this.ErrorsContainer.HasErrors;
            }
        }
		Data
        public IEnumerable GetErrors(string propertyName)
        {
            return this.ErrorsContainer.GetErrors(propertyName);
        }
        #endregion
    }
}
```

BindableBaseクラスのSetPropertyメソッドをオーバーライドして、DataAnnotationsを使った入力値の検証をしているクラスになります。Inputプロパティが未入力の場合はエラーになります。
