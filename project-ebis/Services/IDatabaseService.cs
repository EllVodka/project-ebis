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
        Task<ObservableCollection<Operation>> GetJournalOperation(MySqlConnection connection, int idBorne);
        ObservableCollection<Entretien> GetJournalEntretien(MySqlConnection connection);
        Task<ObservableCollection<ElementVerif>> GetElementVerif(MySqlConnection connection, int idEntretien);
    }

}
