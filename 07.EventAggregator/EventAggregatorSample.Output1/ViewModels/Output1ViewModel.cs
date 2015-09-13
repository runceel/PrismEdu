using EventAggregatorSample.Output1.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAggregatorSample.Output1.ViewModels
{
    public class Output1ViewModel : BindableBase
    {
        public string Title { get; } = "SingleValue";

        private string output;

        public string Output
        {
            get { return this.output; }
            set { this.SetProperty(ref this.output, value); }
        }

        public Output1ViewModel(ISingleValueProvider singleValueProvider)
        {
            singleValueProvider
                .PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == "Value")
                    {
                        this.Output = $"Input value is {singleValueProvider.Value}";
                    }
                };
        }
    }
}
