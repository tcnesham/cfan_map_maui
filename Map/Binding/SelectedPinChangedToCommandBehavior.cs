#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class SelectedPinChangedToCommandBehavior : EventToCommandBehaviorBase
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SelectedPinChanged += OnSelectedPinChanged;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SelectedPinChanged -= OnSelectedPinChanged;
        }

        private void OnSelectedPinChanged(object sender, SelectedPinChangedEventArgs args)
        {
            Command?.Execute(args);
        }
    }
}
