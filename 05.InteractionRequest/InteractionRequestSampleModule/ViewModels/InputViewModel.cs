using InteractionRequestSampleModule.Models;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;

namespace InteractionRequestSampleModule.ViewModels
{
    class InputViewModel : BindableBase, IInteractionRequestAware
    {
        // IInteractionRequestAware
        public Action FinishInteraction { get; set; }
        private INotification notification;
        public INotification Notification
        {
            get { return this.notification; }
            set
            {
                this.notification = value;
                // initialize
                this.InputText = "";
            }
        }

        private string inputText;

        public string InputText
        {
            get { return this.inputText; }
            set { this.SetProperty(ref this.inputText, value); }
        }

        public DelegateCommand OKCommand { get; }

        public InputViewModel()
        {
            this.OKCommand = new DelegateCommand(() =>
                {
                    ((InputNotification)this.Notification).InputText = this.InputText;
                    this.FinishInteraction();
                },
                () => !string.IsNullOrWhiteSpace(this.InputText))
                .ObservesProperty(() => this.InputText);
        }
    }
}
