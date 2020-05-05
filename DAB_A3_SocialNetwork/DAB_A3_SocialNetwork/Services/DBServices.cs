﻿using System;
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
        private readonly IMongoCollection<Circles> _circles;
        private readonly IMongoCollection<Followlist> _followlist;
        private readonly IMongoCollection<Blacklist> _blacklist;

        public DatabaseServices(ISocialNetworkDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<Users>(settings.UsersCollectionName);
            _posts = database.GetCollection<Posts>(settings.PostsCollectionName);
            _circles = database.GetCollection<Circles>(settings.CirclesCollectionName);
            _followlist = database.GetCollection<Followlist>(settings.FollowlistCollectionName);
            _blacklist = database.GetCollection<Blacklist>(settings.BlacklistCollectionName);
        }


        //Functions for users
        public List<Users> GetUsers() => _users.Find(users => true).ToList();

        public Users GetUsers(string id) => _users.Find<Users>(users => users.Id == id).FirstOrDefault();

        public Users CreateUser(Users users)
        {
            _users.InsertOne(users);
            return users;
        }

        public void DeleteUser(Users User) => _users.DeleteOne(users => users.Id == User.Id);
        

        //Functions for posts
        public List<Posts> GetPosts() => _posts.Find(posts => true).ToList();

        public List<Posts> GetMyPosts(string id) => _posts.Find(posts => posts.Poster_Id == id).ToList();

        public List<Posts> GetCirclePosts(string id) => _posts.Find(posts => posts.Circle_Id == id).ToList();

        public Posts CreatePost(Posts post)
        {
            _posts.InsertOne(post);
            return post;
        }

        //Functions for Circles
        public List<Circles> GetCircles() => _circles.Find(circles => true).ToList();

        public Circles GetCircle(string id) => _circles.Find<Circles>(circles => circles.Id == id).FirstOrDefault();

        public List<Circles> GetCircleUserIsIn(string id) => _circles.Find<Circles>(circles => circles.UserIds.Contains(id)).ToList();

        public Circles CreateCircle(Circles circle)
        {
            _circles.InsertOne(circle);
            return circle;
        }

        //Functions for Followlist


        //Functions for Blacklist

    }
}
