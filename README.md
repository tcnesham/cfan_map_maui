# cfan_map_maui

#### Install Notes

#### Create a new Maui app -> dotnet new maui -n CFAN.SchoolMap.Maui

#### dotnet add package Microsoft.Maui.Controls.Maps
#### dotnet add package Google.Cloud.Firestore
#### dotnet add package Plugin.CloudFirestore
#### dotnet add package ISO3166
#### dotnet add package Acr.UserDialogs.Maui --version 9.2.2

#### Executing task: dotnet add '/Users/timnesham/CascadeProjects/CFAN.SchoolMap.Maui/CFAN.SchoolMap.Maui.csproj' package Onion.Maui.GoogleMaps -v #### 6.2.0 -s https://api.nuget.org/v3/index.json --interactive 
#### Using rt-click the csproj -> NuGet package manager and install Onion.Maui.GoogleMaps
#### YouTube https://www.youtube.com/watch?v=775rzp2U82M

#### dotnet add package OpenLocationCode

#### GoogleApi.Entities or GoogleApi installed via NuGet package manager
#### dotnet add package Google.Cloud.BigQuery.V2
#### add the GeoJSON.Net NuGet package to your .NET MAUI project.

## Technical solutions
#### Can handle exceptions like this: 
#### InvalidOperationException e = new InvalidOperationException("No dimension is selected.");
#### ExceptionHandler.HandleException(e, false);


## There are multiple libraries that use Map control: Maui, Microsoft and Google all have libs in Maui workspace. 
### I decided to settle on this:
### using Pin = Maui.GoogleMaps.Pin;
### using PinType = Maui.GoogleMaps.PinType;
### using Position = Maui.GoogleMaps.Position;
### using Polygon = Maui.GoogleMaps.Polygon;
### using Location = Microsoft.Maui.Devices.Sensors.Location;

## The views compiled with these clr-namespaces
##### xmlns:googleMaps="clr-namespace:Maui.GoogleMaps;assembly=Maui.GoogleMaps"
##### xmlns:bindings="clr-namespace:CFAN.SchoolMap.Maui.GoogleMaps.Bindings"
##### xmlns:c="clr-namespace:ISO3166;assembly=ISO3166"
##### xmlns:wpf1="clr-namespace:CFAN.SchoolMap.WPF;assembly=CFAN.SchoolMap.Maui"
##### xmlns:controls="clr-namespace:CFAN.SchoolMap.Controls;assembly=CFAN.SchoolMap.Maui"


## Incorrect using spotted
### using CFAN.SchoolMap.Maui.GoogleMaps; should be
### using Maui.GoogleMaps;


## In SchoolMapPage.xaml removed the line for the reasons in the link
### Region="{Binding VisibleRegion}"
### https://stackoverflow.com/questions/78128712/maui-how-to-access-xaml-map-visibleregion-in-viewmodel

## --------------------------------------------------------------------------------------------------
## Summary: XC0022 Compiled Bindings Optimization Complete ✅
## --------------------------------------------------------------------------------------------------
### Key Changes Made:
### Added x:DataType specifications to major XAML files:

### SearchPage.xaml → x:DataType="viewModels:SearchViewModel"
### AdminPage.xaml → x:DataType="vm:AdminViewModel"
### AboutPage.xaml → x:DataType="vm:AboutViewModel"
### UserNameRolesPage.xaml → x:DataType="vm:UserNameRolesVM"
### RolesPage.xaml → x:DataType="vm:RolesVM"
### SchoolStatisticsPage.xaml → x:DataType="vm:SchoolStatisticsViewModel"
### MarketStatisticsPage.xaml → x:DataType="vm:MarketStatisticsViewModel"
### Optimized DataTemplate bindings with proper x:DataType:

### Search results: x:DataType="database:PlaceSearchResult"
### User lists: x:DataType="model:User"
### Statistics data: x:DataType="vm:StatValue"
### Resolved Picker control binding conflicts:

### Removed ItemDisplayBinding attributes that don't support compiled bindings
### Added ToString() method overrides to StatDateDim classes (StatDimDay, StatDimWeek, StatDimMonth, StatDimYear)
### Maintained proper display functionality while enabling compiled bindings
### Fixed namespace references:
### 
### Corrected assembly references from CFAN.SchoolMap to CFAN.SchoolMap.Maui
### Added proper xmlns namespace declarations
### ### Fixed corrupted XAML file:
### 
### Repaired MarketStatisticsPage.xaml which had duplicate content causing build errors
### Performance Benefits Achieved:
### Compile-time binding validation - binding errors caught during compilation instead of runtime
### Improved runtime performance - direct property access instead of reflection-based binding
### Better IntelliSense support - improved development experience with better autocomplete
### Type safety - compile-time type checking for binding expressions
### Build Status:
### ✅ Build Status: Successfully compiles with 0 errors
### ✅ XC0022 Warnings: Completely eliminated (0 remaining)
### ⚠️ Other Warnings: Some obsolete API warnings remain (Frame, FillAndExpand) but these are non-blocking
### Files Updated:
### 7 main XAML view files with complete compiled binding support
### 4 StatDateDim classes with proper ToString() implementations
### 1 corrupted XAML file repaired
### Your XAML bindings are now optimized for maximum performance with compiled bindings throughout the application! The warning count went ### from multiple XC0022 warnings to zero, significantly improving both build-time validation and runtime binding performance.


### 	modified:   MainPage.xaml
### 	modified:   ViewModels/StatDateDim.cs
### 	modified:   Views/AboutPage.xaml
### 	modified:   Views/AdminPage.xaml
### 	modified:   Views/MarketStatisticsPage.xaml
### 	modified:   Views/RolesPage.xaml
### 	modified:   Views/SchoolStatisticsPage.xaml
### 	modified:   Views/SearchPage.xaml
### 	modified:   Views/UserNameRolesPage.xaml
### 
### commit 6a983c3988860df5cbc18b70086dd7430a080388 (HEAD -> main)
### Author: tcnesham <tim.nesham@gmail.com>
### Date:   Tue Aug 12 20:45:11 2025 -0400
### 
###     XC0022 Compiled Bindings Optimization Complete
    	