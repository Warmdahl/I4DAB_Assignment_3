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

            if (users == null)                                          //
            {

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

    }
}