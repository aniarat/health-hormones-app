using System.Reflection;
using HealthHormonesAPI.Exceptions;
using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;


namespace HealthHormonesAPI.Services;

public class SymptomChangeService : ISymptomChangeService
{
    private readonly IMongoCollection<SymptomChange> _symptomChangesCollection;
    private readonly IMongoCollection<Symptom> _symptomsCollection;
    private readonly ISymptomRepository _symptomRepository;
    private readonly ISymptomChangeRepository _symptomChangeRepository;

    public SymptomChangeService(
        IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(
            mongoDbSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(
            mongoDbSettings.Value.DatabaseName);
        _symptomChangesCollection = database.GetCollection<SymptomChange>(
            mongoDbSettings.Value.SymptomChangeCollectionName);
    } 
    // public async TrackSymptomChange(SymptomChange change)
    // {
    //     await _symptomChangesCollection.InsertOneAsync(change);
    // }
    //
    // public Task<IEnumerable<SymptomChange>> GetSymptomChanges(string symptomId)
    // {
    //     throw new NotImplementedException();
    // }


    public async Task UpdateSymptomAsync(string symptomId, Symptom updatedSymptom)
    {
        // Pobierz istniejący symptom
        Symptom existingSymptom = await _symptomRepository.GetSymptomByIdAsync(symptomId);
        if (existingSymptom == null)
        {
            // Obsługa braku istniejącego symptomu
            throw new SymptomNotFoundException(symptomId);
        }
        
        string existingJson = JsonConvert.SerializeObject(existingSymptom);
        string updatedJson = JsonConvert.SerializeObject(updatedSymptom);

        if (existingJson != updatedJson)
        {
            // Utwórz nową instancję SymptomChange
            var symptomChange = new SymptomChange
            {
                SymptomId = symptomId,
                ChangeType = ChangeType.Updated,
                ChangeTime = DateTime.UtcNow,
                OldValue = existingJson,
                NewValue = updatedJson
            };
        

        // // Przejdź przez wszystkie właściwości i zapisz zmiany
        // PropertyInfo[] properties = typeof(Symptom).GetProperties();
        // foreach (PropertyInfo property in properties)
        // {
        //     object oldValue = property.GetValue(existingSymptom);
        //     object newValue = property.GetValue(updatedSymptom);
        //
        //     if (!object.Equals(oldValue, newValue))
        //     {
        //         // Utwórz nową instancję SymptomChange dla każdej zmienionej właściwości
        //         var symptomChange = new SymptomChange
        //         {
        //             SymptomId = symptomId,
        //             ChangeType = ChangeType.Updated,
        //             ChangeTime = DateTime.UtcNow,
        //             ChangedField = property.Name,
        //             OldValue = oldValue,
        //             NewValue = newValue
        //         };
        //
        //         // Zapisz zmianę symptomu
        //         await _symptomChangeRepository.AddSymptomChangeAsync(symptomChange);
        //     }
        }

        // Zaktualizuj dokument symptomu po przejściu przez wszystkie zmienione właściwości
        await _symptomRepository.UpdateSymptomAsync(symptomId, updatedSymptom);
    }
}