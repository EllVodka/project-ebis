using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class BornePage : ContentPage
{
	public BornePage(BorneViewModels viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}