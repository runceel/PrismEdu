using Prism.Mvvm;

namespace MvvmBasicApp.HelloWorld.ViewModels
{
    public class BindableBaseSampleViewModel : BindableBase
    {
        public string HeaderText => "BindableBaseSample";

        private string _input;
        public string Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value, () => RaisePropertyChanged(nameof(Output))); }
        }

        public string Output => Input?.ToUpper();
    }
}
