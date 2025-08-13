using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class TakeSnapshotBehavior : BehaviorBase<Map>
    {
        public static readonly BindableProperty RequestProperty = BindableProperty.Create("Request", typeof(TakeSnapshotRequest), typeof(TakeSnapshotBehavior), null, propertyChanged: OnRequestChanged);

        private static void OnRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((TakeSnapshotBehavior)bindable).OnRequestChanged(oldValue as TakeSnapshotRequest, newValue as TakeSnapshotRequest);
        }

        private void OnRequestChanged(TakeSnapshotRequest oldValue, TakeSnapshotRequest newValue)
        {
            if (oldValue != null)
            {
                oldValue.TakeSnapshotBehavior = null;
            }
            if (newValue != null)
            {
                newValue.TakeSnapshotBehavior = this;
            }
        }

        public Task<Stream> TakeSnapshot()
        {
            return AssociatedObject.TakeSnapshot();
        }
    }
}
