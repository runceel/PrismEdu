using EventAggregatorSample.Infrastructure;
using Prism.Events;
using Prism.Mvvm;

namespace EventAggregatorSample.Output1.Models
{
    public class SingleValueProvider : BindableBase, ISingleValueProvider
    {
        private int value;

        public int Value
        {
            get { return this.value; }
            private set { this.SetProperty(ref this.value, value); }
        }

        public SingleValueProvider(IEventAggregator eventAggregator)
        {
            eventAggregator
                .GetEvent<InputValueEvent>()
                .Subscribe(x => this.Value = x.Value, ThreadOption.UIThread);
        }
    }
}
