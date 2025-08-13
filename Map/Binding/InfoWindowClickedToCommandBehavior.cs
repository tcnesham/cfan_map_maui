#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class InfoWindowClickedToCommandBehavior : EventToCommandBehaviorBase
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.InfoWindowClicked += OnInfoWindowClicked;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.InfoWindowClicked -= OnInfoWindowClicked;
        }

        private void OnInfoWindowClicked(object sender, InfoWindowClickedEventArgs infoWindowClickedEventArgs)
        {
            Command?.Execute(infoWindowClickedEventArgs);
        }
    }
}
