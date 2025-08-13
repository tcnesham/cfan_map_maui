using System;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.ViewModels;
using Maui.GoogleMaps;

namespace CFAN.SchoolMap.Views
{
    public partial class SchoolMapPage : ContentPage
    {
        private static bool _firstLoad = true;
        public SchoolMapPage()
        {
            InitializeComponent();
             BindingContext = VM = new SchoolMapVM();
        }

        public SchoolMapVM VM { get;}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.RotateGesturesEnabled = true;
            map.UiSettings.TiltGesturesEnabled = false;
            map.UiSettings.ZoomControlsEnabled = false;
            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            map.MapStyle = MapStyle.FromJson(
@"[
    {
    featureType: ""all"",
        elementType: ""labels"",
        stylers: [
        { visibility: ""on"" }
        ]
    }
]"); ;
            VM.MapControl = map;
            if (_firstLoad)
            {
                _firstLoad = false;
                await TaskHelper.SafeRun(VM.NavigateToCurrentPosition());
            }
        }

        private void MapStreet(object sender, CheckedChangedEventArgs e)
        {
            map.MapType = MapType.Street;
        }

        private void MapSatellite(object sender, CheckedChangedEventArgs e)
        {
            map.MapType = MapType.Hybrid;
        }
    }
}