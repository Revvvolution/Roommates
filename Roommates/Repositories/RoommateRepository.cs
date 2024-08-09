﻿using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        // This Method should return roommate FirstName, RentPortion, and the occupied room's Name by using JOIN
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName, LastName, RentPortion, MoveInDate, Room.Id, Room.Name, Room.MaxOccupancy
                                        FROM Roommate 
                                        LEFT JOIN Room ON Roommate.RoomId = Room.Id 
                                        WHERE Roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate()
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                            }
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }


        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, RentPortion, MoveInDate, RoomId
                                        FROM Roommate;"; // @ is used for multi-line format (readability purposes)

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);
                        
                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int rentColumnPosition = reader.GetOrdinal("RentPortion");
                        int rentValue = reader.GetInt32(rentColumnPosition);

                        int moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDateValue = reader.GetDateTime(moveInDateColumnPosition);

                        int roomIdColumnPosition = reader.GetOrdinal("RoomId");
                        Room roomIdValue = null;

                        Roommate roommate = new Roommate()
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            RentPortion = rentValue,
                            MoveInDate = moveInDateValue,
                            Room = roomIdValue,
                        };
                        // Add the object to the list
                        roommates.Add(roommate);
                    }
                    // Close the connection
                    reader.Close();

                    // Return the roommates list once all objects are added
                    return roommates;
                }
            }
        }




    }
}
