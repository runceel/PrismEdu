using EventAggregatorSample.Infrastructure;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EventAggregatorSample.Output2.Models
{
    public class MultiValueProvider : IMultiValueProvider
    {
        private ObservableCollection<int> Source { get; } = new ObservableCollection<int>();

        public IReadOnlyCollection<int> Values { get; }

        private SubscriptionToken Token { get; }

        public MultiValueProvider(IEventAggregator eventAggregator)
        {
            this.Values = new ReadOnlyObservableCollection<int>(this.Source);

            eventAggregator
                .GetEvent<InputValueEvent>()
                .Subscribe(
                    x => this.Source.Insert(0, x.Value), 
                    ThreadOption.UIThread);
        }
    }
}
