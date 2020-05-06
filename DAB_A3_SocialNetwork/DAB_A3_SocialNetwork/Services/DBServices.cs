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
        private readonly IMongoCollection<Circles> _circles;
        private readonly IMongoCollection<Followlist> _followlist;
        private readonly IMongoCollection<Blacklist> _blacklist;
        private readonly IMongoCollection<Comments> _comments;

        public DatabaseServices(ISocialNetworkDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<Users>(settings.UsersCollectionName);
            _posts = database.GetCollection<Posts>(settings.PostsCollectionName);
            _circles = database.GetCollection<Circles>(settings.CirclesCollectionName);
            _followlist = database.GetCollection<Followlist>(settings.FollowlistCollectionName);        
            _blacklist = database.GetCollection<Blacklist>(settings.BlacklistCollectionName);
            _comments = database.GetCollection<Comments>(settings.CommentsCollectionName);
        }


        //Functions for users
        public List<Users> GetUsers() => _users.Find(users => true).ToList();

        public Users GetUsers(string id) => _users.Find<Users>(users => users.Id == id).FirstOrDefault();

        public Users CreateUser(Users users)
        {
            _users.InsertOne(users);
            return users;
        }

        public void DeleteUser(Users user) => _users.DeleteOne(users => users.Id == user.Id);

        public void UpdateUser(string id, Users newuser) => _users.ReplaceOne(users => users.Id == id, newuser);
        

        //Functions for posts
        public List<Posts> GetPosts() => _posts.Find(posts => true).ToList();

        public List<Posts> GetMyPosts(string id) => _posts.Find(posts => posts.Poster_Id == id).ToList();

        public List<Posts> GetPostbyPostid(string id) => _posts.Find(posts => posts.Id == id).ToList();

        public List<Posts> GetCirclePosts(string id) => _posts.Find(posts => posts.Circle_Id == id).ToList();

        public Posts CreatePost(Posts post)
        {
            _posts.InsertOne(post);
            return post;
        }

        public void UpdatePost(string id, Posts post) => _posts.ReplaceOne(post => post.Id == id, post);



        //Functions for Circles
        public List<Circles> GetCircles() => _circles.Find(circles => true).ToList();

        public Circles GetCircle(string id) => _circles.Find<Circles>(circles => circles.Id == id).FirstOrDefault();

        public List<Circles> GetCircleUserIsIn(string id) => _circles.Find<Circles>(circles => circles.UserIds.Contains(id)).ToList();

        public Circles CreateCircle(Circles circle)
        {
            _circles.InsertOne(circle);
            return circle;
        }

        public void DeleteCircle(Circles circle) => _circles.DeleteOne(circles => circles.Id == circle.Id);

        public void UpdateCircle(string id, Circles circle) => _circles.ReplaceOne(circle => circle.Id == id, circle);

        //Functions for Followlist
        public Followlist GetUserFollowlists(string id) => _followlist.Find<Followlist>(Followlist => Followlist.FLOwnerID == id).FirstOrDefault(); 

        public Followlist CreateFollowlist(Followlist followlist)
        {
            _followlist.InsertOne(followlist);
            return followlist;
        }


        //Functions for Blacklist
        public Blacklist GetUserBlacklist(string id) => _blacklist.Find<Blacklist>(blacklist => blacklist.BLOwnerID == id).FirstOrDefault();

        public Blacklist CreateBlacklist(Blacklist blacklist)
        {
            _blacklist.InsertOne(blacklist);
            return blacklist;
        }

        //Functions for Comments
        public Comments GetComments(string id) => _comments.Find<Comments>(comment => comment.PostId == id).FirstOrDefault();

        public Comments CreateComments(Comments comment)
        {
            _comments.InsertOne(comment);
            return comment;
        }

    }
}
