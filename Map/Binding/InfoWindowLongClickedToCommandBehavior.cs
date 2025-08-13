using System;
using System.Collections.Generic;
using System.Text;
#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class InfoWindowLongClickedToCommandBehavior : EventToCommandBehaviorBase
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.InfoWindowLongClicked += OnInfoWindowLongClicked;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.InfoWindowLongClicked -= OnInfoWindowLongClicked;
        }

        private void OnInfoWindowLongClicked(object sender, InfoWindowLongClickedEventArgs infoWindowLongClickedEventArgs)
        {
            Command?.Execute(infoWindowLongClickedEventArgs);
        }
    }
}
