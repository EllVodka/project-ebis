using CommunityToolkit.Mvvm.Input;
using MySqlConnector;
using project_ebis.Model;
using project_ebis.Services;
using project_ebis.View;
using System.Collections.ObjectModel;

namespace project_ebis.ViewModel;

public partial class JournauxEntretiensViewModel : BaseViewModel
{
    public ObservableCollection<Entretien> journalEntretien { get; set; } = new();

    DatabaseService databaseService { get; set; }
    MySqlConnection conn { get; set; }

    public JournauxEntretiensViewModel()
    {
        databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");
    }

    public void GetAllJournalEntretien()
    {
        conn = databaseService.CreateConnection();
        this.journalEntretien = databaseService.GetJournalEntretien(conn);
        conn.Close();
    }

    [RelayCommand]
    async Task GoToEntretien(Entretien entretien)
    {
        await Shell.Current.GoToAsync(
            nameof(EntretienPage), 
            true,
            new Dictionary<string, object>
        {
            {
                "Entretien",entretien
            }
        });
    }
}
