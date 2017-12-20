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
    public class EventsController : ApiController
    {
        private readonly IEventRepository _eventsRepository;
        private readonly string _connectionString = "Server=localhost;Port=5432;User Id=client;Password=passclient;Database=testdb;Search Path=test;";

        public EventsController()
        {
            _eventsRepository = new EventRepository(_connectionString);
        }

        [HttpPost]
        [Route("api/event")]
        public Event CreateEvent([FromBody] CreateEventParameters eventParameters)
        {
            Event newEvent = eventParameters.newEvent;
            byte[] photo = eventParameters.photo;
            var filename = eventParameters.filename;
            var creativeTeamId = eventParameters.creativeTeamId;
            return _eventsRepository.CreateEvent(newEvent, photo, filename, creativeTeamId);
        }

        [HttpGet]
        [Route("api/event")]
        public List<Event> GetAllEvents()
        {
            return _eventsRepository.GetAllEvents();
        }

        [HttpGet]
        [Route("api/event/creativeteam")]
        public List<Event> GetCreativeTeamEvents([FromBody]CreativeTeam creativeteam)
        {
            return _eventsRepository.GetCreativeTeamEvents(creativeteam);
        }

        //[HttpGet]
        //[Route("api/event/subscribedevents")]
        //public List<Event> GetUserSubscribedEvents([FromBody] User user)
        //{
        //    return _eventsRepository.GetUserSubscribedEvents(user);
        //}

        [HttpGet]
        [Route("api/users/events")]
        public List<Event> GetUserUpcomingEvent([FromBody] User user)
        {
            return _eventsRepository.GetUserUpcomingEvents(user);
        }

        [HttpPut]
        [Route("api/event")]
        public Event Update([FromBody] Event newEvent)
        {
            return _eventsRepository.UpdateEvent(newEvent);
        }

        [HttpDelete]
        [Route("api/event/{id}")]
        public void Delete(Guid id)
        {
            _eventsRepository.DeleteEvent(id);
        }

       
    }
}