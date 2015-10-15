using Prism.Commands;
using Prism.Windows.Mvvm;

namespace HelloWorldApp.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
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

        public DelegateCommand CreateOutputCommand { get; }

        public MainPageViewModel()
        {
            this.CreateOutputCommand = new DelegateCommand(() =>
                {
                    this.Output = $"{Input}が入力されました";
                },
                () => !string.IsNullOrWhiteSpace(this.Input))
                .ObservesProperty(() => this.Input);
        }
    }
}
