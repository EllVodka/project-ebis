using project_ebis.Services;
using project_ebis.View;
using project_ebis.ViewModel;

namespace project_ebis;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});


        builder.Services.AddSingleton<MainPage>();		
		builder.Services.AddSingleton<DashboardViewModel>();
		builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<JournauxEntretiensViewModel>();
        builder.Services.AddSingleton<JournauxEntretienPage>();

        builder.Services.AddTransient<BorneViewModels>();
        builder.Services.AddTransient<BornePage>();
        builder.Services.AddTransient<OperationViewModel>();
        builder.Services.AddTransient<OperationPage>();
		builder.Services.AddTransient<EntretienViewModel>();
		builder.Services.AddTransient<EntretienPage>();

		

        return builder.Build();
	}
}
