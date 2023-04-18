using MySqlConnector;
using project_ebis.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_ebis.ViewModel
{
    public class StatViewModel : BindableObject
    {
        public ObservableCollection<string> Resultats { get; set; }

        public StatViewModel()
        {
            Resultats = new ObservableCollection<string>();
        }

        public void SelectToDatabase()
        {
            var databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");

            var conn = databaseService.CreateConnection();

            Resultats = databaseService.ExecuteSelectQuery("SELECT libelle FROM ebis.secteur;", conn);
        }
    }
}
