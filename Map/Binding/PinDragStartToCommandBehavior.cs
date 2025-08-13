﻿#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class PinDragStartToCommandBehavior : EventToCommandBehaviorBase
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.PinDragStart += OnPinDragStart;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.PinDragStart -= OnPinDragStart;
        }

        private void OnPinDragStart(object sender, PinDragEventArgs args)
        {
            Command?.Execute(args);
        }
    }
}
