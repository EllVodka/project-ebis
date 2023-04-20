using project_ebis.ViewModel;

namespace project_ebis.View;

public partial class JournauxEntretienPage : ContentPage
{
	public JournauxEntretienPage(JournauxEntretiensViewModel viewModel)
	{
		InitializeComponent();
		viewModel.GetAllJournalEntretien();
		BindingContext = viewModel;
	}
}