using Microsoft.Practices.Unity;
using ModuleApp.HelloWorldModule.Models;

namespace ModuleApp.HelloWorldModule.ViewModels
{
    class HelloWorldViewModel
    {
        [Dependency]
        public MessageProvider MessageProvider { get; set; }
    }
}
