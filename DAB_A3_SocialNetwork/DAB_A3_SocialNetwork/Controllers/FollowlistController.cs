using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var circle = _databaseServices.GetCircle(id);

            if (circle == null || user == null)
            {
                return NotFound();
            }

            CirclesController.Circleposts circlefeed = new CirclesController.Circleposts();
            circlefeed.circle = circle;
            circlefeed.circleposts = posts;

            return circlefeed;
        }
    }
}