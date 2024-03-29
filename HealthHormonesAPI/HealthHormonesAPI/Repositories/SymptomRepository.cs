using System.Reflection;
using System.Text.Json;
using HealthHormonesAPI.Exceptions;
using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;


namespace HealthHormonesAPI.Repositories;

public class SymptomRepository : ISymptomRepository
{
    private readonly IMongoCollection<Symptom> _symptomsCollection;
    private ISymptomRepository _symptomRepositoryImplementation;

    public SymptomRepository(
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
        var symptom = await _symptomsCollection.Find(x => x.SymptomId == symptomId).FirstOrDefaultAsync();
        if (symptom is null)
        {
            throw new SymptomNotFoundException(symptomId);
        }

        return symptom;
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

    public async Task UpdateSymptomPropertyAsync(string symptomId, Symptom updatedSymptom)
    {
        // Pobierz istniejący symptom
        Symptom existingSymptom = await GetSymptomByIdAsync(symptomId);
        if (existingSymptom == null)
        {
            throw new SymptomNotFoundException(symptomId);
        }
        
        var changedFields = new List<SymptomChange>();
        foreach (var property in updatedSymptom.GetType().GetProperties())
        {
            var currentValue = property.GetValue(updatedSymptom);
            var originalValue = property.GetValue(existingSymptom);

            if (!Equals(currentValue, originalValue))
            {
                // var jsonSerializerSettings = new JsonSerializerSettings()
                // {
                //     Formatting= Formatting.Indented,
                // };

                string oldValue = originalValue.ToString();
                string newValue = currentValue.ToString();

                changedFields.Add(new SymptomChange
                {
                    SymptomId = symptomId,
                    ChangeType = ChangeType.Updated,
                    ChangeTime = DateTime.UtcNow,
                    ChangedField = property.Name,
                    OldValue = oldValue,
                    NewValue = newValue,
                });
            }
        }
        await UpdateSymptomAsync(symptomId, updatedSymptom);
        // SymptomId = symptomId,
        //             ChangeType = ChangeType.Updated,
        //             ChangeTime = DateTime.UtcNow,
        //             ChangedField = property.Name,
        //             OldValue = oldValue.ToString(),
        //             NewValue = newValue.ToString()
        // List<SymptomChange> changes = new List<SymptomChange>();
        //
        // // Porównaj wartości właściwości między istniejącym a zaktualizowanym objektem
        // PropertyInfo[] properties = typeof(Symptom).GetProperties();
        // foreach (PropertyInfo property in properties)
        // {
        //     object oldValue = property.GetValue(existingSymptom);
        //     object newValue = property.GetValue(updatedSymptom);
        //
        //     if (!object.Equals(oldValue, newValue))
        //     {
        //         // Utwórz instancję SymptomChange
        //         var symptomChange = new SymptomChange
        //         {
        //             SymptomId = symptomId,
        //             ChangeType = ChangeType.Updated,
        //             ChangeTime = DateTime.UtcNow,
        //             ChangedField = property.Name,
        //             OldValue = oldValue.ToString(),
        //             NewValue = newValue.ToString()
        //         };
        //
        //         // Dodaj zmianę do listy zmian
        //         changes.Add(symptomChange);
        //     }
        // }
        //
        // // Dodaj listę zmian do istniejącego symptomu
        // existingSymptom.ChangeSymptoms.AddRange(changes);
        //
        // // Zaktualizuj symptom w bazie danych
        // await UpdateSymptomAsync(symptomId, updatedSymptom);
        //
        // Pobierz istniejący symptom

        //
        // string existingJson = JsonConvert.SerializeObject(existingSymptom);
        // string updatedJson = JsonConvert.SerializeObject(updatedSymptom);
        //
        // if (existingJson != updatedJson)
        // {
        //     // Utwórz nową instancję SymptomChange
        //     var symptomChange = new SymptomChange
        //     {
        //         SymptomId = symptomId,
        //         ChangeType = ChangeType.Updated,
        //         ChangeTime = DateTime.UtcNow,
        //         OldValue = existingJson,
        //         NewValue = updatedJson
        //     };
        //     
        //     if (existingSymptom.ChangeSymptoms == null)
        //     { 
        //         existingSymptom.ChangeSymptoms = new List<SymptomChange>();
        //     }
        //     existingSymptom.ChangeSymptoms.Add(symptomChange);
        //
        //
        //     await UpdateSymptomAsync(symptomId, updatedSymptom);
        
            // PropertyInfo property = typeof(Symptom).GetProperty(propertyName);
            // if (property == null)
            // {
            //     throw new ArgumentException("Invalid property name.");
            // }
            //
            // object oldValue = property.GetValue(existingSymptom);
            // if (oldValue != null && oldValue.ToString() == newValue)
            // {
            //     return; // Nie aktualizuj, jeśli wartość jest taka sama
            // }
            //
            // // Utwórz nową zmianę
            // ChangedSymptom changedSymptom = new ChangedSymptom
            // {
            //     ChangeTime = DateTime.UtcNow,
            //     PropertyName = propertyName,
            //     OldValue = oldValue?.ToString(),
            //     NewValue = newValue
            // };
            //
            // // Dodaj zmianę do historii
            // if (existingSymptom.ChangeSymptoms == null)
            // {
            //     existingSymptom.ChangeSymptoms = new List<ChangedSymptom>();
            // }
            // existingSymptom.ChangeSymptoms.Add(changedSymptom);
            //
            // // Zaktualizuj wartość pola w symptomie
            // property.SetValue(existingSymptom, Convert.ChangeType(newValue, property.PropertyType));
            //
            // // Zapisz zmieniony Symptom z nową historią zmian
            // await UpdateSymptomAsync(symptomId, existingSymptom);
    }

    public Task DeleteSymptomAsync(string symptomId)
    {
        throw new NotImplementedException();
    }

    // public async Task DeleteSymptomAsync(string symptomId)
        // {
        //     await _symptomsCollection.DeleteOneAsync(x => x.SymptomId == symptomId);
        // }
        // Zaktualizuj dokument symptomu po przejściu przez wszystkie zmienione właściwości
    

    // public async Task DeleteSymptomAsync(string symptomId)
    // {
    //     await _symptomsCollection.DeleteOneAsync(x => x.SymptomId == symptomId);    
    // }
}