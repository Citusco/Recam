using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Remp.Service.LogModels;

public class AuthLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Event { get; set; }
    public string? UserId { get; set; }
    public string Email { get; set; }
    public string? Role { get; set; }
    public string? FailureReason { get; set; }
    public DateTime Timestamp { get; set; }
}
