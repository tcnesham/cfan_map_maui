using System;
using CFAN.SchoolMap.Helpers;
using Microsoft.Maui.Controls.Maps;
using CFAN.SchoolMap.ViewModels;


namespace CFAN.SchoolMap.Views
{
    public partial class MarketMapPage : ContentPage
    {
        private static bool _firstLoad = true;
        public MarketMapPage()
        {
            InitializeComponent();
             BindingContext = VM = new MarketMapVM();
        }

        public MarketMapVM VM { get;}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Microsoft.Maui.Controls.Maps has limited UI settings; no MapStyle API
            VM.MapControl = map;
            if (_firstLoad)
            {
                _firstLoad = false;
                await TaskHelper.SafeRun(VM.NavigateToCurrentPosition());
            }
        }

        private void MapStreet(object sender, CheckedChangedEventArgs e)
        {
            map.MapType = Microsoft.Maui.Maps.MapType.Street;
        }

        private void MapSatellite(object sender, CheckedChangedEventArgs e)
        {
            map.MapType = Microsoft.Maui.Maps.MapType.Hybrid;
        }
    }
}