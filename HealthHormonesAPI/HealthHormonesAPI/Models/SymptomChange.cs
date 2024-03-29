using MongoDB.Bson.Serialization.Attributes;

namespace HealthHormonesAPI.Models;

public class SymptomChange
{
    // [BsonId]
    // public string Id { get; set; } = null!;

    [BsonElement("SymptomId")]
    public string SymptomId { get; set; } = null!;

    [BsonElement("ChangeType")]
    public ChangeType ChangeType { get; set; } 

    [BsonElement("ChangedField")]
    public string ChangedField { get; set; } = null!;

    [BsonElement("OldValue")]
    public object OldValue { get; set; }  = null!;

    [BsonElement("NewValue")]
    public object NewValue { get; set; }  = null!;

    [BsonElement("ChangeTime")]
    public DateTime ChangeTime { get; set; } = DateTime.UtcNow;
}

public enum ChangeType
{
    Created,
    Updated,
    Deleted
}
