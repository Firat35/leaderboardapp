using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Api.Models
{
    public class LeaderboardUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public int Rank { get; set; }

        public int Point { get; set; }

        public string Prize { get; set; }

        public DateTime CreatedTime {get; set; }
    }
}
