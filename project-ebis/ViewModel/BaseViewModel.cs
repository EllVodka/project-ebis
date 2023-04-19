using CommunityToolkit.Mvvm.ComponentModel;

namespace project_ebis.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string titre;

        [ObservableProperty]
        [NotifyPropertyChangedFor("NEstPasOccupe")]
       private bool estOccupe;

        public bool NEstPasOccupe => !EstOccupe;

    }
}
