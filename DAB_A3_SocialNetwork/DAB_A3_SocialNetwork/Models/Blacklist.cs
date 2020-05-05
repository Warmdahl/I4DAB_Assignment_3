using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_A3_SocialNetwork.Models
{
    public class Blacklist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<string> blacklistedIDs { get; set; }
        public string BLOwnerID { get; set; }
    }
}
