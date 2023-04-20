using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class EntretienPage : ContentPage
{
	public EntretienPage(EntretienViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}