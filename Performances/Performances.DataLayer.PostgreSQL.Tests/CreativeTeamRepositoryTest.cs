using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Performances.Model;

namespace Performances.DataLayer.PostgreSQL.Tests
{
    [TestClass]
    public class CreativeTeamRepositoryTest
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;User Id=client;Password=passclient;Database=testdb;Search Path=test;";
        public List<Guid> _tempCreativeteam = new List<Guid>();

        [TestMethod]
        public void ShouldCreateCreativeTeam()
        {
            var creativeteam = new CreativeTeam
            {
                About = "",
                Email = "ourrockband@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "dicklovers",
                Password = "lovedicks",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "dicklovers.jpeg"
            };
            
            var creativeteamrepository = new CreativeTeamRepository(_connectionString);
            var result = creativeteamrepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeteam.Add(result.Id);

            Assert.AreEqual(creativeteam.Name, result.Name);
            Assert.AreEqual(creativeteam.SubscribersCount, result.SubscribersCount);
        }

        [TestMethod]
        public void ShouldGetCreativeTeamById()
        {
            var creativeteam = new CreativeTeam
            {
                About = "",
                Email = "ourrockband@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "dicklovers",
                Password = "lovedicks",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "dicklovers.jpeg"
            };

            var creativeteamrepository = new CreativeTeamRepository(_connectionString);
            var newcreativetem = creativeteamrepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeteam.Add(newcreativetem.Id);

            var result = creativeteamrepository.GetCreativeTeamById(newcreativetem.Id);

            Assert.AreEqual(newcreativetem.Name, result.Name);
        }

        [TestMethod]
        public void ShouldUpdateCreativeTeam()
        {
            var creativeteam = new CreativeTeam
            {
                About = "",
                Email = "ourrockband@ya.ru",
                Genre = "rock/hardcore/post-punk",
                Name = "dicklovers",
                Password = "lovedicks",
                Rating = 3.2,
                SubscribersCount = 0
            };
            var photo = new File
            {
                Bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                FileExtension = ".jpeg",
                Filename = "dicklovers.jpeg"
            };

            var creativeteamrepository = new CreativeTeamRepository(_connectionString);
            var newcreativetem = creativeteamrepository.CreateCreativeTeam(creativeteam, photo.Bytes, photo.Filename);
            _tempCreativeteam.Add(newcreativetem.Id);

            newcreativetem.Email = "testemail@t.ru";

            var result = creativeteamrepository.UpdateCreativeTeam(newcreativetem);

            Assert.AreEqual(newcreativetem.Email, result.Email);
        }

        [TestCleanup]
        public void Clean()
        {
            foreach (var id in _tempCreativeteam)
                new CreativeTeamRepository(_connectionString).DeleteCreativeTeam(id);
        }
    }
}
