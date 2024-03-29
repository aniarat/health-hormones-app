using HealthHormonesAPI.Models;

namespace HealthHormonesAPI.Interfaces;

public interface ISymptomChangeService
{
    public Task UpdateSymptomAsync(string symptomId, Symptom updatedSymptom);  
}