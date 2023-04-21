using MySqlConnector;
using project_ebis.Model;
using System.Collections.ObjectModel;

namespace project_ebis.Services
{
    public interface IDatabaseService
    {
        MySqlConnection CreateConnection();
        ObservableCollection<string> ExecuteSelectQuery(string selectQuery, MySqlConnection connection);
        ObservableCollection<Borne> ExecuteSelectQueryForBorne(MySqlConnection connection);
        ObservableCollection<JournalIncident> ExecuteSelectQueryForJournauxIncidents(MySqlConnection connection);
        Task<ObservableCollection<Operation>> GetJournalOperation(MySqlConnection connection, int idBorne);
        ObservableCollection<Entretien> GetJournalEntretien(MySqlConnection connection);
        Task<ObservableCollection<ElementVerif>> GetElementVerif(MySqlConnection connection, int idEntretien);
        ObservableCollection<ElementFiable> GetElementFiables(MySqlConnection connection);
        ObservableCollection<ElementDefecteux> GetElementDefecteux(MySqlConnection connection);
        ObservableCollection<IncidentMois> GetMoyenneIncident5Ans(MySqlConnection connection);
        ObservableCollection<FonctionnementMoyen> GetFonctionnementMoyenElement(MySqlConnection connection);
    }

}
