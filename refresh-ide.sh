#!/bin/bash

echo "🔄 Refreshing IDE and clearing caches..."

# Clean build artifacts
echo "Cleaning build artifacts..."
dotnet clean --configuration Debug
rm -rf bin/ obj/

# Restore packages
echo "Restoring packages..."
dotnet restore --force

# Build for iOS
echo "Building for iOS..."
dotnet build --framework net9.0-ios --configuration Debug

echo "✅ Done! Now restart VS Code and the CommunityToolkit errors should be gone."
echo ""
echo "To restart VS Code:"
echo "1. Close VS Code completely"
echo "2. Reopen the project folder"
echo "3. Wait for OmniSharp to load (check bottom status bar)"
echo ""
echo "Your debugging should now work with the updated launch configurations!"
