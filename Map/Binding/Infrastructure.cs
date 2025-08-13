using System.Diagnostics.CodeAnalysis;
using CFAN.SchoolMap.Maui.GoogleMaps.Bindings;
namespace Xamarin
{
    public static class FormsGoogleMapsBindings
    {
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public static void Init()
        {
            new BindingCirclesBehavior();
            new BindingGroundOverlaysBehavior();
            new BindingPinsBehavior();
            new BindingPolygonsBehavior();
            new BindingPolylinesBehavior();
            new BindingTileLayersBehavior();
            new BindingRegionBehavior();
            new MapClickedToCommandBehavior();
            new MapLongClickedToCommandBehavior();
            new MoveToRegionBehavior();
            new PinClickedToCommandBehavior();
            new PinDragStartToCommandBehavior();
            new PinDraggingToCommandBehavior();
            new PinDragEndToCommandBehavior();
            new SelectedPinChangedToCommandBehavior();
            new InfoWindowClickedToCommandBehavior();
        }
    }
}
