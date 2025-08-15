using System;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.ViewModels;
using Microsoft.Maui.Controls.Maps;

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

    protected override void OnAppearing()
        {
            base.OnAppearing();
            // Microsoft.Maui.Controls.Maps does not expose UiSettings/MapStyle
            VM.MapControl = map;
            if (_firstLoad)
            {
                _firstLoad = false;
                // await TaskHelper.SafeRun(VM.NavigateToCurrentPosition());
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

        private void OnCountryButtonClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] Country button clicked!");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] BindingContext type: {BindingContext?.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] VM type: {VM?.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"[DEBUG] VM SearchStateCommand: {VM?.SearchStateCommand}");
            
            if (VM?.SearchStateCommand?.CanExecute(null) == true)
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] Manually executing SearchStateCommand");
                VM.SearchStateCommand.Execute(null);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] SearchStateCommand cannot execute or is null");
            }
        }
    }
}