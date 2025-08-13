using System.Threading.Tasks;
using CFAN.Common.WPF;
using System.Windows.Input;
using CFAN.SchoolMap.Enumerations;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.Model;
using GoogleApi.Entities.Places.Search.Common.Enums;
using CFAN.SchoolMap.Pins.States;
using System;
using CFAN.SchoolMap.Pins;
using CFAN.SchoolMap.Services.Places;
using CFAN.SchoolMap.Services.PlusCodes;
using System.Linq;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using CFAN.SchoolMap.Maui.Model;

namespace CFAN.SchoolMap.ViewModels
{
    public class MarketMapVM : MapBaseVM<MarketPoint>
    {
        public MarketMapVM() : base()
        {
            Title = "CFAN Market map";
        }

        protected MarketInfo _currentMarketInfo;

        public MarketInfo CurrentMarketInfo
        {
            get => _currentMarketInfo;
            set => SetProperty(ref _currentMarketInfo, value);
        }

        public override Modul modul => Modul.Markets;

        public override bool HasAnyModulRole => Repository.HasMarketRoles;

        public override SearchPlaceType[] ModulSearchPlaceTypes => new[]{
            SearchPlaceType.ShoppingMall,
            SearchPlaceType.BusStation,
            SearchPlaceType.SuperMarket,
            SearchPlaceType.TrainStation
        };

        public override bool CanAddPlaces => HasRole(Role.Outreaches_add);

        public override bool CanPlanVisit => HasRole(Role.Outreaches_visit);

        public override bool CanVisitPlace => HasRole(Role.Outreaches_visit);

        protected override string CopyPlaceNameMessage => "The market name copied into the clipboard.";

        public ICommand PlaceVisitedCommand => new SafeCommand(() =>
        {
            if (SelectedPlace == null) return;
            CurrentMarketInfo = new MarketInfo(SelectedPlace.PlusCode);
            IsUpdate = false;
            IsPlaceDetailVisible = true;
        });

        protected async override Task LoadPlaceInfo()
        {
            var v = await Repository.GetMarketInfo(_selectedPlace);
            if (v != null)
            {
                CurrentMarketInfo = v;
                CurrentNote = "";
                IsUpdate = true;
                IsPlaceDetailVisible = true;
            }   
        }

        protected override MarketPoint FindPlace(string v)
        {
            return CountryPlaces.FindPlace(v);
        }

        protected override MarketPoint CountryPlacesAdd(PinDesign pd, string v)
        {
            return CountryPlaces.Add(pd, v);
        }

        protected override void OnQueryPlaceUpdate(string plusCode, string placeName)
        {
            if (CountryPlaces != null)
            {
                var place = CountryPlaces.FindPlace(plusCode);
                if (place == null || place.Type == PlaceStates.UnknownState)
                {
                    GetAndShowPin(placeName, PinDesignFactory.UnknownMarket, plusCode);
                }
            }
        }

        protected override async Task DoSearch(bool shortSearch)
        {
            if (CountryPlaces == null) return;
            try
            {

                var placesService = DependencyService.Get<IPlaceService>();
                var places = await placesService.NearBySearch(VisibleRegion, shortSearch, ModulSearchPlaceTypes);


                var cnt = 0;

                foreach (var p in places)
                {
                    var plusCode = PlusCodeHelper.ToPlusCode(p.Latitude, p.Longitude);
                    if (CountryPlaces != null && !CountryPlaces.ContainsPlace(plusCode))
                    {
                        cnt++;
                        var np = CountryPlaces.Add(PinDesignFactory.New, plusCode);
                        np.Name = p.Name;
                        GetAndShowPin(p.Name, PinDesignFactory.New, np.PlusCode);
                    }
                }
                Message = (cnt == 0)
                    ? GuiMessages.NothingNewFound(modul)
                    : $"{cnt} new {(cnt > 1 ? GuiMessages.PlaceholderPlural(modul) : GuiMessages.Placeholder(modul))} found.";
                IsNextEnabled = cnt > 0;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, false, null, "Search failed!\n" + e.Message);
            }
        }

        protected override async Task OnIgnorePlace()
        {
            await SavePlaceTypeInSearch(PinDesignFactory.Ignored);
        }

        protected override void ChangePinType(MarketPoint place, PinDesign design)
        {
            place = CountryPlaces.ChangePlaceType(place.PlusCode, design, SelectedTeamChar);
            if (place == null) return;
            var pin = GetAndShowPin(place.Name, design, place.PlusCode);
            if (pin == null) return;
            SetupPin(place, design, pin);
            Repository.SavePlace(CountryCode, place);
        }

        protected override async Task ShowCountryPins()
        {
            using (Dialogs.Loading("Loading country data ..."))
            {
                CountryPlaces = await LoadCountryData(CountryCode, false);
                Pins.Clear();
                _pinsIndex.Clear();
                foreach (var p in CountryPlaces.Places)
                {
                    try
                    {
                        GetAndShowPin(BasePoint.PlaceNamePlaceholder, PinDesignFactory.GetByType(p.Type, p.TeamChar), p.PlusCode);
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, false, null, "Error occured during loading pins.");
                    }
                }
            }
        }

        private Task<CountryPlaces<MarketPoint>> LoadCountryData(string countryCode, bool compressData)
        {
            return Repository.MarketsLoadCountry(countryCode, compressData);
        }

        protected override async Task DoNextPlace()
        {
            SelectedPlace = CountryPlaces.FindNextNewPlace(
                SelectedPlace
                ?? new MarketPoint(CameraPlusCode, PlaceStates.UnknownState));//temporary point
            if (SelectedPlace == null)
            {
                Message = GuiMessages.SearchAgain(modul);
                IsNextEnabled = false;
            }
            else
            {
                try
                {
                    var newPos = SelectedPlace.Position;
                    await MoveCameraToPosition(newPos);
                }
                catch { }
            }
        }

        protected override Task SavePlace(string countryCode, MarketPoint p)
        {
            return Repository.SavePlace(countryCode, p);
        }

        public string CurrentNote { get; set; } = null;

        public ICommand SaveMarketInfoCommand => new SafeCommand(async () =>
        {
            if (!CanVisitPlace) return;
            IsPlaceDetailVisible = false;
            if (!CurrentNote.IsNullOrWhiteSpace())
            {
                var name = Repository.CurrentUser.Name;
                CurrentMarketInfo.Note += $"{DateTime.Now:yyyy-MM-dd} - {name}:\n{CurrentNote}\n";
            }

            SelectedPlace ??= FindPlace(CurrentMarketInfo.PlusCode);
            await Repository.SaveMarketInfo(CountryCode, CurrentMarketInfo, IsUpdate);
            CurrentMarketInfo.Country = CountryCode;
            await Repository.SavePlaceName(SelectedPlace);
            ChangePinType(SelectedPlace, PlaceStates.GetTypeByState(CurrentMarketInfo));
            SelectedPlace = null;
            CurrentNote = string.Empty;
            Notify(nameof(CurrentNote));
        }, false);

        protected override void OnTeamCharChanged()
        {
            HighlightTeamPlan(true);
        }

        protected override Task OnSavePlace()
        {
            return SavePlaceTypeInSearch(PinDesignFactory.UnknownMarket);
        }
    }
}
