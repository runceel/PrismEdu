using ModuleApp.HelloWorld.Models;
using System.ComponentModel;

namespace ModuleApp.HelloWorld.ViewModels
{
    // メモリリーク防止のため INotifyPropertyChanged を実装
    public class HelloWorldViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly MessageProvider _messageProvider;

        public string Message => _messageProvider.Message;

        public HelloWorldViewModel(MessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }
    }
}
