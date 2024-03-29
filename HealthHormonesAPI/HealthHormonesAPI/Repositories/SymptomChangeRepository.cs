using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace HealthHormonesAPI.Repositories;

public class SymptomChangeRepository : ISymptomChangeRepository
{
    private readonly IMongoCollection<SymptomChange> _symptomChangesCollection;
    
    // public SymptomChangeRepository(IMongoCollection<SymptomChange> symptomChangesCollection)
    // {
    //         _symptomChangesCollection = symptomChangesCollection;
    // }
    public SymptomChangeRepository(
        IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(
            mongoDbSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(
            mongoDbSettings.Value.DatabaseName);
        _symptomChangesCollection = database.GetCollection<SymptomChange>(
            mongoDbSettings.Value.SymptomChangeCollectionName);
    } 
    
    public async Task AddSymptomChangeAsync(SymptomChange symptomChange)
    {
        
        await _symptomChangesCollection.InsertOneAsync(symptomChange);
    }

    public async Task<IEnumerable<SymptomChange>> GetSymptomChangesForSymptomAsync(string symptomId)
    {
        var filter = Builders<SymptomChange>.Filter.Eq(x => x.SymptomId, symptomId);
        return await _symptomChangesCollection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<SymptomChange>> GetSymptomChangesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var filter = Builders<SymptomChange>.Filter.And(
            Builders<SymptomChange>.Filter.Gte(x => x.ChangeTime, startDate),
            Builders<SymptomChange>.Filter.Lte(x => x.ChangeTime, endDate));

        return await _symptomChangesCollection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<SymptomChange>> GetSymptomChangesByTypeAsync(ChangeType changeType)
    {
        var filter = Builders<SymptomChange>.Filter.Eq(x => x.ChangeType, changeType);
        return await _symptomChangesCollection.Find(filter).ToListAsync();    }

    public async Task DeleteSymptomChangesForSymptomAsync(string symptomId)
    {
        var filter = Builders<SymptomChange>.Filter.Eq(x => x.SymptomId, symptomId);
        await _symptomChangesCollection.DeleteManyAsync(filter);    
    }

    public async Task DeleteSymptomChangesOlderThanAsync(DateTime date)
    {
        var filter = Builders<SymptomChange>.Filter.Lt(x => x.ChangeTime, date);
        await _symptomChangesCollection.DeleteManyAsync(filter);
    }

    public async Task<long> CountSymptomChangesAsync()
    {
        return await _symptomChangesCollection.CountDocumentsAsync(FilterDefinition<SymptomChange>.Empty);
    }

    public async Task<SymptomChange> GetSymptomChangeByIdAsync(string symptomChangeId)
    {
        var filter = Builders<SymptomChange>.Filter.Eq(x => x.SymptomId, symptomChangeId);
        return await _symptomChangesCollection.Find(filter).FirstOrDefaultAsync();    
    }
}