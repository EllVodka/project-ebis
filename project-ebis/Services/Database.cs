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

                MySqlCommand command = new MySqlCommand("SELECT "+
                    "s.libelle                  AS NomSecteur,"+
                    "st.adresseville            AS NomStation,"+
                    "b.id                       AS IdBorne,"+
                    "b.datemiseenservice        AS DateMiseEnService,"+
                    "b.datederniererevision     AS DerniereMaintenance," +
                    "tc.libelletypecharge       AS TypeCharge, "+
                    "st.latitude                AS Latitude,"+
                    "st.longitude               AS Longitude "+
                    "FROM secteur s " +
                    "INNER JOIN station st ON s.id = st.idsecteur "+
                    "INNER JOIN borne b ON b.idstation = st.id "+
                    "INNER JOIN typecharge tc ON b.codetypecharge = tc.codetypecharge;",connection);

                MySqlDataReader reader = command.ExecuteReader();
                ObservableCollection<Borne> results = new ObservableCollection<Borne>();
            

                while (reader.Read())
                {
                var borne = new Borne(); 
                    borne.NomSecteur = (string)reader["NomSecteur"];
                    borne.NomStation = (string)reader["NomStation"];
                    borne.IdBorne = (int)reader["IdBorne"];
                    borne.DateMiseEnService = (DateTime)reader["DateMiseEnService"];
                    borne.DerniereMaintenance = (DateTime)reader["DerniereMaintenance"];
                    borne.TypeCharge = (string)reader["TypeCharge"];
                    borne.Latitude = (float)reader["Latitude"];
                    borne.Longitude = (float)reader["Longitude"];
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

                MySqlCommand command = new MySqlCommand("SELECT ti.libelle as TypeIncident, i.detail as Detail, CONCAT(i.mois,'/',i.jour,'/',i.annee,' ',i.heures,':00') as DateIncident, i.idborne as IdBorne,i.id as IdIncident FROM incident i JOIN typeincident ti ON i.idtypeincident = ti.id ORDER BY i.annee DESC,i.mois DESC,i.jour DESC,i.heures DESC;", connection);

                MySqlDataReader reader = command.ExecuteReader();
                ObservableCollection<JournalIncident> results = new();


                while (reader.Read())
                {
                    var journalIncident = new JournalIncident
                    {
                        TypeIncident = (string)reader["TypeIncident"],
                        DetailIncident = (string)reader["Detail"],
                        DateIncident = (string)reader["DateIncident"],
                        IdBorne = (int)reader["IdBorne"]
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

        public async Task<ObservableCollection<Operation>> GetJournalOperation(MySqlConnection connection, int idBorne)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }


                MySqlCommand command = new MySqlCommand("SELECT "+
                    "oc.dateheuredebut      AS DateDebut,"+
                    "oc.dateheurefin         AS DateFin,"+
                    "oc.numoperation        AS IdOperation,"+
                    "tc.libelletypecharge    AS TypeCharge,"+
                    "oc.nbkwheures           AS KwHConsommer, "+
                    "b.id as IdBorne " +
                    "FROM operationrechargement oc "+
                    "INNER JOIN borne b ON oc.idborne = b.id "+
                    "INNER JOIN typecharge tc ON tc.codetypecharge = b.codetypecharge "+
                    "WHERE b.id = @id; ",connection);
                command.Parameters.AddWithValue("@id", idBorne);

                MySqlDataReader reader = await command.ExecuteReaderAsync();
                var results = new ObservableCollection<Operation>();

                while (reader.Read())
                {
                    var operation = new Operation();
                    operation.DateDebut = (DateTime)reader["DateDebut"];
                    operation.DateFin = (DateTime)reader["DateFin"];
                    operation.IdOperation = (int)reader["IdOperation"];
                    operation.TypeCharge = (string)reader["TypeCharge"];
                    operation.KwHConsomme = (int)reader["KwHConsommer"];
                    operation.IdBorne = (int)reader["IdBorne"];
                    results.Add(operation);

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
