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
            public List<Posts> circleposts;
            public List<Posts> followingposts;
        }

        //Shows the users in the database
        [HttpGet]
        public ActionResult<List<Users>> Get() => _databaseServices.GetUsers();

        //Shows a specific user and their own posts (for now)
        [HttpGet("{id:length(24)}", Name = "GetUserFeed")]
        [ActionName("Getmyfeed")]
        public ActionResult<object> Get(string id)
        {
            var user = _databaseServices.GetUsers(id);
            var posts = _databaseServices.GetMyPosts(id);
            var circleposts = _databaseServices.GetCirclePosts()

            if (user == null || posts == null)
            {
                return NotFound();
            }

            Userfeed userfeed = new Userfeed();
            userfeed.user = user;
            userfeed.userposts = posts;

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

        //Get where you visit someone else
        //Own id first, visit id second
        [HttpGet("{ownid:length(24)}/{visitid:length(24)}", Name = "GetUserVisit")]
        public ActionResult<Users> Get(string ownid, string visitid)
        {
            var own = _databaseServices.GetUsers(ownid);
            var visit = _databaseServices.GetUsers(visitid);

            if (own == null || visit == null)
            {
                return NotFound();
            }

            return visit;
        }

    }
}