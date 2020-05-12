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
    public class CommentController : ControllerBase
    {
        private readonly DatabaseServices _databaseServices;

        public CommentController(DatabaseServices databaseServices)
        {
            _databaseServices = databaseServices;
        }


        struct commentfeed
        {
            public List<Posts> orgpost;
            public List<Comments> comments;
        }

        //Show all the comments to a specific post (enter post id)
        [HttpGet("{id:length(24)}", Name = "GetCommentFeed")]
        public ActionResult<object> Get(string id)
        {
            var orgpost = _databaseServices.GetPostbyPostid(id);

            if (orgpost == null)
            {
                return NotFound();
            }

            var comments = _databaseServices.GetComments(id);

            commentfeed feed = new commentfeed();
            feed.orgpost = orgpost;
            feed.comments = comments;
            return feed;
        }

        //Creates a new comment
        [HttpPost]
        public ActionResult<Comments> Create(Comments comment)
        {
            _databaseServices.CreateComments(comment);

            return CreatedAtRoute("GetUser", new { id = comment.Id.ToString() }, comment);
        }
    }
}