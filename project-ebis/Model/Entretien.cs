namespace project_ebis.Model;

public class Entretien
{
    public int IdStation { get; set; }
    public int IdBorne { get; set; }
    public int IdEntretien { get; set; }
    public string PrenomTechnicien { get; set; }
    public string NomTechnicien { get; set; }
    public string DateEntretien { get; set; }
}
