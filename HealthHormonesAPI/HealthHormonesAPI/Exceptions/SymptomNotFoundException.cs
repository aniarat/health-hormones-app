namespace HealthHormonesAPI.Exceptions;

public class SymptomNotFoundException : NotFoundException
{
    public SymptomNotFoundException(string symptomId)
        : base($"The symptom with the identifier {symptomId} was not found.")
    {
    }
}
