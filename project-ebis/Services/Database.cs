using Microsoft.Maui.Controls;
using MySqlConnector;
using project_ebis.Model;
using System.Collections.ObjectModel;
using System.Data;
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
                    "tc.libelletypecharge       AS TypeCharge "+
                    "FROM secteur s "+
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

        public async Task<ObservableCollection<Operation>> GetJournalOperation(MySqlConnection connection, int idBorne)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT "+
                    "oc.dateheuredebut   AS DateDebut,"+
                    "oc.numoperation     AS IdOperation "+
                    "FROM operationrechargement oc "+
                    "INNER JOIN borne b ON oc.idborne = b.id "+
                    "WHERE b.id = @id",connection);
                command.Parameters.AddWithValue("@id", idBorne);

                MySqlDataReader reader = await command.ExecuteReaderAsync();
                var results = new ObservableCollection<Operation>();

                while (reader.Read())
                {
                    var operation = new Operation();
                    operation.DateDebut = (DateTime)reader["DateDebut"];
                    operation.IdOperation = (int)reader["IdOperation"];
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
