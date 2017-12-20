using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using Performances.Model;
using Perfomances.DataLayer;
using Performances.DataLayer.PostgreSQL.Helpers;

namespace Performances.DataLayer.PostgreSQL
{
    public class EventRepository : IEventRepository
    {
        private readonly string _connectionString; // = "Server=localhost;Port=5432;User Id=client;Password=passclient;Database=testdb;Search Path=test;";

        public EventRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Event CreateEvent(Event newEvent, byte[] photo, string filename, Guid creativeTeamId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (NpgsqlException ex)
                {
                    throw ex;
                }
                
                using (var tran = connection.BeginTransaction())
                {
                    File file = new File();
                    file.Id = Guid.NewGuid();
                    file.Bytes = photo;
                    file.Filename = filename;
                    file.FileExtension = filename.Split('.').Last();
                    newEvent.Id = Guid.NewGuid();
                    newEvent.Photo = file.Id;

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText = 
                            "insert into files (id, filename, fileextension, bytes)" +
                            "values (@id, @filename, @fileextension, @bytes)";
                        command.Parameters.AddWithValue("@id", file.Id);
                        command.Parameters.AddWithValue("@filename", file.Filename);
                        command.Parameters.AddWithValue("@fileextension", file.FileExtension);
                        command.Parameters.AddWithValue("@bytes", file.Bytes);
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText =
                            "insert into event (id, place, participantcount, description, photo, datetime)"
                            + " values (@id, @place, @participantcount, @description, @photo, @datetime)";
                        command.Parameters.AddWithValue("@id", newEvent.Id);
                        command.Parameters.AddWithValue("@place", newEvent.Place);
                        command.Parameters.AddWithValue("@participantcount", newEvent.ParticipantCount);
                        command.Parameters.AddWithValue("@description", newEvent.Description);
                        command.Parameters.AddWithValue("@photo", newEvent.Photo);
                        command.Parameters.AddWithValue("@datetime", new NpgsqlDateTime(newEvent.DateAndTime));
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText = "insert into creativeteamevent (creativeteamid, eventid)" +
                                              "values (@creativeteamid, @eventid)";
                        command.Parameters.AddWithValue("@creativeteamid", creativeTeamId);
                        command.Parameters.AddWithValue("@eventid", newEvent.Id);
                        command.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }

            return newEvent;
        }

        public void DeleteEvent(Event delEvent)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (NpgsqlException ex)
                {
                    throw ex;
                }

                using (var tran = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText = "update event set photo=null where id=@id";
                        command.Parameters.AddWithValue("@id", delEvent.Id);
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText =
                            "delete from creativeteamevent where eventid=@id";
                        command.Parameters.AddWithValue("@id", delEvent.Id);
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText =
                            "delete from userevent where eventid=@id";
                        command.Parameters.AddWithValue("@id", delEvent.Id);
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText =
                            "delete from files where id=@photoid";
                        command.Parameters.AddWithValue("@photoid", delEvent.Photo);
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText =
                            "delete from event where id=@id";
                        command.Parameters.AddWithValue("@id", delEvent.Id);
                        command.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }
        }

        public void DeleteEvent(Guid eventId)
        {
            var delEvent = GetEventById(eventId);
            DeleteEvent(delEvent);
        }

        public List<Event> GetAllEvents()
        {
            List<Event> allEvents = new List<Event>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from event";
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                Event curEvent = new Event();
                                curEvent.Id = reader.GetGuid(reader.GetOrdinal("id"));
                                curEvent.Description = reader.GetString(reader.GetOrdinal("description"));
                                curEvent.DateAndTime = reader.GetDateTime(reader.GetOrdinal("datetime"));
                                curEvent.ParticipantCount = reader.GetInt32(reader.GetOrdinal("participantcount"));
                                curEvent.Photo = reader.GetGuid(reader.GetOrdinal("photo"));
                                curEvent.Place = reader.GetString(reader.GetOrdinal("place"));
                                allEvents.Add(curEvent);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return allEvents;
        }

