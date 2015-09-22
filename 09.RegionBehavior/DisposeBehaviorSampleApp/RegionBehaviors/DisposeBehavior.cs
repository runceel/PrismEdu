using Prism.Common;
using Prism.Regions;
using System;
using System.Collections.Specialized;

namespace DisposeBehaviorSampleApp.RegionBehaviors
{
    class DisposeBehavior : IRegionBehavior
    {
        public const string Key = nameof(DisposeBehavior);

        public IRegion Region { get; set; }

        public void Attach()
        {
            this.Region.Views.CollectionChanged += this.Views_CollectionChanged;
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Action<IDisposable> callDispose = d => d.Dispose();
                foreach (var o in e.OldItems)
                {
                    MvvmHelpers.ViewAndViewModelAction(o, callDispose);
                }
            }
        }
    }
}
