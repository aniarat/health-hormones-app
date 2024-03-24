using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HealthHormonesAPI.Services;

public class SymptomService : ISymptomService
{
    private readonly IMongoCollection<Symptom> _symptomsCollection;
    
    public SymptomService(
        IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(
            mongoDbSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(
            mongoDbSettings.Value.DatabaseName);
        _symptomsCollection = database.GetCollection<Symptom>(
            mongoDbSettings.Value.SymptomCollectionName);
    } 

    public async Task<Symptom> GetSymptomByIdAsync(string symptomId)
    {
        return await _symptomsCollection.Find(x => x.SymptomId == symptomId).FirstOrDefaultAsync();
    }

    public async Task<List<Symptom>> GetAllSymptomsAsync()
    {
        return await _symptomsCollection.Find(_ => true).ToListAsync();
    }

    public async Task AddSymptomAsync(Symptom symptom)
    {
        await _symptomsCollection.InsertOneAsync(symptom);
    }

    public async Task UpdateSymptomAsync(string symptomId, Symptom symptom)
    {
        await _symptomsCollection.ReplaceOneAsync(x => x.SymptomId == symptomId, symptom);
    }

    public async Task DeleteSymptomAsync(string symptomId)
    {
        await _symptomsCollection.DeleteOneAsync(x => x.SymptomId == symptomId);
    }
}