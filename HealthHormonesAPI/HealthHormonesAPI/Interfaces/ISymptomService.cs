using HealthHormonesAPI.Models;
using MongoDB.Driver;

namespace HealthHormonesAPI.Interfaces;

public interface ISymptomService
{
    public Task<Symptom?> GetSymptomByIdAsync(string symptomId);
    public Task<List<Symptom>> GetAllSymptomsAsync();
    public Task<Symptom> AddSymptomAsync(Symptom symptom);
    public Task<ReplaceOneResult> UpdateSymptomAsync(string symptomId, Symptom symptom);
    public Task<DeleteResult> DeleteSymptomAsync(string symptomId);
}