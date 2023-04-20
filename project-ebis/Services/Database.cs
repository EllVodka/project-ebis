using MySqlConnector;
using project_ebis.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using Debug = System.Diagnostics.Debug;

namespace project_ebis.Services
{
    public class DatabaseService : IDatabaseService
    {
        private string connectionString;
        private MySqlConnection connection;

        public DatabaseService(string host, string database,int port, string user, string password)
        {
            this.connectionString = $"server={host};user={user};database={database};port={port};password={password};";
        }   

        public MySqlConnection CreateConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
            }

            return connection;
        }

        public ObservableCollection<string> ExecuteSelectQuery(string selectQuery, MySqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                MySqlDataReader reader = command.ExecuteReader();
                ObservableCollection<string> results = new ObservableCollection<string>();

                while (reader.Read())
                {
                    results.Add(reader.GetString(0));
                }

                return results;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public ObservableCollection<Borne> ExecuteSelectQueryForBorne(MySqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT s.libelle AS NomSecteur," +
                "st.adresseville AS NomStation," +
                "b.id AS IdBorne " +
                "FROM secteur s JOIN station st ON s.id = st.idsecteur " +
                "INNER JOIN borne b ON b.idstation = st.id;",connection);

                MySqlDataReader reader = command.ExecuteReader();
                ObservableCollection<Borne> results = new ObservableCollection<Borne>();
            

                while (reader.Read())
                {
                var borne = new Borne(); 
                    borne.NomSecteur = (string)reader[0];
                    borne.NomStation = (string)reader[1];
                    borne.idBorne = (int)reader[2];
                    results.Add(borne);
                    
                }


                return results;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public ObservableCollection<JournalIncident> ExecuteSelectQueryForJournauxIncidents(MySqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT ti.libelle as TypeIncident, i.detail tas Detail, CONCAT(i.mois,'/',i.jour,'/',i.annee,' ',i.heures,':00') as DateIncident, i.idborne as IdBorne,i.id as IdIncident FROM incident i JOIN typeincident ti ON i.idtypeincident = ti.id ORDER BY i.annee DESC,i.mois DESC,i.jour DESC,i.heures DESC;", connection);

                MySqlDataReader reader = command.ExecuteReader();
                ObservableCollection<JournalIncident> results = new();


                while (reader.Read())
                {
                    var journalIncident = new JournalIncident
                    {
                        TypeIncident = (string)reader[0],
                        DetailIncident = (DateTime)reader[1],
                        DateIncident = (DateTime)reader[2],
                        IdBorne = (int)reader[3]
                    };
                    results.Add(journalIncident);

                }

                return results;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }

}
