using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAB_A3_SocialNetwork.Models;
using Microsoft.CodeAnalysis.CSharp;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAB_A3_SocialNetwork.SeedData
{
    public class Collections
    {
        public void Seedingdata(ISocialNetworkDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            var collection = database.GetCollection<BsonDocument>("Students");
        }

    }
}
