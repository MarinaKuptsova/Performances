using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Performances.Model;

namespace Performances.DataLayer.PostgreSQL.Tests
{
    [TestClass]
    public class EventRepositoryTest
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;User Id=client;Password=passclient;Database=testdb;Search Path=test;";
        public List<Guid> _tempCreativeTeam = new List<Guid>();
        public List<Guid> _tempEvent = new List<Guid>();
        public List<Guid> _tempUsers = new List<Guid>();

        [TestMethod]
        public void ShouldCreateEvent()
        {
            var creativeteam = new CreativeTeam
            {
                About = "test",
                Email = "test@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "testband",
                Password = "test",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "test.jpeg"
            };

            var newEvent = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "Saint-Petersburg, Russia"
            };
            var file = new File
            {
                Bytes = new byte[] { 1, 12, 123, 13 },
                FileExtension = ".jpg",
                Filename = "fotka"
            };
            
            var creativeteamRepository = new CreativeTeamRepository(_connectionString);
            var newcreativeteam = creativeteamRepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeTeam.Add(newcreativeteam.Id);
            
            var eventrepository = new EventRepository(_connectionString);
            var result = eventrepository.CreateEvent(newEvent, file.Bytes, file.Filename, newcreativeteam.Id);
            _tempEvent.Add(result.Id);

