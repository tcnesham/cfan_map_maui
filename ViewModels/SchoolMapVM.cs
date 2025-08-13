using System.Windows.Input;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.Enumerations;
using CFAN.SchoolMap.Pins.States;
using CFAN.SchoolMap.Helpers;
using GoogleApi.Entities.Places.Search.Common.Enums;
using CFAN.SchoolMap.Pins;
using CFAN.SchoolMap.Services.Places;
using CFAN.SchoolMap.Services.PlusCodes;
using System.Timers;
using CFAN.SchoolMap.Maui.Model;
using Pin = Maui.GoogleMaps.Pin;
using PinType = Maui.GoogleMaps.PinType;
using CFAN.SchoolMap.Maui.CountryBorders;

using Timer = System.Timers.Timer;

namespace CFAN.SchoolMap.ViewModels
{
    public class SchoolMapVM : MapBaseVM<PlacePoint>
    {
        protected const double HalfYearDays = 365 / 2.0;
        protected SchoolVisit _currentVisit;

        protected readonly Timer _statisticsTimer;

        public SchoolMapVM() : base()
        {
            Title = "CFAN School map";

            _statisticsTimer = new Timer(60000);
            _statisticsTimer.Elapsed += _statisticsTimer_Elapsed; ;
            _statisticsTimer.AutoReset = true;
            _statisticsTimer.Start();

            Initialization = TaskHelper.SafeRun(DisplayCountryStats);
        }

        public SchoolVisit CurrentVisit
        {
            get => _currentVisit;
            set
            {
                SetProperty(ref _currentVisit, value);
            }
        }

        protected override PlacePoint FindPlace(string v)
        {
            return CountryPlaces.FindPlace(v);
        }

        public override SearchPlaceType[] ModulSearchPlaceTypes => new[] { SearchPlaceType.School };

        public override bool CanPlanVisit => HasRole(Role.Schools_visit);

        public override bool CanVisitPlace => HasRole(Role.Schools_visit);

        public override bool CanAddPlaces => HasRole(Role.Schools_add);

        //public bool CanSaveVisit => CanVisitPlace;// && (!IsUpdate || CanChangeVisit);

        protected override string CopyPlaceNameMessage => "The school name copied into the clipboard.";

        public override Modul modul => Modul.Schools;

        public override bool HasAnyModulRole => Repository.HasSchoolRoles;

        //public bool CanChangeVisit => Auth.IsAdmin || (CurrentVisit?.Date ?? DateTime.Now) > DateTime.Now.AddDays(-HalfYearDays);

        public ICommand SaveSchoolVisitCommand => new SafeCommand(async () =>
        {
            if (!CanVisitPlace) return;
            if (CurrentVisit.IsNotASchool && await ConfirmDelete()) return;
            IsPlaceDetailVisible = false;
            SelectedPlace ??= FindPlace(CurrentVisit.PlusCode);
            await Repository.SaveVisit(CountryCode, CurrentVisit, IsUpdate);
            SelectedPlace.Country = CountryCode;
            await Repository.SavePlaceName(SelectedPlace);
            ChangePinType(SelectedPlace, PlaceStates.GetTypeByState(CurrentVisit));
            SelectedPlace = null;
        }, false);

        public ICommand PlaceVisitedCommand => new SafeCommand(() =>
        {
            if (SelectedPlace == null) return;
            CurrentVisit = new SchoolVisit(SelectedPlace.PlusCode);
            IsUpdate = false;
            IsPlaceDetailVisible = true;
        });

        protected async override Task LoadPlaceInfo()
        {
            if (_selectedPlace.CanHaveNote)
            {
                var v = await Repository.GetVisit(_selectedPlace);
                if (v != null)
                {
                    CurrentVisit = v;
                    IsUpdate = true;
                    IsPlaceDetailVisible = true;
                }
            }
            else
            {
                CurrentVisit = null;
                IsUpdate = true;
                IsPlaceDetailVisible = false;
            }

        }

        public CountryStat CurrentCountryStat => CountryPlaces?.GetStatistics();

        public void UpdateStatistics()
        {
            Notify(nameof(CurrentCountryStat));
        }

        protected void _statisticsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(UpdateStatistics);
        }

        protected override PlacePoint CountryPlacesAdd(PinDesign pd, string v)
        {
            return CountryPlaces.Add(pd, v);
        }

        protected override void OnQueryPlaceUpdate(string plusCode, string placeName)
        {
            if (CountryPlaces != null)
            {
                var place = CountryPlaces.FindPlace(plusCode);
                if (place == null || place.Type == PlaceStates.Ignored)
                {
                    GetAndShowPin(placeName, PinDesignFactory.SearchResult, plusCode);
                }
            }
        }

        protected override async Task OnIgnorePlace()
        {
            if (SelectedPlace?.Type == PinDesignFactory.Visited.TypeCh)
            {
                if (await ConfirmDelete()) return;
            }
            await SavePlaceTypeInSearch(PinDesignFactory.Ignored);
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

        protected override void ChangePinType(PlacePoint place, PinDesign design)
        {
            place = CountryPlaces.ChangePlaceType(place.PlusCode, design, SelectedTeamChar);
            if (place == null) return;
            var pin = GetAndShowPin(place.Name, design, place.PlusCode);
            if (pin == null) return;
            SetupPin(place, design, pin);
            Repository.SavePlace(CountryCode, place);
        }
        protected Task<CountryPlaces<PlacePoint>> LoadCountryData(string countryCode, bool compressData)
        {
            return Repository.SchoolsLoadCountry(countryCode, compressData);
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

                UpdateStatistics();
            }
            //await Repository.RepairCountryData(_countryPlaces);
        }

        protected override async Task DoNextPlace()
        {
            SelectedPlace = CountryPlaces.FindNextNewPlace(
                SelectedPlace
                ?? new PlacePoint(CameraPlusCode, PlaceStates.PlaceNotAPlace));//temporary point
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

        protected override async Task SavePlace(string countryCode, PlacePoint p)
        {
            await Repository.SavePlace(CountryCode, p);
        }

        protected override void OnTeamCharChanged()
        {
            HighlightTeamPlan(true);
        }

        protected async Task DisplayCountryStats()
        {
            //Repository.WriteDebugNoBorders();
            var stat = await Repository.GetStatistics();
            foreach (var s in stat)
            {
                if (s.CountryCode != CountryCode)
                {
                    var pos = await CountryBorderHelper.GetCountryPosition(s.CountryCode);

                    if (pos.Latitude != 0 && pos.Longitude != 0)
                    {
                        Pins.Add
                        (
                            new Pin
                            {
                                IsDraggable = false,
                                Type = PinType.Place,
                                Label = $"{s.CountryCode} - {s.Visualization}",
                                Position = pos,
                                Tag = s.CountryCode
                                //Icon = BitmapDescriptor.FromView(new CountryPinView(), s.CountryCode)
                            }
                        );
                    }
                }
            }
        }

        protected override Task OnCountryChanged()
        {
            return DisplayCountryStats();
        }

        protected override async Task OnSavePlace()
        {
            await SavePlaceTypeInSearch(PinDesignFactory.Unvisited);
        }
    }
}
