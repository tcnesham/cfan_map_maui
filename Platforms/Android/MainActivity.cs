using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;
using Android.Gms.Common;
using CFAN.SchoolMap.Maui.Platforms.Android.Services;

namespace CFAN.SchoolMap.Maui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Acr.UserDialogs.UserDialogs.Init(this);
    }
    
    protected override void OnResume()
    {
        base.OnResume();
        
        // Check Google Play Services when app resumes instead of during initial creation
        // This avoids interfering with Repository and other service initialization
        CheckGooglePlayServices();
    }
    
    private void CheckGooglePlayServices()
    {
        try
        {
            if (GooglePlayServicesHelper.IsGooglePlayServicesAvailable(this))
            {
                System.Diagnostics.Debug.WriteLine("Google Play Services is available and ready");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Google Play Services not available - app will work with limited functionality");
                
                // Get the specific error code for better handling
                var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
                GooglePlayServicesHelper.HandleGooglePlayServicesError(this, resultCode);
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error during Google Play Services check: {ex.Message}");
        }
    }
    
    protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        
        // Handle Google Play Services resolution result
        if (requestCode == 1000)
        {
            if (resultCode == Result.Ok)
            {
                System.Diagnostics.Debug.WriteLine("Google Play Services resolved successfully");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Google Play Services resolution failed or cancelled");
            }
        }
    }
}