            Assert.AreEqual(newEvent.Place, result.Place);
            Assert.AreEqual(newEvent.ParticipantCount, result.ParticipantCount);
            Assert.AreEqual(newEvent.DateAndTime.Date, result.DateAndTime.Date);
        }

        [TestMethod]
        public void ShouldGetAllEvents()
        {
            var creativeteam = new CreativeTeam
            {
                About = "test",
                Email = "test@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "testband",
                Password = "test",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "test.jpeg"
            };

            var creativeteam2 = new CreativeTeam
            {
                About = "test2",
                Email = "test2@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "testband2",
                Password = "test2",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo2 = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "test.jpeg"
            };

            var newEvent = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "Saint-Petersburg, Russia"
            };
            var newEvent2 = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "SPb, Russia"
            };
            var file = new File
            {
                Bytes = new byte[] { 1, 12, 123, 13 },
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var creativeteamRepository = new CreativeTeamRepository(_connectionString);
            var newcreativeteam = creativeteamRepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeTeam.Add(newcreativeteam.Id);
            var newcreativeteam2 = creativeteamRepository.CreateCreativeTeam(creativeteam2, photo2.Bytes, photo2.Filename);
            _tempCreativeTeam.Add(newcreativeteam2.Id);

            var eventrepository = new EventRepository(_connectionString);
            var result = eventrepository.CreateEvent(newEvent, file.Bytes, file.Filename, newcreativeteam.Id);
            var result2 = eventrepository.CreateEvent(newEvent2, file.Bytes, file.Filename, newcreativeteam2.Id);
            _tempEvent.Add(result.Id);
            _tempEvent.Add(result2.Id);

            Assert.AreEqual(newEvent.Place, result.Place);
            Assert.AreEqual(newEvent2.Place, result2.Place);
        }

        [TestMethod]
        public void ShouldGetCreativeTeamEvents()
        {
            var creativeteam = new CreativeTeam
            {
                About = "test",
                Email = "test@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "testband",
                Password = "test",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "test.jpeg"
            };
            var newEvent = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "Saint-Petersburg, Russia"
            };
            var newEvent2 = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "SPb, Russia"
            };
            var file = new File
            {
                Bytes = new byte[] { 1, 12, 123, 13 },
                FileExtension = ".jpg",
                Filename = "fotka"
            };
            
            var creativeteamRepository = new CreativeTeamRepository(_connectionString);
            var newcreativeteam = creativeteamRepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeTeam.Add(newcreativeteam.Id);

            var eventrepository = new EventRepository(_connectionString);
            var result = eventrepository.CreateEvent(newEvent, file.Bytes, file.Filename, newcreativeteam.Id);
            var result2 = eventrepository.CreateEvent(newEvent2, file.Bytes, file.Filename, newcreativeteam.Id);
            _tempEvent.Add(result.Id);
            _tempEvent.Add(result2.Id);

            List<Event> events = new List<Event>();
            events = eventrepository.GetCreativeTeamEvents(newcreativeteam);

            var resultsevents = events.Find(x => x.Id == result.Id);
            Assert.AreEqual(result.Place, resultsevents.Place);

            resultsevents = events.Find(x => x.Id == result2.Id);
            Assert.AreEqual(result2.Photo, resultsevents.Photo);
        }

        [TestMethod]
        public void ShouldGetEventById()
        {
            var creativeteam = new CreativeTeam
            {
                About = "test",
                Email = "test@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "testband",
                Password = "test",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "test.jpeg"
            };
            var newEvent = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "Saint-Petersburg, Russia"
            };
            var file = new File
            {
                Bytes = new byte[] { 1, 12, 123, 13 },
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var creativeteamRepository = new CreativeTeamRepository(_connectionString);
            var newcreativeteam = creativeteamRepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeTeam.Add(newcreativeteam.Id);

            var eventrepository = new EventRepository(_connectionString);
            var result = eventrepository.CreateEvent(newEvent, file.Bytes, file.Filename, newcreativeteam.Id);
            _tempEvent.Add(result.Id);

            var resultEvent = eventrepository.GetEventById(result.Id);

            Assert.AreEqual(newEvent.Place, resultEvent.Place);
            Assert.AreEqual(newEvent.Description, resultEvent.Description);
        }
        
        [TestMethod]
        public void ShouldGetUserSubscribedEvents()
        {
            var user = new User
            {
                Age = 18,
                City = "Москва",
                Email = "text@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };
            var creativeteam = new CreativeTeam
            {
                About = "test",
                Email = "test@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "testband",
                Password = "test",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "test.jpeg"
            };
            var newEvent = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "Saint-Petersburg, Russia1"
            };
            var file = new File
            {
                Bytes = new byte[] { 1, 12, 123, 13 },
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var userRepository = new UserRepository(_connectionString);
            var eventRepository = new EventRepository(_connectionString);
            var creativeteamRepository = new CreativeTeamRepository(_connectionString);

            var newUser = userRepository.CreateUser(user, file);
            _tempUsers.Add(newUser.Id);
            var newCreativeteam = creativeteamRepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeTeam.Add(newCreativeteam.Id);
            userRepository.Subscribe(newUser.Id, newCreativeteam.Id);

            var teamEvent = eventRepository.CreateEvent(newEvent, photo.Bytes, photo.Filename, newCreativeteam.Id);
            _tempEvent.Add(teamEvent.Id);

            var subscribedEvents = new List<Event>();
            subscribedEvents = eventRepository.GetUserSubscribedEvents(newUser);
            var result = subscribedEvents.Find(x => x.Id == teamEvent.Id);

            Assert.AreEqual(teamEvent.Place, result.Place);
        }

        [TestMethod]
        public void ShoulGetUserUpcomingEvents()
        {
            var user = new User
            {
                Age = 18,
                City = "Москва",
                Email = "text@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };
            var creativeteam = new CreativeTeam
            {
                About = "test",
                Email = "test@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "testband",
                Password = "test",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "test.jpeg"
            };
            var newEvent = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "Saint-Petersburg, Russia1"
            };
            var newEvent2 = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert!!!!!",
                ParticipantCount = 0,
                Place = "Saint-Petersburg, Russia1"
            };
            var file = new File
            {
                Bytes = new byte[] { 1, 12, 123, 13 },
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var userRepository = new UserRepository(_connectionString);
            var eventRepository = new EventRepository(_connectionString);
            var creativeteamRepository = new CreativeTeamRepository(_connectionString);

            var newUser = userRepository.CreateUser(user, file);
            _tempUsers.Add(newUser.Id);
            var newCreativeteam = creativeteamRepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeTeam.Add(newCreativeteam.Id);
            var teamEvent = eventRepository.CreateEvent(newEvent, photo.Bytes, photo.Filename, newCreativeteam.Id);
            var teamEvent2 = eventRepository.CreateEvent(newEvent2, photo.Bytes, photo.Filename, newCreativeteam.Id);
            _tempEvent.Add(teamEvent.Id);
            _tempEvent.Add(teamEvent2.Id);
            userRepository.GoToEvent(newUser.Id, teamEvent2.Id);

            var usersEvents = new List<Event>();
            usersEvents = eventRepository.GetUserUpcomingEvents(newUser);
            var result = usersEvents.Find(x => x.Id == teamEvent2.Id);

            Assert.AreEqual(teamEvent2.Description, result.Description);

        }

        [TestMethod]
        public void ShouldUpdateEvent()
        {
            var creativeteam = new CreativeTeam
            {
                About = "test",
                Email = "test@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "testband",
                Password = "test",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "test.jpeg"
            };

            var newEvent = new Event
            {
                DateAndTime = DateTime.Now,
                Description = "Big concert on the street of your city!",
                ParticipantCount = 0,
                Place = "Saint-Petersburg, Russia"
            };
            var file = new File
            {
                Bytes = new byte[] { 1, 12, 123, 13 },
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var creativeteamRepository = new CreativeTeamRepository(_connectionString);
            var newcreativeteam = creativeteamRepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeTeam.Add(newcreativeteam.Id);

            var eventrepository = new EventRepository(_connectionString);
            var teamEvent = eventrepository.CreateEvent(newEvent, file.Bytes, file.Filename, newcreativeteam.Id);
            _tempEvent.Add(teamEvent.Id);

            teamEvent.Description = "Big concert!!!!!!!!!!!!!!!!!";

            var result = eventrepository.UpdateEvent(teamEvent);

            Assert.AreEqual(teamEvent.Description, result.Description);
        }



        [TestCleanup]
        public void Clean()
        {
            foreach (var id in _tempEvent)
                new EventRepository(_connectionString).DeleteEvent(id);
            foreach (var id in _tempCreativeTeam)
                new CreativeTeamRepository(_connectionString).DeleteCreativeTeam(id);
            foreach (var id in _tempUsers)
                new UserRepository(_connectionString).DeleteUser(id);
        }
    }
}
