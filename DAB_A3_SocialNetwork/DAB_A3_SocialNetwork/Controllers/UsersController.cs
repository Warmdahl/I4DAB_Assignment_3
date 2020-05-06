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
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseServices _databaseServices;

        public UsersController(DatabaseServices databaseServices)
        {
            _databaseServices = databaseServices;
        }

        struct Userfeed
        {
            public Users user;
            public List<Posts> userposts;
            public List<Circles> circles;
            public List<Posts> circleposts;
            public List<Posts> followingposts;
        }

        //Shows the users in the database
        [HttpGet]
        public ActionResult<object> Get()
        {
            var users =_databaseServices.GetUsers();

            if (users == null) //Seeder data til database
            {
                TherulingSeed();
            }

            return users;
        }

        //Shows a specific user and their own posts (for now)
        [HttpGet("{id:length(24)}", Name = "GetUserFeed")]
        [ActionName("Getmyfeed")]
        public ActionResult<object> Get(string id)
        {
            var user = _databaseServices.GetUsers(id);                          //Finder user information
            var posts = _databaseServices.GetMyPosts(id);                  //Finder post skrevet af denne user
            
            var circles = _databaseServices.GetCircleUserIsIn(id);        //Finder de circles user er med i
            var circleposts = new List<Posts>();                                    //Finder de posts som er skrevet i de circles user er med i
            foreach (var circle in circles)
            {
                var tempId = circle.Id;
                var tempcircleposts = _databaseServices.GetCirclePosts(tempId);
                foreach (var p in tempcircleposts)
                {
                    circleposts.Add(p);
                }
            }

            var followlist = _databaseServices.GetUserFollowlists(id);              //Finder alle posts lavede af users som user følger
            List<Posts> FLposts = new List<Posts>();
            foreach (var FLids in followlist.followingIDs)
            {
                var ID = FLids;                                               //Laver en liste med alle posts fra alle på followlisten
                var UsersPosts = _databaseServices.GetMyPosts(ID);
                foreach (var p in UsersPosts)
                {
                    FLposts.Add(p);
                }
            }

            if (user == null || posts == null)                                      //Checker om user findes hvis ikke sendes 404
            {
                return NotFound();
            }

            Userfeed userfeed = new Userfeed();                                     //Struct som indeholder alt information som vises på siden.
            userfeed.user = user;
            userfeed.userposts = posts;
            userfeed.circles = circles;
            userfeed.circleposts = circleposts;
            userfeed.followingposts = FLposts;

            return userfeed;                    
        }

        //Creates a new user
        [HttpPost]
        public ActionResult<Users> Create(Users users)
        {
            _databaseServices.CreateUser(users);

            return CreatedAtRoute("GetUser", new {id = users.Id.ToString()}, users);
        }

        //Deletes a user
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = _databaseServices.GetUsers(id);

            if (user == null)
            {
                return NotFound();
            }

            _databaseServices.DeleteUser(user);

            return NoContent();
        }

        //Update a user
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Users newuser)
        {
            var olduser = _databaseServices.GetUsers(id);

            if (olduser == null)
            {
                return NotFound();
            }

            _databaseServices.UpdateUser(id, newuser);

            return NoContent();
        }


        struct VisitedFeed
        {
            public Users user;
            public List<Posts> userposts;
            public List<Circles> circles;
            public List<Posts> circleposts;
        }

        //Get where you visit someone else
        //Own id first, visit id second
        [HttpGet("{ownid:length(24)}/{visitid:length(24)}", Name = "GetUserVisit")]
        public ActionResult<object> Get(string ownid, string visitid)
        {
            var own = _databaseServices.GetUsers(ownid);                                    //Id på den user som tilgår en anden user
            var visit = _databaseServices.GetUsers(visitid);                                //Id på den user som bliver tilgået

            if (own == null || visit == null)                                                   //Hvis en af de to users ikke findes, sendes 404
            {
                return NotFound();
            }

            var publicposts = new List<Posts>();
            var posts = _databaseServices.GetMyPosts(visitid);                          //Finder post skrevet af denne user og sikre us at de er public
            foreach (var post in posts)
            {
                if (post.ispublic == true)
                {
                    publicposts.Add(post);
                }

            }

            var comcircles = new List<Circles>();
            var owncircles = _databaseServices.GetCircleUserIsIn(ownid);            //Finder de circles user er med i
            var visitcircles = _databaseServices.GetCircleUserIsIn(visitid);        //Finder de circles som visited user er i
            foreach (var ownc in owncircles)
            {
                foreach (var visc in visitcircles)
                {
                    if (ownc.Id == visc.Id)
                        comcircles.Add(visc);                                                   //Laver en ny liste over alle de circles som er fælles
                }
            }

            var circleposts = new List<Posts>();                                                //Finder de posts som er skrevet i de fællescircles begge users er med i
            foreach (var circle in comcircles)
            {
                var tempId = circle.Id;
                var tempcircleposts = _databaseServices.GetCirclePosts(tempId);
                foreach (var p in tempcircleposts)
                {
                    circleposts.Add(p);
                }
            }

            VisitedFeed visitedFeed = new VisitedFeed();                                     //Struct som indeholder alt information som vises på siden.
            visitedFeed.user = visit;
            visitedFeed.userposts = publicposts;
            visitedFeed.circles = comcircles;
            visitedFeed.circleposts = circleposts;

            return visitedFeed;
        }



        //One seed to rule them all
        public void TherulingSeed()
        {
            SeedingUsers();
            SeedingCircles();
            SeedingPosts();
            SeedingFollowlist();
            SeedingComment();
        }


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

        public object SeedingPosts()
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

            return CreatedAtRoute("GetUser", new { id = p1.Id.ToString() }, p1);
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