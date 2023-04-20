using MySqlConnector;
using project_ebis.Model;
using project_ebis.Services;
using System.Collections.ObjectModel;

namespace project_ebis.ViewModel;

public class JournauxEntretiensViewModel
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
}
