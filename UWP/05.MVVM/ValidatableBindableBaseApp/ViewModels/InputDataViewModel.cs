using Prism.Windows.Validation;
using System.ComponentModel.DataAnnotations;

namespace ValidatableBindableBaseApp.ViewModels
{
    class InputDataViewModel : ValidatableBindableBase
    {
        private string input;

        [Required(ErrorMessage = "入力してください")]
        public string Input
        {
            get { return this.input; }
            set { this.SetProperty(ref this.input, value); }
        }

    }
}
