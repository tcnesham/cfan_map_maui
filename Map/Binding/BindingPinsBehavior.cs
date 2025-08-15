using System.Collections.ObjectModel;
#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

using Maui.GoogleMaps;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class BindingPinsBehavior : BehaviorBase<Map>
    {
        private static readonly BindablePropertyKey ValuePropertyKey = BindableProperty.CreateReadOnly("Value", typeof(ObservableCollection<Pin>), typeof(BindingPinsBehavior), default(ObservableCollection<Pin>));

        public static readonly BindableProperty ValueProperty = ValuePropertyKey.BindableProperty;
        public ObservableCollection<Pin> Value
        {
            get => (ObservableCollection<Pin>)GetValue(ValueProperty);
            private set => SetValue(ValuePropertyKey, value);
        }

    protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            Value = bindable.Pins as ObservableCollection<Pin>;
        }

    protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            Value = null;
        }
    }
}
