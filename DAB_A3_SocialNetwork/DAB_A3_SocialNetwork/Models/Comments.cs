using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_A3_SocialNetwork.Models
{
    public class Comments
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PostId { get; set; }
        public string text { get; set; }
        public string commenterId { get; set; }
    }
}
