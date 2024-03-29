namespace HealthHormonesAPI.Models;

public class ChangedSymptom
{
    public DateTime ChangeTime { get; set; }
    public string PropertyName { get; set; } = null!;
    public string OldValue { get; set; } = null!;
    public string NewValue { get; set; } = null!;
}