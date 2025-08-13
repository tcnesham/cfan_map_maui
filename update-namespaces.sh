#!/bin/bash

# Script to update Xamarin.Forms namespaces to CFAN.SchoolMap.Maui namespaces

echo "Updating namespaces from Xamarin.Forms to CFAN.SchoolMap.Maui..."

# Update all namespace declarations in Map directory files
find Map -name "*.cs" -type f -exec sed -i '' 's/namespace Xamarin\.Forms\.GoogleMaps\.Bindings/namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/namespace Xamarin\.Forms\.GoogleMaps\.Logics/namespace CFAN.SchoolMap.Maui.GoogleMaps.Logics/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/namespace Xamarin\.Forms\.GoogleMaps\.Internals/namespace CFAN.SchoolMap.Maui.GoogleMaps.Internals/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/namespace Xamarin\.Forms\.GoogleMaps\.Extensions/namespace CFAN.SchoolMap.Maui.GoogleMaps.Extensions/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/namespace Xamarin\.Forms\.GoogleMaps\.Helpers/namespace CFAN.SchoolMap.Maui.GoogleMaps.Helpers/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/namespace Xamarin\.Forms\.GoogleMaps/namespace CFAN.SchoolMap.Maui.GoogleMaps/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/namespace Xamarin\.Forms/namespace CFAN.SchoolMap.Maui/g' {} \;

# Update using statements as well
find Map -name "*.cs" -type f -exec sed -i '' 's/using Xamarin\.Forms\.GoogleMaps\.Bindings/using CFAN.SchoolMap.Maui.GoogleMaps.Bindings/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/using Xamarin\.Forms\.GoogleMaps\.Logics/using CFAN.SchoolMap.Maui.GoogleMaps.Logics/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/using Xamarin\.Forms\.GoogleMaps\.Internals/using CFAN.SchoolMap.Maui.GoogleMaps.Internals/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/using Xamarin\.Forms\.GoogleMaps\.Extensions/using CFAN.SchoolMap.Maui.GoogleMaps.Extensions/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/using Xamarin\.Forms\.GoogleMaps\.Helpers/using CFAN.SchoolMap.Maui.GoogleMaps.Helpers/g' {} \;
find Map -name "*.cs" -type f -exec sed -i '' 's/using Xamarin\.Forms\.GoogleMaps/using CFAN.SchoolMap.Maui.GoogleMaps/g' {} \;

echo "Namespace update completed!"
echo "Files updated: $(find Map -name "*.cs" | wc -l)"
