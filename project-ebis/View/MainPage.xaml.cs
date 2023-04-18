using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		var viewModel = new StatViewModel();
		viewModel.SelectToDatabase();
		this.BindingContext = viewModel;
	}
}