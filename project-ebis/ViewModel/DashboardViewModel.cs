using CommunityToolkit.Mvvm.Input;
using MySqlConnector;
using project_ebis.Model;
using project_ebis.Services;
using project_ebis.View;
using System.Collections.ObjectModel;

namespace project_ebis.ViewModel;

public partial class DashboardViewModel : BaseViewModel
{
    public ObservableCollection<Borne> Bornes { get; set; } = new();

    DatabaseService databaseService { get; set; }
    MySqlConnection conn { get; set; }


    public DashboardViewModel()
    {
        this.databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");
        this.conn = this.databaseService.CreateConnection();
    }

    public void GetAllBorne()
    {            
        Bornes = this.databaseService.ExecuteSelectQueryForBorne(conn);
        conn.Close();
    }

    [RelayCommand]
    async Task GoToBorne(Borne borne)
    {
        await Shell.Current.GoToAsync(
            "BornePage",
            true,
            new Dictionary<string, object>
            {
                {
                "Borne", borne
                }
            });
    }

    [RelayCommand]
    async Task GoToEntretien()
    {
        await Shell.Current.GoToAsync(nameof(JournauxEntretienPage), true);
    }
}
