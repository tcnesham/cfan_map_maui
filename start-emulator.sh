#!/bin/bash

# Script to start Pixel 9 emulator for .NET MAUI debugging

echo "Starting Pixel 9 Emulator with increased storage..."

# Start Pixel 9 emulator with additional storage options
emulator -avd Pixel_9 \
  -partition-size 2048 \
  -memory 2048 \
  -netdelay none \
  -netspeed full \
  -wipe-data &

echo "Waiting for emulator to fully boot..."
adb wait-for-device

echo "Checking connected devices..."
adb devices

echo ""
echo "Pixel 9 Emulator is ready for debugging!"
echo "In VS Code: Select 'Debug Pixel 9 Emulator' and press F5"
echo ""
echo "If you still get storage errors, try:"
echo "1. adb shell pm uninstall com.companyname.mauiapp1"
echo "2. Or wipe emulator data from Android Studio AVD Manager"
