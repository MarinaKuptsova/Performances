using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using Performances.Model;

namespace Performances.DataLayer.PostgreSQL
{
    public class CreativeTeamRepository : ICreativeTeamRepository
    {
        private readonly string _connectionString; // = "Server=localhost;Port=5432;User Id=client;Password=passclient;Database=testdb;Search Path=test;";

        public CreativeTeamRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CreativeTeam CreateCreativeTeam(CreativeTeam creativeTeam, byte[] photo, string filename)
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
                    creativeTeam.Id = Guid.NewGuid();
                    creativeTeam.Photo = file.Id;

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
                            "insert into creativeteam (id, name, genre, about, subscriberscount, rating, password, email, photo)"
                            + " values (@id, @name, @genre, @about, @subscriberscount, @rating, @password, @email, @photo)";
                        command.Parameters.AddWithValue("@id", creativeTeam.Id);
                        command.Parameters.AddWithValue("@name", creativeTeam.Name);
                        command.Parameters.AddWithValue("@genre", creativeTeam.Genre);
                        command.Parameters.AddWithValue("@about", creativeTeam.About);
                        command.Parameters.AddWithValue("@photo", creativeTeam.Photo);
                        command.Parameters.AddWithValue("@subscriberscount", creativeTeam.SubscribersCount);
                        command.Parameters.AddWithValue("@rating", creativeTeam.Rating);
                        command.Parameters.AddWithValue("@password", creativeTeam.Password);
                        command.Parameters.AddWithValue("@email", creativeTeam.Email);
                        command.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }
            return creativeTeam;
        }

        public void DeleteCreativeTeam(CreativeTeam creativeTeam)
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
                        command.CommandText = "update creativeteam set photo=null where id=@id";
                        command.Parameters.AddWithValue("@id", creativeTeam.Id);
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText = "delete from files where id=@photoid";
                        command.Parameters.AddWithValue("@photoid", creativeTeam.Photo);
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText =
                            "delete from subscribes where creativeteamid=@id";
                        command.Parameters.AddWithValue("@id", creativeTeam.Id);
                        command.ExecuteNonQuery();
                    }

                    List<Event> events = new List<Event>();
                    EventRepository eventrepo = new EventRepository(_connectionString);
                    events = eventrepo.GetCreativeTeamEvents(creativeTeam);

                    foreach (var e in events)
                    {
                        eventrepo.DeleteEvent(e);
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = tran;
                        command.CommandText =
                            "delete from creativeteam where id=@id";
                        command.Parameters.AddWithValue("@id", creativeTeam.Id);
                        command.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }
        }

        public void DeleteCreativeTeam(Guid creativeTeamId)
        {
            DeleteCreativeTeam(GetCreativeTeamById(creativeTeamId));
        }

        public CreativeTeam GetCreativeTeamById(Guid creativeTeamId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from creativeteam where id = @creativeteamid";
                    command.Parameters.AddWithValue("@creativeteamid", creativeTeamId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException($"Творческого коллектива с Id {creativeTeamId} не нашлось");
                        var newCreativeTeam = new CreativeTeam()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            About = reader.GetString(reader.GetOrdinal("about")),
                            SubscribersCount = reader.GetInt32(reader.GetOrdinal("subscriberscount")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Photo = reader.GetGuid(reader.GetOrdinal("photo")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Password = reader.GetString(reader.GetOrdinal("password")),
                            Genre = reader.GetString(reader.GetOrdinal("genre")),
                            Rating = reader.GetDouble(reader.GetOrdinal("rating"))
                        };
                        return newCreativeTeam;
                    }
                }
            }
        }

        public CreativeTeam UpdateCreativeTeam(CreativeTeam newCreativeTeam)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update creativeteam set name = @name, " +
                                          "genre = @genre, " +
                                          "about = @about, " +
                                          "subscriberscount = @subscriberscount, " +
                                          "photo = @photo, " +
                                          "rating = @rating, " +
                                          "password = @password, " +
                                          "email = @email " +
                                          "WHERE id = @creativeteamid";
                    command.Parameters.AddWithValue("@creativeteamid", newCreativeTeam.Id);
                    command.Parameters.AddWithValue("@name", newCreativeTeam.Name);
                    command.Parameters.AddWithValue("@genre", newCreativeTeam.Genre);
                    command.Parameters.AddWithValue("@subscriberscount", newCreativeTeam.SubscribersCount);
                    command.Parameters.AddWithValue("@photo", newCreativeTeam.Photo);
                    command.Parameters.AddWithValue("@password", newCreativeTeam.Password);
                    command.Parameters.AddWithValue("@rating", newCreativeTeam.Rating);
                    command.Parameters.AddWithValue("@email", newCreativeTeam.Email);
                    command.Parameters.AddWithValue("@about", newCreativeTeam.About);
                    command.ExecuteNonQuery();
                }
            }
            return newCreativeTeam;
        }
    }
}
