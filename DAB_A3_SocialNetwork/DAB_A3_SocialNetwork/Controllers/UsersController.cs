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
    public class UsersController : ControllerBase
    {
        private readonly DatabaseServices _databaseServices;

        public UsersController(DatabaseServices databaseServices)
        {
            _databaseServices = databaseServices;
        }

        [HttpGet]
        public ActionResult<List<Users>> Get() => _databaseServices.GetUsers();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        [ActionName("Getmyfeed")]
        public ActionResult<List<Posts>> Get(string id)
        {
            var user = _databaseServices.GetUsers(id);
            var posts = _databaseServices.GetMyPosts(id);

            if (user == null || posts == null)
            {
                return NotFound();
            }

            return posts;
        }

        [HttpPost]
        public ActionResult<Users> Create(Users users)
        {
            _databaseServices.CreateUser(users);

            return CreatedAtRoute("GetUser", new {id = users.Id.ToString()}, users);
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