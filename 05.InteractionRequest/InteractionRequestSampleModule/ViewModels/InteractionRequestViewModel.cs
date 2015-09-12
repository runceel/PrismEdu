using InteractionRequestSampleModule.Models;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionRequestSampleModule.ViewModels
{
    class InteractionRequestViewModel : BindableBase
    {
        public InteractionRequest<Notification> NotificationRequest { get; } = new InteractionRequest<Notification>();

        public InteractionRequest<Confirmation> ConfirmationRequest { get; } = new InteractionRequest<Confirmation>();

        public InteractionRequest<InputNotification> InputNotificationRequest { get; } = new InteractionRequest<InputNotification>();

        public DelegateCommand NotificationCommand { get; }

        public DelegateCommand ConfirmationCommand { get; }

        public DelegateCommand InputNotificationCommand { get; }

        private string confirmationMessage;

        public string ConfirmationMessage
        {
            get { return this.confirmationMessage; }
            set { this.SetProperty(ref this.confirmationMessage, value); }
        }

        private string outputText;

        public string OutputText
        {
            get { return this.outputText; }
            set { this.SetProperty(ref this.outputText, value); }
        }

        public InteractionRequestViewModel()
        {
            this.NotificationCommand = new DelegateCommand(this.NotificationCommandExecute);
            this.ConfirmationCommand = new DelegateCommand(this.ConfirmationCommandExecute);
            this.InputNotificationCommand = new DelegateCommand(this.InputNotificationExecute);
        }

        private void NotificationCommandExecute()
        {
            this.NotificationRequest.Raise(new Notification { Title = "Alert", Content = "Notification message." });
        }

        private async void ConfirmationCommandExecute()
        {
            var result = await this.ConfirmationRequest.RaiseAsync(new Confirmation { Title = "Confirm", Content = "OK?" });
            this.ConfirmationMessage = result.Confirmed + " selected";
        }

        private async void InputNotificationExecute()
        {
            var result = await this.InputNotificationRequest.RaiseAsync(new InputNotification { Title = "Input" });
            this.OutputText = $"Input text is {result.InputText}.";
        }
    }
}
