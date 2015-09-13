using EventAggregatorSample.Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;

namespace EventAggregatorSample.Input.Models
{
    public class ValuePublisher : IValuePublisher
    {
        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        private Random Random { get; } = new Random();
        
        public void Publish()
        {
            this.EventAggregator
                .GetEvent<InputValueEvent>()
                .Publish(new InputValue { Value = this.Random.Next(100) });
        }        
    }
}
