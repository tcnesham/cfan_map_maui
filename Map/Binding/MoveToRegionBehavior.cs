﻿#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class MoveToRegionBehavior : BehaviorBase<Map>
    {
        public static readonly BindableProperty RequestProperty = BindableProperty.Create("Request", typeof(MoveToRegionRequest), typeof(MoveToRegionBehavior), default(MoveToRegionRequest), propertyChanged:OnRequestChanged);

        private static void OnRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((MoveToRegionBehavior)bindable).OnRequestChanged(oldValue as MoveToRegionRequest, newValue as MoveToRegionRequest);
        }

        private void OnRequestChanged(MoveToRegionRequest oldValue, MoveToRegionRequest newValue)
        {
            if (oldValue != null)
            {
                oldValue.MoveToRegionRequested -= OnMoveToRegionRequested;
            }
            if (newValue != null)
            {
                newValue.MoveToRegionRequested += OnMoveToRegionRequested;
            }
        }

        private void OnMoveToRegionRequested(object sender, MoveToRegionRequestedEventArgs moveToRegionRequestedEventArgs)
        {
            if (moveToRegionRequestedEventArgs.MapSpan != null)
            {
                AssociatedObject.MoveToRegion(
                    moveToRegionRequestedEventArgs.MapSpan, 
                    moveToRegionRequestedEventArgs.Animated);
            }
        }
    }
}
