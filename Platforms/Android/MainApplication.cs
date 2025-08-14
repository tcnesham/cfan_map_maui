using Android.App;
using Android.Runtime;
using AndroidX.AppCompat.App;

namespace CFAN.SchoolMap.Maui;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	public override void OnCreate()
	{
		base.OnCreate();
		Acr.UserDialogs.UserDialogs.Init(this);
		
		// Initialize Firebase
		try
		{
			Firebase.FirebaseApp.InitializeApp(this);
		}
		catch (System.Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Firebase initialization error: {ex.Message}");
		}
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
