using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyHostAPI.Domain
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

