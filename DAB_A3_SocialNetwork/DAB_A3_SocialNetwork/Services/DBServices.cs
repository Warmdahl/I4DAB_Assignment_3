using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_A3_SocialNetwork.Models;
using MongoDB.Driver;

namespace DAB_A3_SocialNetwork.Services
{
    public class DatabaseServices
    {
        private readonly IMongoCollection<Users> _users;
        private readonly IMongoCollection<Posts> _posts;

        public DatabaseServices(ISocialNetworkDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<Users>(settings.UsersCollectionName);
            _posts = database.GetCollection<Posts>(settings.PostsCollectionName);
        }


        //Functions for users
        public List<Users> GetUsers() => _users.Find(users => true).ToList();

        public Users GetUsers(string id) => _users.Find<Users>(users => users.Id == id).FirstOrDefault();

        public Users CreateUser(Users users)
        {
            _users.InsertOne(users);
            return users;
        }

        //Functions for posts
        public List<Posts> GetPosts() => _posts.Find(posts => true).ToList();
    }
}
