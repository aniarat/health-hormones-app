using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HealthHormonesAPI.Services;

public class SymptomService : ISymptomService
{
    private readonly IMongoCollection<Symptom> _symptomsCollection;
    
    public SymptomService(
        IOptions<MongoDbSettings> mongoDbSettings, IMongoClient client)
    {
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _symptomsCollection = database.GetCollection<Symptom>(mongoDbSettings.Value.SymptomCollectionName);
        // MongoClient client = new MongoClient(
        //     mongoDbSettings.Value.ConnectionString);
        // IMongoDatabase database = client.GetDatabase(
        //     mongoDbSettings.Value.DatabaseName);
        // _symptomsCollection = database.GetCollection<Symptom>(
        //     mongoDbSettings.Value.SymptomCollectionName);
    } 

    public async Task<Symptom?> GetSymptomByIdAsync(string symptomId)
    {
        return await _symptomsCollection.Find(x => x.Id == symptomId).FirstOrDefaultAsync();
    }

    public async Task<List<Symptom>> GetAllSymptomsAsync()
    {
        return await _symptomsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Symptom> AddSymptomAsync(Symptom symptom)
    {
        //symptom.Id = ObjectId.GenerateNewId().ToString();
        await _symptomsCollection.InsertOneAsync(symptom);
        return symptom;
    }

    public async Task<ReplaceOneResult> UpdateSymptomAsync(string symptomId, Symptom symptom)
    {
        // Symptom existingSymptom = await GetSymptomByIdAsync(symptomId);
        // if (existingSymptom != null)
        // {
        //     // Utwórz listę zmian
        //     List<string> changes = CompareSymptoms(existingSymptom, symptom);
        //
        //     // Jeśli wystąpiły zmiany, zapisz je w historii
        //     if (changes.Count > 0)
        //     {
        //         // Utwórz obiekt SymptomHistory z informacjami o zmianach
        //         SymptomHistory history = new SymptomHistory
        //         {
        //             Date = DateTime.UtcNow,
        //             Action = "Update",
        //             Data = string.Join("; ", changes)
        //         };
        //
        //         // Dodaj historię do objawu
        //         if (existingSymptom.History == null)
        //         {
        //             existingSymptom.History = new List<SymptomHistory>();
        //         }
        //
        //         existingSymptom.History.Add(history);
        //         
        //     }
        //
        //     // Zaktualizuj objaw w bazie danych
        // }
        
        return await _symptomsCollection.ReplaceOneAsync(x => x.Id == symptomId, symptom);

        //symptom.Updated = true;
        //return await _symptomsCollection.ReplaceOneAsync(x => x.Id == symptomId, symptom);
        
    }

    public async Task<DeleteResult> DeleteSymptomAsync(string symptomId)
    {
        return await _symptomsCollection.DeleteOneAsync(x => x.Id == symptomId);
    }
    
    private List<string> CompareSymptoms(Symptom existingSymptom, Symptom updatedSymptom)
    {
        List<string> changes = new List<string>();

        if (existingSymptom.Location != updatedSymptom.Location)
        {
            changes.Add($"Location changed from '{existingSymptom.Location}' to '{updatedSymptom.Location}'");
        }

        if (existingSymptom.Severity != updatedSymptom.Severity)
        {
            changes.Add($"Severity changed from '{existingSymptom.Severity}' to '{updatedSymptom.Severity}'");
        }

        if (existingSymptom.Description != updatedSymptom.Description)
        {
            changes.Add($"Description changed from '{existingSymptom.Description}' to '{updatedSymptom.Description}'");
        }

        // Dodaj dodatkowe porównania dla innych pól objawu, jeśli są one obecne

        return changes;
    }
}