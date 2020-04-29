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
    public class PostsController : ControllerBase
    {
        private readonly DatabaseServices _databaseServices;

        public PostsController(DatabaseServices databaseServices)
        {
            _databaseServices = databaseServices;
        }

        [HttpGet]
        public ActionResult<List<Posts>> Get() => _databaseServices.GetPosts();
    }
}