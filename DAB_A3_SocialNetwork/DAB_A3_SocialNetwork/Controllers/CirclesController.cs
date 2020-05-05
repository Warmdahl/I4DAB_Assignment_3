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
    public class CirclesController : ControllerBase
    {
        private readonly DatabaseServices _databaseServices;

        public CirclesController(DatabaseServices databaseServices)
        {
            _databaseServices = databaseServices;
        }

        //see All circles which are available
        [HttpGet]
        public ActionResult<List<Circles>> Get() => _databaseServices.GetCircles();

        //Creates a new circle
        [HttpPost]
        public ActionResult<Circles> Create(Circles circle)
        {
            _databaseServices.CreateCircle(circle);

            return CreatedAtRoute("GetCircle", new { id = circle.Id.ToString() },circle);
        }

        struct Circleposts
        {
            public Circles circle;
            public List<Posts> circleposts;
        }

        //Shows a specific circle and all the posts in the cirlce
        [HttpGet("{id:length(24)}", Name = "GetCircleFeed")]
        public ActionResult<object> Get(string id)
        {
            var circle = _databaseServices.GetCircle(id);
            var posts = _databaseServices.GetCirclePosts(id);

            if (circle == null || posts == null)
            {
                return NotFound();
            }

            Circleposts circlefeed = new Circleposts();
            circlefeed.circle = circle;
            circlefeed.circleposts = posts;

            return circlefeed;
        }
    }
}