using CFAN.SchoolMap.Helpers;

namespace CFAN.SchoolMap.Maui;

public partial class App : Application
{
	public const string Key = "AIzaSyAZqhVZWwYQBkCCti5K4v0EtSLQjfdNGOY";
	private readonly IServiceProvider _serviceProvider;
	
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		_serviceProvider = serviceProvider;
		ServiceHelper.Services = serviceProvider;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Ensure ServiceHelper is set before creating AppShell
		ServiceHelper.Services = _serviceProvider;
		return new Window(new AppShell());
	}
}