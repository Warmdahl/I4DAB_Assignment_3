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
        public ActionResult<Users> Get(string id)
        {
            var user = _databaseServices.GetUsers(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<Users> Create(Users users)
        {
            _databaseServices.CreateUser(users);

            return CreatedAtRoute("GetUser", new {id = users.Id.ToString()}, users);
        }
    }
}