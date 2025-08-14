using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
    public abstract class EventToCommandBehaviorBase : BehaviorBase<Map>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(MapClickedToCommandBehavior), default(ICommand));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        internal EventToCommandBehaviorBase()
        {
        }
    }
}
