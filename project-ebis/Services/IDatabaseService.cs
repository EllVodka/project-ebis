using MySqlConnector;
using System.Collections.ObjectModel;

namespace project_ebis.Services
{
    public interface IDatabaseService
    {
        MySqlConnection CreateConnection();
        ObservableCollection<string> ExecuteSelectQuery(string selectQuery, MySqlConnection connection);
    }

}
