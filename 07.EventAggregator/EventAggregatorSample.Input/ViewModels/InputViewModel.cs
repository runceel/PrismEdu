using EventAggregatorSample.Input.Models;
using Microsoft.Practices.Unity;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAggregatorSample.Input.ViewModels
{
    public class InputViewModel
    {
        [Dependency]
        public IValuePublisher ValuePublisher { get; set; }

        public DelegateCommand InputCommand { get; }

        public InputViewModel()
        {
            this.InputCommand = new DelegateCommand(() =>
            {
                this.ValuePublisher.Publish();
            });
        }
    }
}
