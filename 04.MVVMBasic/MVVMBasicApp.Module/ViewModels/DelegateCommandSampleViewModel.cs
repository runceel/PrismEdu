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
