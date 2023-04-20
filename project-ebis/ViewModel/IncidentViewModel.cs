using CommunityToolkit.Mvvm.ComponentModel;
using project_ebis.Model;

namespace project_ebis.ViewModel;

[QueryProperty("Incident", "Incident")]
public partial class IncidentViewModel : BaseViewModel
{
    [ObservableProperty]
    JournalIncident incident;


}
