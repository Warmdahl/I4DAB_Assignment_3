﻿using System;
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
        public ActionResult<List<Users>> Get() => _databaseServices.GetUsers();

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
                circleposts = _databaseServices.GetCirclePosts(circle.Id);
            }

            //var following = user.followerslist;
            //var followingsposts = new List<Posts>();
            //foreach (var follow in following.followingIDs)
            //{
            //    followingsposts = _databaseServices.GetMyPosts(follow);
            //}

            if (user == null || posts == null)                                      //Checker om user findes hvis ikke sendes 404
            {
                return NotFound();
            }

            Userfeed userfeed = new Userfeed();                                     //Struct som indeholder alt information som vises på siden.
            userfeed.user = user;
            userfeed.userposts = posts;
            userfeed.circles = circles;
            userfeed.circleposts = circleposts;
            //userfeed.followingposts = followingsposts;

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