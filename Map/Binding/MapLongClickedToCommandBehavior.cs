﻿#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class MapLongClickedToCommandBehavior : EventToCommandBehaviorBase
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.MapLongClicked += OnMapLongClicked;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.MapLongClicked -= OnMapLongClicked;
        }

        private void OnMapLongClicked(object sender, MapLongClickedEventArgs args)
        {
            Command?.Execute(args);
        }
    }
}
