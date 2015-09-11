using Microsoft.Practices.Unity;
using ModuleApp.HelloWorldModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleApp.HelloWorldModule.ViewModels
{
    class HelloWorldViewModel
    {
        [Dependency]
        public MessageProvider MessageProvider { get; set; }
    }
}
