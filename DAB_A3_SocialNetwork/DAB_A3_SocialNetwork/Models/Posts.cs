using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAB_A3_SocialNetwork.Models
{
    public class Posts
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("public")] 
        public bool ispublic { get; set; }
        public string Circle_Id { get; set; }
        public string Poster_Id { get; set; }

        public string text { get; set; }
        public string Image { get; set; }
    }
}
