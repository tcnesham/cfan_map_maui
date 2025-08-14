using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Timers;
using Microsoft.Maui.Authentication;
using ISO3166;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.Enumerations;
using CFAN.SchoolMap.Pins.States;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.Pins;
using CFAN.SchoolMap.Services.Places;
using CFAN.SchoolMap.Services.PlusCodes;
using CFAN.SchoolMap.Services.Auth;
using CFAN.SchoolMap.Maui.GoogleMaps;
using CFAN.SchoolMap.Map;
using GoogleApi.Entities.Places.Search.Common.Enums;
using CFAN.SchoolMap.Maui.Model;
using CFAN.SchoolMap.Maui.CountryBorders;
using CFAN.SchoolMap.Maui.Database;
using CFAN.SchoolMap.MVVM;
using Pin = CFAN.SchoolMap.Maui.GoogleMaps.Pin;
using PinType = CFAN.SchoolMap.Maui.GoogleMaps.PinType;
using Position = CFAN.SchoolMap.Maui.GoogleMaps.Position;
using Polygon = CFAN.SchoolMap.Maui.GoogleMaps.Polygon;
using MapSpan = CFAN.SchoolMap.Maui.GoogleMaps.MapSpan;
using Distance = CFAN.SchoolMap.Maui.GoogleMaps.Distance;
using CameraPosition = CFAN.SchoolMap.Maui.GoogleMaps.CameraPosition;
using CameraUpdate = CFAN.SchoolMap.Maui.GoogleMaps.CameraUpdate;
using CameraUpdateFactory = CFAN.SchoolMap.Maui.GoogleMaps.CameraUpdateFactory;
using Bounds = CFAN.SchoolMap.Maui.GoogleMaps.Bounds;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace CFAN.SchoolMap.ViewModels
{
    public abstract class MapBaseVM<TPoint> : BaseVM, IQueryAttributable
        where TPoint : BasePoint, new()
    {

        protected const double MinSelZoom = 12;
        protected readonly System.Timers.Timer _messageTimer;
        protected readonly Dictionary<string, Pin> _pinsIndex = new Dictionary<string, Pin>();
        protected string _message = string.Empty!;
        protected bool _hasMessage;
        protected bool _canImport;
        protected Pin _selectedPin = null!;
        protected TPoint _selectedPlace = null!;
        protected bool _isNextEnabled;
        protected char _selectedTeamChar='A';
        protected MapSpan _visibleRegion = MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromKilometers(1));
        protected ActionStates _actionState = ActionStates.ChoosingAction;
        protected string _countrySearch = string.Empty!;
        protected ActionStates _nextActionState;
        protected Country _country = null!;
        protected bool _isTeamSelectionVisible;
        protected CameraPosition _cameraPosition = new CameraPosition(new Position(0, 0), 1d);
        protected CameraUpdate _cameraUpdate = null!;
        protected ObservableCollection<Polygon> _borders = new ObservableCollection<Polygon>();
        protected bool _isPlaceDetailVisible;
        protected TPoint? _lastSelected;

        protected Pin _lastSelectedPin = null!;
        

        protected ObservableCollection<Pin> _pins = new ObservableCollection<Pin>();
        public MapBaseVM()
        {
            _messageTimer = new System.Timers.Timer(10000);
            _messageTimer.Elapsed += OnMessageTimedEvent;
            _messageTimer.AutoReset = false;

            

            var auth = DependencyService.Get<IAuth>();
            CurrentUserEmail = auth?.User?.Email ?? string.Empty;

            Repository = DependencyService.Get<IRepository>();
            if (Repository != null)
            {
                Repository.PlacesChanged += PlacesChanged;
            }
            Clipboard.ClipboardContentChanged += Clipboard_ClipboardContentChanged;
            Clipboard_ClipboardContentChanged(null, null);

            var teams = new List<Team>();
            for (char c = 'A'; c <= 'Z'; c++) teams.Add(new Team(c, $"Team {c}"));
            for (char c = '0'; c <= '9'; c++) teams.Add(new Team(c, $"Team {c}"));
            Teams = teams.ToArray();

            

        }

        public abstract Modul modul { get; }

        public abstract bool HasAnyModulRole { get; }

        public abstract SearchPlaceType[] ModulSearchPlaceTypes { get; }

        public abstract bool CanAddPlaces { get; }
        public abstract bool CanPlanVisit { get; }
        public abstract bool CanVisitPlace { get; }

        public Team[] Teams { get; set; }

        public string CurrentUserEmail { get; set; }


        public bool HasMessage
        {
            get => _hasMessage;
            set => SetProperty(ref _hasMessage, value);
        }

        public string Message
        {
            get => _message;
            set
            {
                SetProperty(ref _message, value);
                HasMessage = true;
                _messageTimer.Enabled = true;
            }
        }

        public string? ClipboardText { get; set; }

        public bool CanImport
        {
            get => _canImport;
            set => SetProperty(ref _canImport, value);
        }

        

        public Pin SelectedPin
        {
            get => _selectedPin;
            set
            {
                SetProperty(ref _selectedPin, value);
                if (value == null)
                {
                    if (SelectedPlace!=null) SelectedPlace = null;
                }
                else
                {
                    if (value.Type == PinType.Place) return;
                    SelectedPlace = FindPlace(PlusCodeHelper.ToPlusCode(value.Position));
                }
            }
        }

        protected abstract TPoint FindPlace(string v);

        public ICommand SearchCommand => new SafeCommand(DoSearch);

        public TPoint? SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                if (_selectedPlace?.PlusCode == value?.PlusCode) return;
                SetProperty(ref _selectedPlace, value);
                Notify(nameof(IsPlaceSelected));
                Notify(nameof(CanSavePlace));
                if (value?.PlusCode == null)
                {
                    SelectedPin = null;
                }
                else
                {
                    _pinsIndex.TryGetValue(value.PlusCode, out var pin);
                    SelectedPin = pin;
                    TaskHelper.SafeRun(AfterPinSelected(value, pin));
                }
            }
        }

        public bool IsPlaceSelected => SelectedPlace != null;

        public bool CanSavePlace => IsPlaceSelected && SelectedPlace != null && (SelectedPlace.Type == PinDesignFactory.New.TypeCh);

        public bool IsNextEnabled
        {
            get => _isNextEnabled;
            set
            {
                _isNextEnabled = value;
                Notify(nameof(IsNextEnabled));
            }
        }

        public char SelectedTeamChar
        {
            get => _selectedTeamChar;
            set
            {
                SetProperty(ref _selectedTeamChar, value);
                Notify(nameof(TeamColor));
                Notify(nameof(SelectedTeam));
                OnTeamCharChanged();
            }
        }

        protected abstract void OnTeamCharChanged();

        public Team SelectedTeam
        {
            get => Teams.FirstOrDefault(t=>t.Char==_selectedTeamChar);
            set
            {
                if (value != null && value.Char != SelectedTeamChar)
                {
                    SelectedTeamChar = value.Char;
                }
            }
        }

        public Color TeamColor => PinDesignFactory.GetByTeam(SelectedTeamChar).Color;

        public MapSpan VisibleRegion
        {
            get => _visibleRegion;
            set => SetProperty(ref _visibleRegion, value);
        }

        public bool CanOpenSearch => Country != null;

        public IEnumerable<Country> Countries
        {
            get
            {
                var ls = CountrySearch?.ToLower();
                return (CountrySearch == null)
                    ? Country.List
                    : Country.List.Where(c => c.Name.ToLower().Contains(ls) || c.ThreeLetterCode.ToLower() == ls);
            }
        }

        public Country Country
        {
            get => _country;
            set
            {
                SetProperty(ref _country, value);
                Notify(nameof(IsCountrySelected));
                Notify(nameof(CanOpenSearch));
            }
        }

        public string CountryCode => Country?.ThreeLetterCode;

        public string CountrySearch
        {
            get => _countrySearch;
            set { SetProperty(ref _countrySearch, value); Notify(nameof(Countries));}
        }
        public ICommand CountrySelectedCommand => new SafeCommand(async item => 
        { 
            Country = item as ISO3166.Country;
            _actionState = _nextActionState; 
            StateChanged();
            await ChangeCountry();
        });
        public bool IsCountriesVisible => _actionState == ActionStates.ChoosingCountry;
        
        public bool IsCountrySelected => Country != null;
        public bool IsSearchVisible => (_actionState == ActionStates.SearchingForPlaces) && !IsPlaceDetailVisible;

        public bool IsTeamSelectionVisible
        {
            get => _isTeamSelectionVisible;
            set => SetProperty(ref _isTeamSelectionVisible, value);
        }

        public bool IsActionsVisible => _actionState == ActionStates.ChoosingAction && IsCountrySelected && HasAnyModulRole;

        public bool IsPlanningVisible => (_actionState == ActionStates.PlanningVisits) && !IsPlaceDetailVisible;
        public bool IsVisitingVisible => (_actionState == ActionStates.VisitingPlaces) && !IsPlaceDetailVisible;

        public ICommand ShowCountryCommand => new SafeCommand(async () => await ChangeCountry());

        public string CameraPlusCode => PlusCodeHelper.ToPlusCode(CameraPosition.Target);

        public CameraPosition CameraPosition
        {
            get => _cameraPosition;
            set => SetProperty(ref _cameraPosition, value);
        }

        public CameraUpdate CameraUpdate
        {
            get => _cameraUpdate;
            set => SetProperty(ref _cameraUpdate, value);
        }

        public Command AddStateCommand => new SafeCommand(() =>
        {
            if (Country == null)
            {
                _actionState = ActionStates.ChoosingCountry;
                _nextActionState = ActionStates.SearchingForPlaces;
            }
            else
            {
                _actionState = ActionStates.SearchingForPlaces;
            }

            StateChanged();
        });

        public ObservableCollection<Polygon> Borders
        {
            get => _borders;
            set => SetProperty(ref _borders, value);
        }
        

        

        

        public ICommand CloseFromPlaceVisitCommand => new SafeCommand(() =>
        {
            IsPlaceDetailVisible = false;
        });

        public ICommand CopyPlaceNameCommand => new SafeCommand(async () =>
        {
            if (SelectedPlace == null || string.IsNullOrEmpty(SelectedPlace.Name)) return;
            await Clipboard.SetTextAsync(SelectedPlace.Name);
            Dialogs.Toast(CopyPlaceNameMessage);
        });

        protected abstract string CopyPlaceNameMessage { get; }

        public ICommand IgnorePlaceCommand => new SafeCommand(OnIgnorePlace, false);

        abstract protected Task OnIgnorePlace();


        public ICommand ImportPlaceCommand => new SafeCommand(async () =>
        {
            using (Dialogs.Loading("Importing places from the clipboard"))
            {
                if (!IsCountrySelected)
                {
                    Dialogs.Alert("Choose the country first!");
                    return;
                }

                if (Repository != null)
                {
                    await Repository.ImportFromText<PlacePoint>(CountryCode, ClipboardText);
                }
            }
        });
        public Task Initialization { get; set; } = Task.CompletedTask;

        

        public bool IsPlaceDetailVisible
        {
            get => _isPlaceDetailVisible;
            set
            {
                SetProperty(ref _isPlaceDetailVisible, value);
                Notify(nameof(IsVisitingVisible));
                Notify(nameof(IsPlanningVisible));
                Notify(nameof(IsSearchVisible));
            }
        }

        public bool IsUpdate { get; set; } = true;

        public ICommand MapClickedCommand => new SafeCommand<MapLongClickedEventArgs>(async args =>
        {
            if (!CanAddPlaces)
            {
                await Dialogs.AlertAsync(GuiMessages.NotHasAddPermission(modul));
                return;
            }

            if (!IsCountrySelected)
            {
                await Dialogs.AlertAsync(GuiMessages.ChooseCountryFirstThenAdd(modul));
                return;
            }
            if (_actionState != ActionStates.SearchingForPlaces)
            {
                if (!await Dialogs.ConfirmAsync(GuiMessages.NotInModeToAdd(modul))) return;
            }

            PinDesign pd = PinDesignFactory.Unvisited;
            if (_actionState == ActionStates.SearchingForPlaces) pd = PinDesignFactory.Unvisited;
            if (_actionState == ActionStates.PlanningVisits) pd = PinDesignFactory.GetByTeam(SelectedTeamChar);
            if (_actionState == ActionStates.VisitingPlaces) pd = PinDesignFactory.GetByTeam(SelectedTeamChar);
            if (pd.Type != PinDesignFactory.Ignored.Type)
            {
                var p = CountryPlacesAdd(pd, PlusCodeHelper.ToPlusCode(args.Point));
                GetAndShowPin(PlacePoint.PlaceNamePlaceholder, pd, p.PlusCode);
                GoTo(args.Point);
                await SavePlace(CountryCode, p);
            }
        }, false);

        protected abstract TPoint CountryPlacesAdd(PinDesign pd, string v);

        public Maui.GoogleMaps.Map MapControl { get; set; } = null!;

        public ICommand MarkPlaceCommand => new SafeCommand(() =>
        {
            if (SelectedPlace == null) return;
            ChangePinType(SelectedPlace, PinDesignFactory.GetByTeam(SelectedTeamChar));
        });

        public ICommand NavigateToSelectedCommand => new SafeCommand(async () =>
        {
            if (SelectedPlace == null) return;
            var pos = SelectedPlace.Position;
            // Using DeviceInfo.Platform per MAUI migration guidance.
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var location = new Location(pos.Latitude, pos.Longitude);
                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                await Microsoft.Maui.ApplicationModel.Map.OpenAsync(location, options);
            }
            else
            {
                var current = await GetCurrentPosition();
                if (current == null)
                {
                    Message = "Can not detect your position. Check if GPS is enabled on this device.";
                    return;
                }
                var dest = new Location(pos.Latitude, pos.Longitude);
                // if GoogleMap installed
                if (await Launcher.CanOpenAsync("comgooglemaps://"))
                {
                    await Launcher.OpenAsync($"comgooglemaps://?saddr={current.ToGpsString()}&daddr={dest.ToGpsString()}&directionsmode=driving");
                }
                else
                {
                    // if GoogleMap App is not installed
                    await Launcher.OpenAsync($"https://www.google.com/maps/dir/?saddr={current.ToGpsString()}&daddr={dest.ToGpsString()}&directionsmode=driving");
                }
            }
        });

        public ICommand NextPlaceCommand => new SafeCommand(DoNextPlace);

        public ICommand OpenSearchCommand => new SafeCommand(async () =>
        {
            if (!CanOpenSearch || Country == null) return;
            await Shell.Current.GoToAsync(new ShellNavigationState("SearchPage"+ Param.EncodeParameters(new Param(nameof(SearchViewModel.Country), Country.ThreeLetterCode))));
        });

        public ICommand PinDoubleClickedCommand => new SafeCommand<PinClickedEventArgs>(async args =>
        {
            if (!(args.Pin.Tag is string)) return; //only for country pins
            var country = args.Pin.Tag as string;
            if (IsCountrySelected)
            {
                if (!await Dialogs.ConfirmAsync($"Do you want to load country {country}?")) return;
            }

            var newCountry = ISO3166.Country.List.FirstOrDefault(c => c.ThreeLetterCode == country);
            if (newCountry == null) return;
            Country = newCountry;
            _actionState = ActionStates.ChoosingAction;
            StateChanged();
            await ChangeCountry();
        }, false);

        public ICommand RenamePinCommand => new SafeCommand<InfoWindowClickedEventArgs>(async args =>
        {
            if (args.Pin == null) return;
            var selected = FindPlace(PlusCodeHelper.ToPlusCode(args.Pin.Position));
            var newName = await Dialogs.PromptAsync("Rename this place:", "Rename", placeholder: selected.Name);
            if (newName.Ok)
            {
                selected.Name = args.Pin.Label = newName.Text;
                await Repository.SavePlaceName(selected);
            }
        }, false);
        
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        

        public ICommand PoiClickedCommand => new SafeCommand<Map.Poi>(async args =>
        {
            if (!IsCountrySelected)
            {
                await Dialogs.AlertAsync(GuiMessages.ChooseCountryFirstThenAdd(modul));
                return;
            }
            if (_actionState != ActionStates.SearchingForPlaces)
            {
                if (!await Dialogs.ConfirmAsync(GuiMessages.NotInModeToAdd(modul))) return;
            }
            
            if (!await Dialogs.ConfirmAsync($"Do you want to save {args.Name} as a {GuiMessages.Placeholder(modul)}?")) return;
            var p = CountryPlacesAdd(PinDesignFactory.Unvisited, PlusCodeHelper.ToPlusCode(args.Latitude, args.Longitude));
            p.Name = args.Name;
            GetAndShowPin(p.Name, PinDesignFactory.Unvisited, p.PlusCode);
            GoTo(p.Position);
            await SavePlace(CountryCode, p);
            await Repository.SavePlaceName(p);
        }, false);

        public ICommand PositionChangedCommand => new SafeCommand<PinDragEventArgs>(async args =>
        {
            if (SelectedPin == null) return;
            if (!await Dialogs.ConfirmAsync("Do you really want to move the pin here?")) return;

            var newpc = PlusCodeHelper.ToPlusCode(args.Pin.Position);
        }, false);

        public ICommand SavePlaceCommand => new SafeCommand(OnSavePlace);

        protected abstract Task OnSavePlace();

        public Command SearchPlacesCommand => new SafeCommand(() =>
        {
            if (Country == null)
            {
                _actionState = ActionStates.ChoosingCountry;
                _nextActionState = ActionStates.SearchingForPlaces;
            }
            else
            {
                _actionState = ActionStates.SearchingForPlaces;
            }
            StateChanged();
        });

        public Command SearchStateCommand => new SafeCommand(() =>
        {
            _nextActionState = _actionState;
            _actionState = ActionStates.ChoosingCountry;
            StateChanged();
        });

        

        public ICommand SyncCommand => new SafeCommand(async () =>
        {
            // TODO: implement synchronization logic if needed
        });

        public ICommand UnmarkPlaceCommand => new SafeCommand(() =>
        {
            if (SelectedPlace == null) return;
            ChangePinType(SelectedPlace, PinDesignFactory.Unvisited);
        });

        public Command InitializeCommand => new SafeCommand(() =>
        {
            if (_actionState == ActionStates.VisitingPlaces || _actionState == ActionStates.PlanningVisits) HighlightTeamPlan(false);
            _actionState = ActionStates.ChoosingAction;
            SelectedPlace = null;
            StateChanged();
        });

        public Command PlanStateCommand => new SafeCommand(() =>
        {
            _actionState = ActionStates.PlanningVisits;
            StateChanged();
            HighlightTeamPlan(true);
        });

        public CountryPlaces<TPoint> CountryPlaces { get; set; } = null!;

        protected void HighlightTeamPlan(bool highlight)
        {
            if (CountryPlaces == null) return;
            foreach (var place in CountryPlaces.Places.Where(p => p.IsPlanned))
            {
                _pinsIndex.TryGetValue(place.PlusCode, out var pin);
                if (pin != null)
                {
                    if (highlight)
                    {
                        pin.Transparency = (place.Type == PlaceStates.PlacePlanned && place.TeamChar == SelectedTeamChar) ? 0f : 0.5f;
                    }
                    else
                    {
                        pin.Transparency = 0f;
                    }
                }
            }
        }

        protected override void RefreshRoles()
        {
            base.RefreshRoles();
            Notify(nameof(CanAddPlaces));
            Notify(nameof(CanPlanVisit));
            Notify(nameof(CanVisitPlace));
        }

        protected async void Clipboard_ClipboardContentChanged(object? sender, EventArgs? e)
        {
            if (!Clipboard.HasText)
            {
                CanImport = false;
            }
            else
            {
                ClipboardText = await Clipboard.GetTextAsync();
                CanImport = ClipboardText?.Contains("|")??false;
            }
        }
        protected void OnMessageTimedEvent(object? sender, ElapsedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() => HasMessage = false);
        }
        protected void PlacesChanged(object s, BasePoint[] places)
        {
            foreach (var p in places)
            {
                UpdatePin(p);
            }
        }

        

        protected void UpdatePin(BasePoint place)
        {
            var design = PinDesignFactory.Get(place);
            var pin = GetAndShowPin(place.Name ?? PlacePoint.PlaceNamePlaceholder, design, place.PlusCode);
            if (pin == null) return;
            SetupPin(place, design, pin);
        }

        

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            var pars = Param.DecodeValues(query);
            var plusCode = Param.GetOrDefault<string>(pars, nameof(SearchViewModel.PlusCode));
            if (PlusCodeHelper.IsValid(plusCode))
            {
                var placeName = Param.GetOrDefault<string>(pars, nameof(SearchViewModel.PlaceName));
                OnQueryPlaceUpdate(plusCode, placeName);

                TaskHelper.SafeRun(JumpToPlusCode(plusCode));
            }
        }

        protected abstract void OnQueryPlaceUpdate(string plusCode, string placeName);

        protected abstract Task DoSearch(bool shortSearch);

        protected Task DoSearch()
        {
            return DoSearch(false);
        }

        protected abstract void ChangePinType(TPoint place, PinDesign design);

        protected Pin? GetAndShowPin(string name, PinDesign design, string plusCode)
        {
            _pinsIndex.TryGetValue(plusCode, out var pin);
            if (pin == null && !design.IsHidden)
            {
                pin = new Pin
                {
                    Label = name?? PlacePoint.PlaceNamePlaceholder,
                    IsDraggable = design.IsNew,
                    Transparency = design.Transparency,
                    Position = PlusCodeHelper.ToPosition(plusCode),
                    Icon = design.GetIcon()
                };
                _pinsIndex.Add(plusCode, pin);
                Pins.Add(pin);
            }
            return pin;
        }

        protected void SetupPin(BasePoint place, PinDesign design, Pin pin)
        {
            if (design.IsHidden)
            {
                Pins.Remove(pin);
                _pinsIndex.Remove(place.PlusCode);
            }
            else
            {
                pin.Transparency = design.Transparency;
                pin.Icon = design.GetIcon();
                pin.IsDraggable = design.IsNew;
                pin.Label = place.GetPinName();
            }
        }

        protected async Task JumpToPlusCode(string plusCode)
        {
            if (MapControl == null) return;
            var position = PlusCodeHelper.ToPosition(plusCode);
            await MapControl.AnimateCamera(CameraUpdateFactory.NewPositionZoom(position, 18));
        }

        protected async Task MoveCameraToPlusCode(string plusCode)
        {
            var position = PlusCodeHelper.ToPosition(plusCode);
            await MoveCameraToPosition(position);
        }

        protected async Task MoveCameraToPosition(Position newPos)
        {
            if (MapControl == null) return;
            var speed = 2.0;
            if (_lastSelected != null && SelectedPlace != null)
            {
                var bounds = Bounds.FromPositions(new[] {_lastSelected.Position, newPos});
                var zoomout = CameraUpdateFactory.NewBounds(bounds, 100);
                var n = MapSpan.FromBounds(bounds);
                speed = Math.Log(SelectedPlace.DistanceTo(_lastSelected) + Math.E);
                speed = Math.Max(speed, 0.5);
                speed = Math.Min(speed, 1.5);
                if (n.Radius.Meters > VisibleRegion.Radius.Meters * 1.1)
                {
                    await MapControl.AnimateCamera(zoomout, TimeSpan.FromSeconds(speed));
                }
            }

            await MapControl.AnimateCamera(CameraUpdateFactory.NewPositionZoom(newPos, 18), TimeSpan.FromSeconds(speed));
            //await ZoomTo(SelectedPlace.Position, 18);
        }

        protected async Task<Location> GetCurrentPosition()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                return location;
            }
            //catch (FeatureNotSupportedException fnsEx)
            //{
                
            //}
            //catch (FeatureNotEnabledException fneEx)
            //{
            //    // Handle not enabled on device exception
            //}
            //catch (PermissionException pEx)
            //{
            //    // Handle permission exception
            //}
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool GoBack()
        {
            if (_actionState == ActionStates.ChoosingAction)
            {
                if (SelectedPlace != null)
                {
                    SelectedPlace = null;
                }
                else return false;
            }
            if (IsPlaceDetailVisible)
            {
                IsPlaceDetailVisible = false;
            }
            else
            {
                _actionState = ActionStates.ChoosingAction;
                StateChanged();
            }
            return true;
        }

        protected void GoTo(Position position)
        {
            CameraUpdate = CameraUpdateFactory.NewPosition(position);
        }
        
        protected async Task ChangeCountry()
        {
            if (Country == null) return;
            ShowCountryBorders();
            await ShowCountryPins();
            CountrySearch = "";
            await OnCountryChanged();
        }

        protected virtual Task OnCountryChanged()
        {
            return Task.CompletedTask;
        }

        public async Task NavigateToCurrentPosition()
        {
            var c = await GetCurrentPosition();
            if (c != null && MapControl != null)
            {
                MapControl.MoveToRegion(Maui.GoogleMaps.MapSpan.FromCenterAndRadius(new Maui.GoogleMaps.Position(c.Latitude, c.Longitude), Maui.GoogleMaps.Distance.FromMiles(20)));
            }
        }
        
        protected void StateChanged()
        {
            Notify(nameof(IsCountriesVisible));
            Notify(nameof(IsSearchVisible));
            Notify(nameof(IsActionsVisible));
            Notify(nameof(IsPlanningVisible));
            Notify(nameof(IsVisitingVisible));
        }

        protected async Task ZoomTo(Position position, double maxZoomLevel = 18)
        {
            if (MapControl == null) return;
            await MapControl.AnimateCamera(CameraUpdateFactory.NewPositionZoom(position, maxZoomLevel), TimeSpan.FromSeconds(2));
        }

        protected async void ShowCountryBorders()
        {
            Borders.Clear();
            var polygons = await CountryBorderHelper.GetCountryBorders(Country);
            if (polygons == null)
            {
                Position? position = await CountryBorderHelper.GetCountryPosition(Country);
                if (position == null) return;
                await TaskHelper.SafeRun(ZoomTo(position ?? new Position(), 6));
                return;
            }
            foreach (var mapPolygon in polygons.SelectMany(pg => pg.ToMapPolygons()))
            {
                // Convert Microsoft.Maui.Controls.Maps.Polygon to Maui.GoogleMaps.Polygon
                var gPolygon = new Polygon();
                foreach (var pos in mapPolygon.Geopath)
                {
                    gPolygon.Positions.Add(new Position(pos.Latitude, pos.Longitude));
                }
                // Copy basic styling if available
                gPolygon.StrokeColor = mapPolygon.StrokeColor;
                gPolygon.FillColor = mapPolygon.FillColor;
                gPolygon.StrokeWidth = (float)mapPolygon.StrokeWidth;
                Borders.Add(gPolygon);
            }
            var bounds = await CountryBorderHelper.GetCountryBounds(Country);
            CameraUpdate = CameraUpdateFactory.NewBounds(bounds, 0);
        }

        protected abstract Task ShowCountryPins();

        public Command VisitStateCommand => new SafeCommand(() =>
        {
            _actionState = ActionStates.VisitingPlaces;
            StateChanged();
            HighlightTeamPlan(true);
        });
        protected async Task<bool> ConfirmDelete()
        {
            return !await Dialogs.ConfirmAsync($"Do you really want to delete visited {GuiMessages.Placeholder(modul)}? You cannot undo this action!");
        }

        protected abstract Task DoNextPlace();
        
        protected async Task SavePlaceTypeInSearch(PinDesign design)
        {
            _lastSelected = SelectedPlace;
            if (_lastSelected == null) return;
            ChangePinType(_lastSelected, design);
            _lastSelected.Country = CountryCode;
            await SavePlace(CountryCode, _lastSelected);
            await Repository.SavePlaceName(_lastSelected);
            SelectedPlace = null;
            await DoNextPlace();
        }

        protected abstract Task SavePlace(string countryCode, TPoint p);

        

        protected async Task AfterPinSelected(TPoint value, Pin? pin)
        {
            if (value == null) return;
            if (CameraPosition.Zoom < MinSelZoom)
            {
                _ = TaskHelper.SafeRun(async () => await ZoomTo(value.Position, MinSelZoom));
            }

            if (pin != null && pin.Label == PlacePoint.PlaceNamePlaceholder && !pin.Equals(_lastSelectedPin))
            {
                if (Repository != null && SelectedPlace != null)
                {
                    await Repository.LoadPlaceName(SelectedPlace);
                    pin.Label = SelectedPlace.GetPinName();
                    _lastSelectedPin = pin;
                }
            }

            await LoadPlaceInfo();
        }

        protected abstract Task LoadPlaceInfo();
    }

    public class Team
    {
        public Team(char c, string name)
        {
            Char = c;
            Name = name;
        }

        public char Char { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public static class GuiMessages
    {
        public static string PlaceholderPlural(Modul modul) => modul switch
        {
            Modul.Schools => "schools",
            _ => "places"
        };

        public static string Placeholder(Modul modul) => modul switch
        {
            Modul.Schools => "school",
            _ => "place"
        };

        public static string NotHasAddPermission(Modul modul)
            => $"You don't have permission to add {PlaceholderPlural(modul)}.";

        public static string ChooseCountryFirstThenAdd(Modul modul)
            => $"You have to choose country first and then add {PlaceholderPlural(modul)}.";

        public static string NotInModeToAdd(Modul modul)
            => $"You are not in the Find {PlaceholderPlural(modul)} mode now. Do you really want to add this {Placeholder(modul)}?";

        public static string SearchAgain(Modul modul)
            => $"Search again to find more {PlaceholderPlural(modul)}";

        public static string NothingNewFound(Modul modul)
            => $"No new {Placeholder(modul)} found!";
    }
}
