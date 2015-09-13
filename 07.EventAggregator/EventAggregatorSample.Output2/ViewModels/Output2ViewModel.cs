using EventAggregatorSample.Output2.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace EventAggregatorSample.Output2.ViewModels
{
    public class Output2ViewModel
    {
        public string Title { get; } = "Multi values";

        public ReadOnlyObservableCollection<string> Values { get; }

        public Output2ViewModel(IMultiValueProvider multiValueProvider)
        {
            var source = new ObservableCollection<string>();
            this.Values = new ReadOnlyObservableCollection<string>(source);
            ((INotifyCollectionChanged)multiValueProvider.Values).CollectionChanged += (_, e) =>
            {
                // 割り切ってAddしか処理しない
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    source.Insert(e.NewStartingIndex, e.NewItems.Cast<int>().Select(x => $"Input value is {x}").First());
                }
            };
        }
    }
}
