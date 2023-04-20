using project_ebis.View;

namespace project_ebis;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("BornePage", typeof(BornePage));
		Routing.RegisterRoute("JournauxIncidentPage", typeof(JournauxIncidentPage));
		Routing.RegisterRoute("IncidentPage", typeof(IncidentPage));
    }
}
