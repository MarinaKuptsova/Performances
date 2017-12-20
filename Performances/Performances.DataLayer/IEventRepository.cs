using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Performances.Model;

namespace Perfomances.DataLayer
{
    public interface IEventRepository
    {
        Event CreateEvent(Event newEvent, byte[] photo, string filename, Guid creativeTeamId);
        Event GetEventById(Guid eventId);
        void DeleteEvent(Event delEvent);
        void DeleteEvent(Guid eventId);
        Event UpdateEvent(Event newEvent);
        List<Event> GetAllEvents();
        List<Event> GetCreativeTeamEvents(CreativeTeam creativeTeam);
        List<Event> GetUserSubscribedEvents(User user);
        List<Event> GetUserUpcomingEvents(User user);
        List<Event> GetNearestEvents(User user);
    }
}
