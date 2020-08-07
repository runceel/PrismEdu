using Prism.Commands;
using Prism.Mvvm;

namespace MvvmBasicApp.HelloWorld.ViewModels
{
    public class DelegateCommandSampleViewModel : BindableBase
    {
        public string HeaderText => "DelegateCommandSample";

        private string _input;
        public string Input
        {
            get { return _input; }
            // Input プロパティが変更されたら ToUpperCommandEnabled プロパティも偏向通知をする
            set { SetProperty(ref _input, value, () => RaisePropertyChanged(nameof(ToUpperCommandEnabled))); }
        }

        private string _outpu;
        public string Output
        {
            get { return _outpu; }
            private set { SetProperty(ref _outpu, value); }
        }

        private DelegateCommand _toUpperCommand;
        public DelegateCommand ToUpperCommand =>
            _toUpperCommand ?? (_toUpperCommand = new DelegateCommand(ExecuteToUpperCommand)
                // ToUpperCommandEnabled を CanExecute と紐づける
                .ObservesCanExecute(() => ToUpperCommandEnabled));

        private void ExecuteToUpperCommand()
        {
            Output = Input.ToUpper();
        }

        // Input が空の場合は実行できない
        public bool ToUpperCommandEnabled => !string.IsNullOrWhiteSpace(Input);
    }
}
