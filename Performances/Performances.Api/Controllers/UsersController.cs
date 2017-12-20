using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Perfomances.DataLayer;
using Performances.Api.Models;
using Performances.DataLayer.PostgreSQL;
using Performances.Model;

namespace Performances.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly string _connectionString = "Server=localhost;Port=5432;User Id=client;Password=passclient;Database=testdb;Search Path=test;";

        public UsersController()
        {
            _userRepository = new UserRepository(_connectionString);
        }

        [HttpPost]
        [Route("api/user")]
        public User Create([FromBody] CreateUserParameters userParameters)
        {
            File photo = userParameters.photo;
            User user = userParameters.user;
            return _userRepository.CreateUser(user, photo);
        }

        [HttpPut]
        [Route("api/user")]
        public User Update([FromBody] User user)
        {
            return _userRepository.UpdateUser(user);
        }

        [HttpGet]
        [Route("api/user")]
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        [HttpGet]
        [Route("api/user/{id}")]
        public User GetUserById(Guid id)
        {
            return _userRepository.GetUserById(id);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        [HttpGet]
        [Route("api/user/{email}/{password}")]
        public User Login(string email, string password)
        {
            return _userRepository.Login(email, password);
        }

        /// <summary>
        /// Subscribe
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="creativeteamId">Creative team id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/{userId}/creativeteam/{creativeteamId}")]
        public void Subscribe(Guid userId, Guid creativeteamId)
        {
            _userRepository.Subscribe(userId, creativeteamId);
        }

        /// <summary>
        /// Go to event
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        [HttpPost]
        [Route("api/user/{userId}/event/{eventId}")]
        public void GoToEvent(Guid userId, Guid eventId)
        {
            _userRepository.GoToEvent(userId, eventId);
        }

        [HttpDelete]
        [Route("api/user/{id}")]
        public void Delete(Guid id)
        {
            _userRepository.DeleteUser(id);
        }
    }
}