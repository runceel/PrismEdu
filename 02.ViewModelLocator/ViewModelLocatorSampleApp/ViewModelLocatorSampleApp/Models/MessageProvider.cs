using System.ComponentModel;

namespace ViewModelLocatorSampleApp.Models
{
    public class MessageProvider : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Message => "Hello world";
    }
}
