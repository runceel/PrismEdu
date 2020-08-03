using System.ComponentModel;
using ViewModelLocatorSampleApp.Models;

namespace ViewModelLocatorSampleApp.ViewModels
{
    // WPF は INotifyPropertyChanged を実装していないものを DataContext に設定すると
    // メモリリークの原因になるので実装するようにしましょう
    // 後述しますが、Prism では自分で実装することはほとんどありませんが、ここではまだ自分で実装しておきます
    public class ShellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MessageProvider MessageProvider { get; }

        public ShellViewModel(MessageProvider messageProvider)
        {
            MessageProvider = messageProvider;
        }
    }
}