        public List<Event> GetCreativeTeamEvents(CreativeTeam creativeTeam)
        {
            List<Event> creativeTeamEvents = new List<Event>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select e.id, e.datetime, e.photo, e.description, e.participantcount, e.place " +
                                          "FROM event e " +
                                          "join creativeteamevent cte ON e.id = cte.eventid " +
                                          "join creativeteam ct ON cte.creativeteamid = ct.id " +
                                          "where ct.id = @creativeteamid";
                    command.Parameters.AddWithValue("@creativeteamid", creativeTeam.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                Event curEvent = new Event();
                                curEvent.Id = reader.GetGuid(reader.GetOrdinal("id"));
                                curEvent.Description = reader.GetString(reader.GetOrdinal("description"));
                                curEvent.DateAndTime = reader.GetDateTime(reader.GetOrdinal("datetime"));
                                curEvent.ParticipantCount = reader.GetInt32(reader.GetOrdinal("participantcount"));
                                curEvent.Photo = reader.GetGuid(reader.GetOrdinal("photo"));
                                curEvent.Place = reader.GetString(reader.GetOrdinal("place"));
                                creativeTeamEvents.Add(curEvent);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return creativeTeamEvents;
        }

        public Event GetEventById(Guid eventId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from event where id = @EventId";
                    command.Parameters.AddWithValue("@EventId", eventId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException($"Событие с Id {eventId} не нашлось");
                        var newEvent = new Event()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            ParticipantCount = reader.GetInt32(reader.GetOrdinal("participantcount")),
                            Place = reader.GetString(reader.GetOrdinal("place")),
                            DateAndTime = reader.GetDateTime(reader.GetOrdinal("datetime")),
                            Photo = reader.GetGuid(reader.GetOrdinal("photo"))
                        };

                        return newEvent;
                    }
                }
            }
        }

        public List<Event> GetNearestEvents(User user)
        {
            // TODO: Это вряд ли напишем, нужна сложная логика, простая - бесполезно
            return null;
        }

        public List<Event> GetUserSubscribedEvents(User user)
        {
            List<Event> userEvents = new List<Event>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select e.id, e.place, e.participantcount, e.description, e.datetime, e.photo " +
                                          "from users u join subscribes s on u.id = s.userid " +
                                          "join creativeteam ct on s.creativeteamid = ct.id " +
                                          "join creativeteamevent cte on ct.id = cte.creativeteamid " +
                                          "join event e on cte.eventid = e.id " +
                                          "where u.id=@userid and e.datetime >= current_date";
                    command.Parameters.AddWithValue("@userid", user.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                Event curEvent = new Event();
                                curEvent.Id = reader.GetGuid(reader.GetOrdinal("id"));
                                curEvent.Description = reader.GetString(reader.GetOrdinal("description"));
                                curEvent.DateAndTime = reader.GetDateTime(reader.GetOrdinal("datetime"));
                                curEvent.ParticipantCount = reader.GetInt32(reader.GetOrdinal("participantcount"));
                                curEvent.Photo = reader.GetGuid(reader.GetOrdinal("photo"));
                                curEvent.Place = reader.GetString(reader.GetOrdinal("place"));
                                userEvents.Add(curEvent);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return userEvents;
        }

        public List<Event> GetUserUpcomingEvents(User user)
        {
            List<Event> userUpcomingEvents = new List<Event>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select e.id, e.place, e.participantcount, e.description, e.datetime, e.photo " +
                                          "from test.users u " +
                                          "join test.userevent on u.id = userevent.userid " +
                                          "join test.event e on userevent.eventid = e.id " +
                                          "where u.id = @userid and e.datetime >= current_date";
                    command.Parameters.AddWithValue("@userid", user.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                Event curEvent = new Event();
                                curEvent.Id = reader.GetGuid(reader.GetOrdinal("id"));
                                curEvent.Description = reader.GetString(reader.GetOrdinal("description"));
                                curEvent.DateAndTime = reader.GetDateTime(reader.GetOrdinal("datetime"));
                                curEvent.ParticipantCount = reader.GetInt32(reader.GetOrdinal("participantcount"));
                                curEvent.Photo = reader.GetGuid(reader.GetOrdinal("photo"));
                                curEvent.Place = reader.GetString(reader.GetOrdinal("place"));
                                userUpcomingEvents.Add(curEvent);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return userUpcomingEvents;
        }

        public Event UpdateEvent(Event newEvent)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update event set place = @place, " +
                                          "participantcount = @participantcount, " +
                                          "description = @description, " +
                                          "datetime = @datetime, " +
                                          "photo = @photo " +
                                          "WHERE id = @eventid";
                    command.Parameters.AddWithValue("@eventId", newEvent.Id);
                    command.Parameters.AddWithValue("@place", newEvent.Place);
                    command.Parameters.AddWithValue("@participantcount", newEvent.ParticipantCount);
                    command.Parameters.AddWithValue("@description", newEvent.Description);
                    command.Parameters.AddWithValue("@datetime", newEvent.DateAndTime);
                    command.Parameters.AddWithValue("@photo", newEvent.Photo);
                    command.ExecuteNonQuery();
                }
            }
            return newEvent;
        }
    }
}
