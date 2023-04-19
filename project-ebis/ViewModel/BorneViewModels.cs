using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using project_ebis.Model;


namespace project_ebis.ViewModel
{
    [QueryProperty("Borne", "Borne")]
    public partial class BorneViewModels : BaseViewModel
    {
        [ObservableProperty]
        Borne borne;
    }
}
