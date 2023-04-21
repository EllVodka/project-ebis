using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class MainPage : ContentPage
{
	public MainPage(DashboardViewModel viewModel)
	{
        InitializeComponent();
		viewModel.GetAllBorne();
		viewModel.GetElementFiable();
		viewModel.GetElementDefecteux();
		viewModel.GetMoyenneIncident();
		viewModel.GetFonctionnementMoyen();
        BindingContext = viewModel;
	}
}