using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_A3_SocialNetwork.Models;
using DAB_A3_SocialNetwork.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DAB_A3_SocialNetwork.Controllers
{
    public class seedData
    {

        private readonly DatabaseServices _databaseServices;

        public void SeedingUsers()
        {
            //A bunch of users
            Users u1 = new Users();
            u1.UserName = "Johnny Bravo";
            u1.Age = "24";

            Users u2 = new Users();
            u2.UserName = "Steve Steve";
            u2.Age = "2";

            Users u3 = new Users();
            u3.UserName = "Frans Goldengun";
            u3.Age = "27";

            Users u4 = new Users();
            u4.UserName = "Mads Tolstrup";
            u4.Age = "68";

            Users u5 = new Users();
            u5.UserName = "Preben Hansen";
            u5.Age = "26";

            _databaseServices.CreateUser(u1);
            _databaseServices.CreateUser(u2);
            _databaseServices.CreateUser(u3);
            _databaseServices.CreateUser(u4);
            _databaseServices.CreateUser(u5);
        }

        public void SeedingCircles()
        {
            var users = _databaseServices.GetUsers();

            //A Bunch of circles
            Circles c1 = new Circles();
            c1.circleName = "Thebois";
            c1.UserIds.Add(users[0].Id);
            c1.UserIds.Add(users[1].Id);
            c1.UserIds.Add(users[2].Id);

            //A Bunch of circles
            Circles c2 = new Circles();
            c2.circleName = "HighfivesAreforpussies";
            c2.UserIds.Add(users[2].Id);
            c2.UserIds.Add(users[3].Id);
            c2.UserIds.Add(users[4].Id);

            //A Bunch of circles
            Circles c3 = new Circles();
            c3.circleName = "MenInBlackisthebestmovieEVER";
            c3.UserIds.Add(users[0].Id);
            c3.UserIds.Add(users[2].Id);
            c3.UserIds.Add(users[4].Id);


            _databaseServices.CreateCircle(c1);
            _databaseServices.CreateCircle(c2);
            _databaseServices.CreateCircle(c3);
        }

        public void SeedingPosts()
        {
            var users = _databaseServices.GetUsers();
            var circles = _databaseServices.GetCircles();

            Posts p1 = new Posts();
            p1.ispublic = true;
            p1.Poster_Id = users[0].Id;
            p1.Image = "";
            p1.text = "Hej med dig din stej";
            p1.Circle_Id = circles[0].Id;

            Posts p2 = new Posts();
            p2.ispublic = true;
            p2.Poster_Id = users[2].Id;
            p2.Image = "";
            p2.text = "Frands er høj";
            p2.Circle_Id = circles[2].Id;

            Posts p3 = new Posts();
            p3.ispublic = true;
            p3.Poster_Id = users[1].Id;
            p3.Image = "";
            p3.text = "Hej med dig din stej";
            p3.Circle_Id = circles[1].Id;

            Posts p4 = new Posts();
            p4.ispublic = true;
            p4.Poster_Id = users[0].Id;
            p4.Image = "";
            p4.text = "Hansen er ikke en del af MIB";
            p4.Circle_Id = circles[2].Id;

            Posts p5 = new Posts();
            p5.ispublic = false;
            p5.Poster_Id = users[4].Id;
            p5.Image = "";
            p5.text = "Jeg vil gerne sige hej";
            p5.Circle_Id = null;

            Posts p6 = new Posts();
            p6.ispublic = false;
            p6.Poster_Id = users[1].Id;
            p6.Image = "";
            p6.text = "nononoononoon!";
            p6.Circle_Id = circles[0].Id;

            Posts p7 = new Posts();
            p7.ispublic = true;
            p7.Poster_Id = users[2].Id;
            p7.Image = "";
            p7.text = "My gun is golden";
            p7.Circle_Id = circles[1].Id;

            _databaseServices.CreatePost(p1);
            _databaseServices.CreatePost(p2);
            _databaseServices.CreatePost(p3);
            _databaseServices.CreatePost(p4);
            _databaseServices.CreatePost(p5);
            _databaseServices.CreatePost(p6);
            _databaseServices.CreatePost(p7);
        }

        public void SeedingFollowlist()
        {
            var users = _databaseServices.GetUsers();

            Followlist f1 = new Followlist();
            f1.FLOwnerID = users[1].Id;

            f1.followingIDs.Add(users[2].Id);
            f1.followingIDs.Add(users[3].Id);
            f1.followingIDs.Add(users[5].Id);

            Followlist f2 = new Followlist();
            f1.FLOwnerID = users[2].Id;

            f2.followingIDs.Add(users[1].Id);
            f2.followingIDs.Add(users[2].Id);
            f2.followingIDs.Add(users[4].Id);


            Followlist f3 = new Followlist();
            f3.FLOwnerID = users[3].Id;

            f3.followingIDs.Add(users[2].Id);
            f3.followingIDs.Add(users[3].Id);
            f3.followingIDs.Add(users[4].Id);


            Followlist f4 = new Followlist();
            f4.FLOwnerID = users[4].Id;

            f4.followingIDs.Add(users[4].Id);
            f4.followingIDs.Add(users[5].Id);
            f4.followingIDs.Add(users[1].Id);

            Followlist f5 = new Followlist();
            f5.FLOwnerID = users[5].Id;

            f5.followingIDs.Add(users[2].Id);
            f5.followingIDs.Add(users[3].Id);
            f5.followingIDs.Add(users[4].Id);

            _databaseServices.CreateFollowlist(f1);
            _databaseServices.CreateFollowlist(f2);
            _databaseServices.CreateFollowlist(f3);
            _databaseServices.CreateFollowlist(f4);
            _databaseServices.CreateFollowlist(f5);
        }

        public void SeedingComment()
        {
            var users = _databaseServices.GetUsers();
            var post = _databaseServices.GetPosts();

            //A bunch of comments
            Comments c1 = new Comments();
            c1.PostId = post[0].Id;
            c1.text = "Hey there boi";
            c1.commenterId = users[1].Id;

            Comments c2 = new Comments();
            c1.PostId = post[0].Id;
            c1.text = "How's it hanging";
            c1.commenterId = users[0].Id;

            Comments c3 = new Comments();
            c1.PostId = post[0].Id;
            c1.text = "We are runing out of time :(";
            c1.commenterId = users[1].Id;

            Comments c4 = new Comments();
            c1.PostId = post[2].Id;
            c1.text = "Så kan vi være det sammen";
            c1.commenterId = users[1].Id;

            Comments c5 = new Comments();
            c1.PostId = post[3].Id;
            c1.text = "Nice XD";
            c1.commenterId = users[2].Id;

            _databaseServices.CreateComments(c1);
            _databaseServices.CreateComments(c2);
            _databaseServices.CreateComments(c3);
            _databaseServices.CreateComments(c4);
            _databaseServices.CreateComments(c5);
        }


    }
}
