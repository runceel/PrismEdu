using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAggregatorSample.Output2.Models
{
    public interface IMultiValueProvider
    {
        IReadOnlyCollection<int> Values { get; }
    }
}
