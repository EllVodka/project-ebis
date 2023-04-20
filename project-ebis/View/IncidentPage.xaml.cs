using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class IncidentPage : ContentPage
{
	public IncidentPage(IncidentViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}