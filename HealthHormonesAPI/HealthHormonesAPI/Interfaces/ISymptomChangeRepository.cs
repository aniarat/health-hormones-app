using HealthHormonesAPI.Models;

namespace HealthHormonesAPI.Interfaces;

public interface ISymptomChangeRepository
{
    Task AddSymptomChangeAsync(SymptomChange symptomChange);
    Task<IEnumerable<SymptomChange>> GetSymptomChangesForSymptomAsync(string symptomId);
    Task<IEnumerable<SymptomChange>> GetSymptomChangesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SymptomChange>> GetSymptomChangesByTypeAsync(ChangeType changeType);
    Task DeleteSymptomChangesForSymptomAsync(string symptomId);
    Task DeleteSymptomChangesOlderThanAsync(DateTime date);
    Task<long> CountSymptomChangesAsync();
    Task<SymptomChange> GetSymptomChangeByIdAsync(string symptomChangeId);
}