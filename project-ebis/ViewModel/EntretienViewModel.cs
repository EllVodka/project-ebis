using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySqlConnector;
using project_ebis.Model;
using project_ebis.Services;
using System.Collections.ObjectModel;

namespace project_ebis.ViewModel;

[QueryProperty("Entretien", "Entretien")]
public partial class EntretienViewModel : BaseViewModel
{
    [ObservableProperty]
    Entretien entretien;

    public ObservableCollection<ElementVerif> elementVerif { get; set; } = new();
    public ObservableCollection<ElementVerif> elementChanger { get; set; } = new();

    DatabaseService databaseService { get; set; }
    MySqlConnection conn { get; set; }

    public EntretienViewModel()
    {
        databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");
    }

    [RelayCommand]
    async Task GetElementVerif()
    {
        if (EstOccupe)
            return;
        EstOccupe = true;
        conn = databaseService.CreateConnection();
        var elements = await databaseService.GetElementVerif(conn, Entretien.IdEntretien);
        if (elementVerif.Count != 0)
        {
            elementVerif.Clear();
            elementChanger.Clear();
        }

        foreach (ElementVerif element in elements)
        {
            
            if (element.Annee == 1)
            {
                elementChanger.Add(element);
            }
            else
            {
                elementVerif.Add(element);
            }
        }

        conn.Close();
        EstOccupe = false;
    }
}
