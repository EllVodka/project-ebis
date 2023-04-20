using project_ebis.Model;
using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class JournauxIncidentPage : ContentPage
{
	public JournauxIncidentPage(JournauxIncidentViewModel viewModel)
	{
		InitializeComponent();
		viewModel.GetAllJounauxIncidents();
		BindingContext = viewModel;
	}
}