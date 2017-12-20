using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Performances.Model;
using Performances.DataLayer.PostgreSQL;

namespace Performances.DataLayer.PostgreSQL.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;User Id=client;Password=passclient;Database=testdb;Search Path=test;";
        public List<Guid> _tempUsers = new List<Guid>();
        public List<Guid> _tempEvents = new List<Guid>();
        public List<Guid> _tempCreativeTeam = new List<Guid>();

        [TestMethod]
        public void ShouldCreateNewUser()
        {
            var user = new User();
            var file = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            user.Age = 18;
            user.City = "Питер";
            user.Email = "text@yandex.ru";
            user.Name = "Марина";
            user.Surname = "Купцова";
            user.Password = "12345";
            
            

            var usersRepository = new UserRepository(_connectionString);
            var result = usersRepository.CreateUser(user, file);
            _tempUsers.Add(result.Id);

            Assert.AreEqual(user.Name, result.Name);
            Assert.AreEqual(user.Surname, result.Surname);
        }

        [TestMethod]
        public void ShouldGetAllUsers()
        {
            var user1 = new User
            {
                Age = 18,
                City = "Москва",
                Email = "text@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };

            var file1 = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka1"
            };
            var user2 = new User
            {
                Age = 18,
                City = "Санкт-Петербург",
                Email = "text1@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };

            var file2 = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka2"
            };
            var user3 = new User
            {
                Age = 18,
                City = "Питер",
                Email = "text2@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };

            var file3 = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka3"
            };

            var usersRepository = new UserRepository(_connectionString);
            var newuser1 = usersRepository.CreateUser(user1, file1);
            var newuser2 = usersRepository.CreateUser(user2, file2);
            var newuser3 = usersRepository.CreateUser(user3, file3);
            _tempUsers.Add(newuser1.Id);
            _tempUsers.Add(newuser2.Id);
            _tempUsers.Add(newuser3.Id);

            List<User> allusers = new List<User>();
            allusers = usersRepository.GetAllUsers();

            var result = allusers.Find(x => x.Id == newuser1.Id);
            Assert.AreEqual(user1.Name, result.Name);

            result = allusers.Find(x => x.Id == newuser2.Id);
            Assert.AreEqual(user2.Email, result.Email);

            result = allusers.Find(x => x.Id == newuser3.Id);
            Assert.AreEqual(user3.Photo, result.Photo);
        }

        [TestMethod]
        public void ShouldGetUserById()
        {
            var user = new User
            {
                Age = 18,
                City = "Питер",
                Email = "text@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };

            var file = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var repository = new UserRepository(_connectionString);
            var resultUser = repository.CreateUser(user, file);
            _tempUsers.Add(resultUser.Id);

            var result = repository.GetUserById(resultUser.Id);

            Assert.AreEqual(user.City, result.City);
            Assert.AreEqual(user.Email, result.Email);
        }

        [TestMethod]
        public void ShouldGoToEvent()
        {
            var user = new User
            {
                Age = 18,
                City = "Питер",
                Email = "text@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };

            var file = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var creativeteam = new CreativeTeam();
            creativeteam.About = "test";
            creativeteam.Email = "test@ya.ru";
            creativeteam.Genre = "rock/hardcore/post-punk";
            creativeteam.Name = "testband";
            creativeteam.Password = "test";
            creativeteam.Rating = 3.2;
            creativeteam.SubscribersCount = 0;

            var teamevent = new Event
            {
                Place = "СПб",
                DateAndTime = DateTime.Now,
                Description = "",
                ParticipantCount = 13
            };

            var Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var FileExtension = ".jpeg";
            var Filename = "test.jpeg";

            var usersRepository = new UserRepository(_connectionString);
            var eventRepository = new EventRepository(_connectionString);
            var creativeteamRepository = new CreativeTeamRepository(_connectionString);

            var newcreativeteam = creativeteamRepository.CreateCreativeTeam(creativeteam, Bytes, Filename);
            _tempCreativeTeam.Add(newcreativeteam.Id);
            var newuser = usersRepository.CreateUser(user, file);
            _tempUsers.Add(newuser.Id);
            var newevent = eventRepository.CreateEvent(teamevent, Bytes, Filename, newcreativeteam.Id);
            _tempEvents.Add(newevent.Id);
            usersRepository.GoToEvent(newuser.Id, newevent.Id);
        }

        [TestMethod]
        public void Subscribe()
        {
            var user = new User
            {
                Age = 18,
                City = "Питер",
                Email = "text@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };

            var file = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var Filename = "test.jpeg";

            var creativeteam = new CreativeTeam();
            creativeteam.About = "test";
            creativeteam.Email = "test@ya.ru";
            creativeteam.Genre = "rock/hardcore/post-punk";
            creativeteam.Name = "testband";
            creativeteam.Password = "test";
            creativeteam.Rating = 3.2;
            creativeteam.SubscribersCount = 0;

            var usersRepository = new UserRepository(_connectionString);
            var creativeteamRepository = new CreativeTeamRepository(_connectionString);

            var newUser = usersRepository.CreateUser(user, file);
            _tempUsers.Add(newUser.Id);
            var team = creativeteamRepository.CreateCreativeTeam(creativeteam, Bytes, Filename);
            _tempCreativeTeam.Add(team.Id);

            usersRepository.Subscribe(newUser.Id, team.Id);
        }

        [TestMethod]
        public void ShouldUpdateUser()
        {
            var user = new User
            {
                Age = 18,
                City = "Питер",
                Email = "text@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "12345",
            };

            var file = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var userRepository = new UserRepository(_connectionString);
            var result = userRepository.CreateUser(user, file);
            _tempUsers.Add(result.Id);

            result.City = "Москва";
            var updatedUser = userRepository.UpdateUser(result);

            Assert.AreEqual(result.City, updatedUser.City);
        }

        [TestMethod]
        public void ShouldLoginUser()
        {
            var user = new User
            {
                Age = 18,
                City = "Питер",
                Email = "text@yandex.ru",
                Name = "Марина",
                Surname = "Купцова",
                Password = "123"
            };

            var file = new File
            {
                Bytes = new byte[0],
                FileExtension = ".jpg",
                Filename = "fotka"
            };

            var userRepository = new UserRepository(_connectionString);
            var result = userRepository.CreateUser(user, file);
            _tempUsers.Add(result.Id);

            var loginUser = userRepository.Login(result.Email, result.Password);

            Assert.AreEqual(result.Id, loginUser.Id);
            Assert.AreEqual(result.Name, loginUser.Name);
            Assert.AreEqual(result.Photo, loginUser.Photo);

        }

        [TestCleanup]
        public void Clean()
        {
            foreach (var id in _tempUsers)
                new UserRepository(_connectionString).DeleteUser(id);
            foreach (var id in _tempEvents)
                new EventRepository(_connectionString).DeleteEvent(id);
            foreach (var id in _tempCreativeTeam)
                new CreativeTeamRepository(_connectionString).DeleteCreativeTeam(id);
        }
    }
}
