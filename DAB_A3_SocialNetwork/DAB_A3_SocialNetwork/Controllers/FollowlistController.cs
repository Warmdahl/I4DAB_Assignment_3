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
            //var circle = _databaseServices.GetCircle(id);
            var followlist = _databaseServices.GetUserFollowlists(id); //får en followlist til brugeren
            
            
            
            if (followlist == null || user == null)
            {
                return NotFound();
            }

            List<Posts> posts = new List<Posts>();
            
            foreach (var post in followlist.followingIDs)
            {
                var ID = followlist.followingIDs[1];                        //Laver en liste med alle posts fra alle på followlisten
                var UsersPosts = _databaseServices.GetMyPosts(ID);
                foreach (var p in UsersPosts)
                {
                    posts.Add(p);
                }
            }
            
            /*CirclesController.Circleposts circlefeed = new CirclesController.Circleposts();
            circlefeed.circle = circle;
            circlefeed.circleposts = posts;*/

            return posts;
        }
    }
}