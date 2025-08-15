# Actions taken — 2025-08-15

- Map types and handler
  - Kept CFAN map types across the app and ensured the custom map control is backed by the platform handler.
  - Confirmed handler mapping in `MauiProgram.cs`: maps `CFAN.SchoolMap.Maui.GoogleMaps.Map` to `Maui.GoogleMaps.Handlers.MapHandler` via `ConfigureMauiHandlers` and enabled `UseGoogleMaps()`.

- School Map page alignment
  - `Views/SchoolMapPage.xaml` uses `xmlns:googleMaps="clr-namespace:CFAN.SchoolMap.Maui.GoogleMaps"` and CFAN binding behaviors.
  - `Views/SchoolMapPage.xaml.cs` aligns to CFAN namespace, configures UI settings/MapStyle, and sets `VM.MapControl = map`.

- MapBaseVM fixes
  - Reverted and normalized to CFAN map types (Pin, Position, Polygon, MapSpan, Distance, CameraPosition/Update, Bounds) and CFAN `Map` for `MapControl`.
  - Event/override alignment: used `new` for `Initialization` and `ApplyQueryAttributes(IDictionary<string,string>)`; adjusted `PlacesChanged(object? s, BasePoint[] places)` to match nullability.
  - Eliminated overloaded-operator null-compare issues on `Pin` by using `is null`/`is not null` and explicit guards.
  - Nullability hardening: made `SelectedPin`, `_lastSelectedPin`, `SelectedPlace`, and `Country` nullable; added `SetRefProperty` helper for nullable ref-type properties.
  - Guarded `CountryCode` and clipboard before repository calls (import/save); avoided assigning null to `BasePoint.Country`.
  - Guarded borders/bounds before camera updates; converted fire-and-forget calls to `_ = TaskHelper.SafeRun(...)`.

- Build validation
  - Built for Android (`net9.0-android`): success with warnings; no blocking compile errors remain.
