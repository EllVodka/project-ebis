using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class OperationPage : ContentPage
{
	public OperationPage(OperationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;	
	}
}