using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySqlConnector;
using project_ebis.Model;
using project_ebis.Services;
using System.Collections.ObjectModel;

namespace project_ebis.ViewModel
{
    [QueryProperty("Borne", "Borne")]
    public partial class BorneViewModels : BaseViewModel
    {
        [ObservableProperty]
        Borne borne;

        public ObservableCollection<Operation> journalOperation { get; set; } = new();

        DatabaseService databaseService { get; set; }
        MySqlConnection conn { get; set; }

        public BorneViewModels()
        {
            databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");
            
        }

        [RelayCommand]
        async Task OpenMap()
        {
            await Map.Default.OpenAsync(Borne.Latitude,Borne.Longitude);
        }

        [RelayCommand]
        async Task GetJournalOperation() 
        {
            if (EstOccupe)
                return;
            EstOccupe = true;
            conn = databaseService.CreateConnection();
            var operations = await databaseService.GetJournalOperation(conn, Borne.IdBorne);
            if( journalOperation.Count != 0)
            {
                journalOperation.Clear();
            }

            foreach(Operation operation in operations)
            {
                journalOperation.Add(operation);
            }

            conn.Close();
            EstOccupe = false;
        }

        [RelayCommand]
        async Task GoToOperation(Operation operation)
        {
            await Shell.Current.GoToAsync(
                "OperationPage",
                true,
                new Dictionary<string, object>
                {
                    {
                    "Operation", operation
                    }
                });
        }
    }
}
