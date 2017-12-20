using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using Performances.Api.Models;
using Performances.DataLayer;
using Performances.DataLayer.PostgreSQL;
using Performances.Model;

namespace Performances.Api.Controllers
{
    public class CreativeTeamController : ApiController
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;User Id=client;Password=passclient;Database=testdb;Search Path=test;";
        private readonly ICreativeTeamRepository _creativeTeamRepository;

        public CreativeTeamController()
        {
            _creativeTeamRepository = new CreativeTeamRepository(_connectionString);
        }

        [HttpPost]
        [Route("api/creativeteam")]
        public CreativeTeam Create([FromBody] CreateCreativeTeamParameters parameters)
        {
            CreativeTeam creativeTeam = parameters.creativeTeam;
            var photo = parameters.photo;
            var filename = parameters.filename;
            return _creativeTeamRepository.CreateCreativeTeam(creativeTeam, photo, filename);
        }

        [HttpGet]
        [Route("api/creativeteam/{id}")]
        public CreativeTeam GetById(Guid id)
        {
            return _creativeTeamRepository.GetCreativeTeamById(id);
        }

        [HttpPut]
        [Route("api/creativeteam")]
        public CreativeTeam Update([FromBody] CreativeTeam newCreativeTeam)
        {
            return _creativeTeamRepository.UpdateCreativeTeam(newCreativeTeam);
        }

        [HttpDelete]
        [Route("api/creativeteam/{id}")]
        public void Delete(Guid id)
        {
            _creativeTeamRepository.DeleteCreativeTeam(id);
        }
    }
}