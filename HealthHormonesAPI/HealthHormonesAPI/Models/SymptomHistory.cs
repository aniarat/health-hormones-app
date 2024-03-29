namespace HealthHormonesAPI.Models;

public class SymptomHistory
{
    public DateTime Date { get; set; }
    public string Action { get; set; } = null!;
    public string Data { get; set; } = null!;

}