using Microsoft.Extensions.Logging;
using CFAN.SchoolMap.Services.Auth;
using CFAN.SchoolMap.Maui.Services.Auth;
using CFAN.SchoolMap.Maui.Database;
using Microsoft.Maui.Controls.Maps;

namespace CFAN.SchoolMap.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiMaps()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register services
		builder.Services.AddSingleton<IAuth, DefaultAuthService>();
		builder.Services.AddSingleton<IRepository, Repository>();
		builder.Services.AddSingleton<App>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
