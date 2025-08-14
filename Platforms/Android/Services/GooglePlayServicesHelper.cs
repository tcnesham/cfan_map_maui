using Android.Content;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Android.Gms.Common;

namespace CFAN.SchoolMap.Maui.Platforms.Android.Services
{
    public static class GooglePlayServicesHelper
    {
        public static bool IsGooglePlayServicesAvailable(Context context)
        {
            try
            {
                var googleAPI = GoogleApiAvailability.Instance;
                var result = googleAPI.IsGooglePlayServicesAvailable(context);
                
                if (result == ConnectionResult.Success)
                {
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Google Play Services not available: {result}");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking Google Play Services: {ex.Message}");
                return false;
            }
        }
        
        public static void HandleGooglePlayServicesError(AndroidX.AppCompat.App.AppCompatActivity activity, int resultCode)
        {
            try
            {
                var googleAPI = GoogleApiAvailability.Instance;
                
                if (googleAPI.IsUserResolvableError(resultCode))
                {
                    var dialog = googleAPI.GetErrorDialog(activity, resultCode, 1000);
                    dialog?.Show();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Google Play Services error cannot be resolved by user");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error handling Google Play Services error: {ex.Message}");
            }
        }
    }
}
