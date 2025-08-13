using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Database;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.MVVM;
using CFAN.SchoolMap.Services.Places;
#if IOS || MACCATALYST
using Foundation;
#endif
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Places.Common;
using ISO3166;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using CFAN.SchoolMap.Maui.Database;

namespace CFAN.SchoolMap.ViewModels
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public class SearchViewModel : BaseVM, IQueryAttributable
    {
        private readonly IPlaceService _placesService;
        private string _text;
        private static Location _location;
        private IRepository _repository;

        public SearchViewModel()
        {
            SessionToken = Guid.NewGuid().ToString();
            TextChangedCommand = new SafeCommand(DoSearch);
            PlaceClickedCommand = new SafeCommand(ShowPlace);
            _placesService = DependencyService.Get<IPlaceService>();
            _repository = DependencyService.Get<IRepository>();
            TaskHelper.SafeRun(LoadLocation);
        }

        private static async Task LoadLocation()
        {
            _location = await Geolocation.GetLastKnownLocationAsync();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            var pars = Param.DecodeValues(query);
            var cc = pars[nameof(Country)];
            Country = Country.List.First(c => c.ThreeLetterCode == cc);
        }

        public ICommand PasteCommand => new SafeCommand(async () =>
        {
            Text = await Clipboard.GetTextAsync();
        });

        public Country Country { get; set; }

        public string SessionToken { get; set; }

        public ICommand TextChangedCommand { get; }
        public ICommand PlaceClickedCommand { get; }

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    if (_text?.Length > 3)
                    {
                        DoSearch();
                    }
                    else
                    {
                        Predictions.Clear();
                        Notify(nameof(HasResult));
                        Notify(nameof(HasNoResult));
                    }

                    Notify(nameof(Text));
                }
            }
        }

        public bool HasNoResult => !HasResult;
        public bool HasResult => Predictions.Any();

        public ObservableCollection<PlaceSearchResult> Predictions { get; set; } = new ObservableCollection<PlaceSearchResult>();

        public string PlusCode { get; set; }
        public string PlaceName { get; set; }

        private async Task ShowPlace(object item)
        {
            var prediction = (PlaceSearchResult) item;
            PlusCode = prediction.PlusCode??(await prediction.GetPlusCodeAction());
            PlaceName = prediction.Title;
            await Shell.Current.GoToAsync(new ShellNavigationState(".." + 
                    Param.EncodeParameters(
                        new Param(nameof(PlusCode), PlusCode),
                        new Param(nameof(PlaceName), PlaceName))));
        }

        private void DoSearch()
        {
            if (_location != null)
            {
                Predictions.Clear();
                TaskHelper.SafeRun(SearchGoogle, false);
                TaskHelper.SafeRun(SearchDb, false);
            }
        }

        private async Task SearchGoogle()
        {
            var places = await _placesService.AutocompleteSearch(_location.Latitude, _location.Longitude, Text, SessionToken,
                Country.TwoLetterCode);
            Predictions.AddRange(places.Select(ToPlaceSearchResult));
            Notify(nameof(HasResult));
            Notify(nameof(HasNoResult));
        }

        private async Task SearchDb()
        {
            Predictions.AddRange(await _repository.SearchSchools(Country.ThreeLetterCode, Text));
            Notify(nameof(HasResult));
            Notify(nameof(HasNoResult));
        }

        private PlaceSearchResult ToPlaceSearchResult(Prediction prediction)
        {
            return new PlaceSearchResult
            {
                Title = prediction.StructuredFormatting.MainText,
                Subtitle = prediction.StructuredFormatting.SecondaryText,
                GetPlusCodeAction = ()=>_placesService.GetPluscodeOfPlace(prediction.PlaceId)
            };
        }
    }
}