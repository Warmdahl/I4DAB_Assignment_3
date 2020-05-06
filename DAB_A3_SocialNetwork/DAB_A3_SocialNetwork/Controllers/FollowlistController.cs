using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_A3_SocialNetwork.Models;
using DAB_A3_SocialNetwork.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAB_A3_SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowlistController : ControllerBase
    {
        private readonly DatabaseServices _databaseServices;

        public FollowlistController(DatabaseServices databaseServices)
        {
            _databaseServices = databaseServices;
        }

        //Shows the specific followinglist for a user
        [HttpGet("{id:length(24)}", Name = "GetFollowFeed")]
        public ActionResult<object> Get(string id)
        {
            var user = _databaseServices.GetUsers(id);
            var followlist = _databaseServices.GetUserFollowlists(id);                //får en followlist til brugeren
            
            if (followlist == null || user == null)
            {
                return NotFound();
            }

            List<Posts> posts = new List<Posts>();
            
            foreach (var post in followlist.followingIDs)
            {
                var ID = post;                                                  //Laver en liste med alle posts fra alle på followlisten
                var UsersPosts = _databaseServices.GetMyPosts(ID);
                foreach (var p in UsersPosts)
                {
                    posts.Add(p);
                }
            }

            return posts;
        }


        //Creates a followlist for a user
        [HttpPost]
        public ActionResult<Users> Create(Followlist followlist)
        {
            _databaseServices.CreateFollowlist(followlist);

            return CreatedAtRoute("GetUser", new { id = followlist.FLOwnerID.ToString() }, followlist);
        }
    }
}