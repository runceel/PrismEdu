using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAggregatorSample.Output1.Models
{
    public interface ISingleValueProvider : INotifyPropertyChanged
    {
        int Value { get; }
    }
}
