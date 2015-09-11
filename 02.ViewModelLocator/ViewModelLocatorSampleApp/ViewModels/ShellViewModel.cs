using Microsoft.Practices.Unity;
using ViewModelLocatorSampleApp.Models;

namespace ViewModelLocatorSampleApp.ViewModels
{
    class ShellViewModel
    {
        [Dependency]
        public MessageProvider MessageProvider { get; set; }
    }
}
