using CommunityToolkit.Mvvm.Input;
using MySqlConnector;
using project_ebis.Model;
using project_ebis.Services;
using project_ebis.View;
using System.Collections.ObjectModel;

namespace project_ebis.ViewModel;

public partial class DashboardViewModel : BaseViewModel
{
    public ObservableCollection<Borne> Bornes { get; set; } = new();
    public ObservableCollection<ElementFiable> elementFiables { get; set; } = new();
    public ObservableCollection<ElementDefecteux> elementDefecteux { get; set; } = new();
    public ObservableCollection<IncidentMois> moyenneIncident { get; set; } = new();
    public ObservableCollection<FonctionnementMoyen> fonctionnementMoyen { get; set; } = new();

    DatabaseService databaseService { get; set; }
    MySqlConnection conn { get; set; }


    public DashboardViewModel()
    {
        this.databaseService = new DatabaseService("localhost", "ebis", 3306, "root", "root");
        this.conn = this.databaseService.CreateConnection();   
    }

    public void GetAllBorne()
    {            
        Bornes = this.databaseService.ExecuteSelectQueryForBorne(conn);
        this.conn.Close();  
        
    }

    public void GetElementFiable()
    {
        elementFiables =  this.databaseService.GetElementFiables(conn);
        this.conn.Close();
    }

    public void GetElementDefecteux()
    {
        elementDefecteux = this.databaseService.GetElementDefecteux(conn);
        this.conn.Close();
    }

    public void GetMoyenneIncident()
    {
        moyenneIncident = databaseService.GetMoyenneIncident5Ans(conn);
        this.conn.Close();
    }

    public void GetFonctionnementMoyen()
    {
        fonctionnementMoyen = databaseService.GetFonctionnementMoyenElement(conn);
        this.conn.Close();
    }

    [RelayCommand]
    async Task GoToBorne(Borne borne)
    {
        await Shell.Current.GoToAsync(
            "BornePage",
            true,
            new Dictionary<string, object>
            {
                {
                "Borne", borne
                }
            });
    }

    [RelayCommand]
    async Task GoToJournauxIncidents()
    {
        await Shell.Current.GoToAsync(
            "JournauxIncidentPage",
            true
           );
    }


    [RelayCommand]
    async Task GoToEntretien()
    {
        await Shell.Current.GoToAsync(nameof(JournauxEntretienPage), true);
    }
}
