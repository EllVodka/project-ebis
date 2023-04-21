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

        public DatabaseService(string host, string database, int port, string user, string password)
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

                MySqlCommand command = new MySqlCommand("SELECT " +
                    "s.libelle                  AS NomSecteur," +
                    "st.adresseville            AS NomStation," +
                    "b.id                       AS IdBorne," +
                    "b.datemiseenservice        AS DateMiseEnService," +
                    "b.datederniererevision     AS DerniereMaintenance," +
                    "tc.libelletypecharge       AS TypeCharge, " +
                    "st.latitude                AS Latitude," +
                    "st.longitude               AS Longitude " +
                    "FROM secteur s " +
                    "INNER JOIN station st ON s.id = st.idsecteur " +
                    "INNER JOIN borne b ON b.idstation = st.id " +
                    "INNER JOIN typecharge tc ON b.codetypecharge = tc.codetypecharge;", connection);

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


                MySqlCommand command = new MySqlCommand("SELECT " +
                    "oc.dateheuredebut      AS DateDebut," +
                    "oc.dateheurefin         AS DateFin," +
                    "oc.numoperation        AS IdOperation," +
                    "tc.libelletypecharge    AS TypeCharge," +
                    "oc.nbkwheures           AS KwHConsommer, " +
                    "b.id as IdBorne " +
                    "FROM operationrechargement oc " +
                    "INNER JOIN borne b ON oc.idborne = b.id " +
                    "INNER JOIN typecharge tc ON tc.codetypecharge = b.codetypecharge " +
                    "WHERE b.id = @id; ", connection);
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

        public ObservableCollection<Entretien> GetJournalEntretien(MySqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT " +
                    "CONCAT(ent.jour, '/', ent.mois, '/', ent.annee, ' ', ent.heure, ':00')     AS DateEntretien," +
                    "st.id                                                                      AS IdStation," +
                    "b.id                                                                       AS IdBorne," +
                    "ent.id                                                                     AS IdEntretien," +
                    "t.prenom                                                                   AS PrenomTechnicien," +
                    "t.nom                                                                      AS NomTechnicien " +
                    "FROM entretien ent " +
                    "INNER JOIN technicien t ON ent.idtechnicien = t.id " +
                    "INNER JOIN secteur s ON s.id = t.idsecteur " +
                    "INNER JOIN station st ON st.idsecteur = s.id " +
                    "INNER JOIN borne b ON b.idstation = st.id " +
                    "ORDER BY ent.annee DESC, ent.mois DESC, ent.jour DESC, ent.heure DESC; ", connection);

                MySqlDataReader reader = command.ExecuteReader();
                var results = new ObservableCollection<Entretien>();

                while (reader.Read())
                {
                    var entretien = new Entretien();
                    entretien.DateEntretien = (string)reader["DateEntretien"];
                    entretien.IdStation = (int)reader["IdStation"];
                    entretien.IdEntretien = (int)reader["IdEntretien"];
                    entretien.IdBorne = (int)reader["IdBorne"];
                    entretien.PrenomTechnicien = (string)reader["PrenomTechnicien"];
                    entretien.NomTechnicien = (string)reader["NomTechnicien"];
                    results.Add(entretien);
                }

                return results;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<ObservableCollection<ElementVerif>> GetElementVerif(MySqlConnection connection, int idEntretien)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT e.libelle AS Libelle ,ISNULL(de.annee) AS annee " +
                    "FROM element e " +
                    "INNER JOIN detailentretien de ON de.idElement = e.id " +
                    "INNER JOIN entretien ent ON ent.id = de.idEntretien " +
                    "WHERE ent.id = @idEntretien; ", connection);
                command.Parameters.AddWithValue("@idEntretien", idEntretien);

                MySqlDataReader reader = await command.ExecuteReaderAsync();
                var results = new ObservableCollection<ElementVerif>();

                while (reader.Read())
                {
                    var element = new ElementVerif();
                    element.Libelle = (string)reader["Libelle"];
                    element.Annee = (int)reader["Annee"];
                    results.Add(element);
                }

                return results;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public ObservableCollection<ElementFiable> GetElementFiables(MySqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT e.libelle, COUNT(0) as total " +
                    "FROM element e " +
                    "INNER JOIN detailentretien de ON de.idElement = e.id " +
                    "WHERE de.annee IS NOT NULL AND de.annee >= YEAR(NOW())-5 " +
                    "GROUP BY e.libelle " +
                    "ORDER BY total ASC " +
                    "LIMIT 5;" , connection);

                MySqlDataReader reader = command.ExecuteReader();
                var results = new ObservableCollection<ElementFiable>();

                while (reader.Read())
                {
                    var element = new ElementFiable();
                    element.NomElement = (string)reader["libelle"];
                    element.NbIncident = (int)(long)reader["total"];
                    results.Add(element);
                }

                return results;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public ObservableCollection<ElementDefecteux> GetElementDefecteux(MySqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT e.libelle, COUNT(0) as total " +
                    "FROM element e " +
                    "INNER JOIN detailentretien de ON de.idElement = e.id " +
                    "WHERE de.annee IS NOT NULL AND de.annee >= YEAR(NOW())-5 " +
                    "GROUP BY e.libelle " +
                    "ORDER BY total DESC " +
                    "LIMIT 5;" , connection);

                MySqlDataReader reader = command.ExecuteReader();
                var results = new ObservableCollection<ElementDefecteux>();

                while (reader.Read())
                {
                    var element = new ElementDefecteux();
                    element.NomElement = (string)reader["libelle"];
                    element.NbIncident = (int)(long)reader["total"];
                    results.Add(element);
                }

                return results;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public ObservableCollection<IncidentMois> GetMoyenneIncident5Ans(MySqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT ROUND(AVG(countAll),1) AS MoyenneIncidentParMois, CASE mois WHEN 1 THEN 'janvier' WHEN 2 THEN 'février' WHEN 3 THEN 'mars' WHEN 4 THEN 'avril'" +
                   "          WHEN 5 THEN 'mai' WHEN 6 THEN 'juin' WHEN 7 THEN 'juillet' WHEN 8 THEN 'août' WHEN 9 THEN 'septembre' WHEN 10 THEN 'octobre' WHEN 11 THEN 'novembre'"+
                   "          ELSE 'décembre'"+
                   " END AS mois FROM"+
                   " ("+
                   "     SELECT count(0) AS countAll, annee, mois"+
                   "     FROM incident"+
                   "     WHERE annee >= YEAR(NOW()) - 5"+
                   "     GROUP BY annee, mois"+
                   " ) as counts"+
                   " GROUP BY mois; " , connection);

                MySqlDataReader reader = command.ExecuteReader();
                var results = new ObservableCollection<IncidentMois>();

                while (reader.Read())
                {
                    var element = new IncidentMois();
                    element.MoyenneIncidentMois = (decimal)reader["MoyenneIncidentParMois"];
                    element.Mois = (string)reader["mois"];
                    results.Add(element);
                }

                return results;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public ObservableCollection<FonctionnementMoyen> GetFonctionnementMoyenElement(MySqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlCommand command = new MySqlCommand("SELECT  ROUND(AVG(DATEDIFF(t2.dateEntretien, t1.dateEntretien)),0) AS MoyenneElement,libelle"+
                   " FROM("+
                   "   SELECT idElement, idEntretien, CONCAT(annee, '-', mois, '-', jour, ' ', heure) AS dateEntretien"+
                   "   FROM detailentretien"+
                   "   WHERE annee IS NOT NULL"+
                   "   ORDER BY dateEntretien ASC"+
                   " ) t1"+
                   " INNER JOIN("+
                   "   SELECT idElement, idEntretien, CONCAT(annee, '-', mois, '-', jour, ' ', heure) AS dateEntretien"+
                   "   FROM detailentretien"+
                   "   WHERE annee IS NOT NULL"+
                   "   ORDER BY dateEntretien ASC"+
                   " ) t2 ON t1.idElement = t2.idElement AND t1.idEntretien < t2.idEntretien"+
                   " INNER JOIN element e ON t1.idElement = e.id"+
                   " GROUP BY e.libelle; " , connection);

                MySqlDataReader reader = command.ExecuteReader();
                var results = new ObservableCollection<FonctionnementMoyen>();

                while (reader.Read())
                {
                    var element = new FonctionnementMoyen();
                    element.MoyenneElement = (decimal)reader["MoyenneElement"];
                    element.Libelle = (string)reader["libelle"];
                    results.Add(element);
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
