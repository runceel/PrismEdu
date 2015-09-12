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
