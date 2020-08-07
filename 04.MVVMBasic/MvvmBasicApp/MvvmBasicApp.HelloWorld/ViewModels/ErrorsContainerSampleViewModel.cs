using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MvvmBasicApp.HelloWorld.ViewModels
{
    public class ErrorsContainerSampleViewModel : BindableBase, INotifyDataErrorInfo
    {
        public string HeaderText => "ErrorContainerSample";

        private string _input;

        [Required(ErrorMessage = "入力してください")]
        public string Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
        }

        private ErrorsContainer<string> ErrorsContainer { get; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ErrorsContainerSampleViewModel()
        {
            ErrorsContainer = new ErrorsContainer<string>(
                x => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(x)));
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!base.SetProperty(ref storage, value, propertyName))
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

        public bool HasErrors => ErrorsContainer.HasErrors;

        public IEnumerable GetErrors(string propertyName) => ErrorsContainer.GetErrors(propertyName);
    }
}
