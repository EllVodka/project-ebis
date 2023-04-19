using MySqlConnector;
using project_ebis.Model;
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
    public partial class StatViewModel : BindableObject
    {
        public ObservableCollection<string> Resultats { get; set; }
        public ObservableCollection<Borne> Bornes { get; set; } = new();

        public StatViewModel()
        {
            Resultats = new ObservableCollection<string>();
          
        }

        public void SelectToDatabase()
        {
            var databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");

            var conn = databaseService.CreateConnection();

            Resultats = databaseService.ExecuteSelectQuery("SELECT libelle FROM secteur;", conn);
        }

        public void GetAllBorne()
        {
            var databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");

            var conn = databaseService.CreateConnection();

            Bornes = databaseService.ExecuteSelectQueryForBorne(conn);
        }
    }
}
