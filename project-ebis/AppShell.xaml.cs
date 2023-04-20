﻿using project_ebis.View;

namespace project_ebis;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("BornePage", typeof(BornePage));
		    Routing.RegisterRoute("JournauxIncidentPage", typeof(JournauxIncidentPage));
		    Routing.RegisterRoute("IncidentPage", typeof(IncidentPage));
        Routing.RegisterRoute("OperationPage", typeof(OperationPage));
        Routing.RegisterRoute(nameof(JournauxEntretienPage), typeof(JournauxEntretienPage));
        Routing.RegisterRoute(nameof(EntretienPage), typeof(EntretienPage));
    }
}
