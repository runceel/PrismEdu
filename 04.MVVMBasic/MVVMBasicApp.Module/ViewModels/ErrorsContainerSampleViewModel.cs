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

        public IEnumerable GetErrors(string propertyName)
        {
            return this.ErrorsContainer.GetErrors(propertyName);
        }
        #endregion
    }
}
