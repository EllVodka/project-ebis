﻿using project_ebis.Model;
using project_ebis.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_ebis.ViewModel
{
    public partial class JournauxIncidentViewModel : BindableObject
    {
        public ObservableCollection<JournalIncident> JournauxIncidents { get; set; }

        public void GetAllJounauxIncidents()
        {
            var databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");

            var conn = databaseService.CreateConnection();

            JournauxIncidents = databaseService.ExecuteSelectQueryForJournauxIncidents(conn);
        }
    }
}
