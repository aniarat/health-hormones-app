using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthHormonesAPI.Models
{
    [Serializable, BsonIgnoreExtraElements]
    public class Symptom
    {
        [BsonId, BsonElement("Id"), BsonRepresentation(BsonType.ObjectId)]
        public string SymptomId { get; set; } = null!;
        
        [BsonElement("CreatedAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } // Date and time when the symptom was added

        public string Location { get; set; } = null!; // Location of the symptom (e.g., head, abdomen, back)
        public string Severity { get; set; } = null!;// Severity of the symptom (e.g., mild, moderate, severe)
        
        [BsonElement("SeverityScale"), BsonRepresentation(BsonType.Int32)]
        public int SeverityScale { get; set; } // Severity scale of the symptom, ranging from 1 to 10
        public bool IsPersistent { get; set; } // Indicates whether the symptom is persistent over time
        
        [BsonElement("DurationHours"), BsonRepresentation(BsonType.Int32)]
        public int? SymptomDurationHours { get; set; } // Duration of symptom in hours
        
        [BsonElement("StartDate"), BsonRepresentation(BsonType.DateTime)]
        public DateTime? StartDate { get; set; } // Date and time when the symptom started
        
        [BsonElement("EndDate"), BsonRepresentation(BsonType.DateTime)]
        public DateTime? EndDate { get; set; } // Date and time when the symptom ended
        public string? Description { get; set; } // Description of the symptom
        public string? Treatment { get; set; } // Information about treatment or therapy used to alleviate the symptom
        public string? AssociatedSymptoms { get; set; } // Associated symptoms that occur concurrently with the main symptom
        public string? Trigger { get; set; } // Triggers that initiate the symptom (e.g., stress, specific foods, weather changes)
        public string? Notes { get; set; } // Additional notes related to the symptom entry


    }
}