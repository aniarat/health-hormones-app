using HealthHormonesAPI.Models;

namespace HealthHormonesAPI.Interfaces;

public interface ISymptomService
{
    public Task<Symptom> GetSymptomByIdAsync(string symptomId);
    public Task<List<Symptom>> GetAllSymptomsAsync();
    public Task AddSymptomAsync(Symptom symptom);
    public Task UpdateSymptomAsync(string symptomId,Symptom symptom);
    public Task DeleteSymptomAsync(string symptomId);
}