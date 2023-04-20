using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class MainPage : ContentPage
{
	public MainPage(DashboardViewModel viewModel)
	{
		InitializeComponent();
		viewModel.GetAllBorne();
		BindingContext = viewModel;
	}
}