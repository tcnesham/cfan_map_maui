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