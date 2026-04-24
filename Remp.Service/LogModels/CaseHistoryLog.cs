using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Remp.Service.LogModels;

public class CaseHistoryLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Event { get; set; }
    public string OperatorId { get; set; }
    public int? ListingCaseId { get; set; }
    public string? Detail { get; set; }
    public DateTime Timestamp { get; set; }
}