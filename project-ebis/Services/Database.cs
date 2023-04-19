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
                    borne.IdBorne = (int)reader[2];
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

        public Borne GetBorne(MySqlConnection connection,int idBorne)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT b.id AS IdBorne,"+
                    "b.datemiseenservice     AS DateMiseEnService," +
                    "b.datederniererevision  AS DateDerniereRevision,"+
                    "tc.libelletypecharge    AS TypeCharge"+
                    "FROM borne b"+
                    "INNER JOIN typecharge tc ON b.codetypecharge = tc.codetypecharge"+
                    "WHERE b.id = "+idBorne,connection);
                //command.Parameters.AddWithValue("@id", idBorne);

                MySqlDataReader reader = command.ExecuteReader();
                Borne results = new Borne();           

                while (reader.Read())
                {
                    results.IdBorne = (int)reader[0];
                    results.DateMiseEnService = (string)reader[1];
                    results.DerniereMaintenance = (string)reader[2];
                    results.TypeCharge = (string)reader[3];                    
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
