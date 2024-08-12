using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true; TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;

            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} -- has an Id of {c.Id}");
                        }
                        Console.Write("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for a chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore selectedChore = choreRepo.GetById(choreId);

                        try
                        {
                            Console.WriteLine($"Chore Id {selectedChore.Id} -- {selectedChore.Name}");

                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            break;
                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("\nNo chore is associated with this Id.");
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            break;
                        }

                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName,
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added to the list of chores.");

                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for a roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        try
                        {
                            Roommate selectedRoommate = roommateRepo.GetById(roommateId);
                            Console.WriteLine($"{selectedRoommate.FirstName} -- Paying ${selectedRoommate.RentPortion} rent portion -- Currently occupying {selectedRoommate.Room.Name}");

                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            break;
                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("\nNo roommate is associated with this Id.");
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            break;
                        }

                    case ("View unassigned chores"):
                        List<Chore> unassignedChore = choreRepo.GetUnassignedChores();

                        Console.WriteLine("\t\tUnassigned Chores\n");

                        foreach (Chore uc in unassignedChore)
                        {
                            Console.WriteLine($"{uc.Name}");
                        }

                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all roommates"):
                        List<Roommate> allRoommates = roommateRepo.GetAll();

                        Console.WriteLine("\t\tAll Roommates\n");

                        foreach (Roommate rm in allRoommates)
                        {
                            Console.WriteLine($"Id: {rm.Id} -- {rm.FirstName} {rm.LastName} -- Rent Portion: ${rm.RentPortion} -- Moved In: {rm.MoveInDate.ToShortDateString()}");
                        }

                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Find roommates by room Id"):
                        Console.Write("\t\tFind roommates by room Id\nRoom Id: ");
                        int roomId = int.Parse(Console.ReadLine());

                        List<Roommate> selectedRoom = roommateRepo.GetRoommatesByRoomId(roomId);

                        try
                        {
                            Console.WriteLine($"Currently occupying {selectedRoom[0].Room.Name}: \n");

                            foreach (Roommate rm in selectedRoom)
                            {
                                Console.WriteLine($"{rm.FirstName} {rm.LastName} -- Paying ${rm.RentPortion} rent portion -- Moved In: {rm.MoveInDate.ToShortDateString()}");
                            }

                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            break;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Console.WriteLine("\nNo roommates found occupying this room.");
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            break;
                        }

                    case ("Add a roommate"):
                        Console.Write("Enter First Name: ");
                        string rmFirstName = Console.ReadLine();

                        Console.Write("Enter Last Name: ");
                        string rmLastName = Console.ReadLine();

                        Console.Write("Enter Rent Portion amount: $");
                        int rmRent = int.Parse(Console.ReadLine());

                        Console.Write("Move In Date (YYYY-MM-DD):");
                        DateTime rmMoveInDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("Room Id for roommate: ");
                        int rmRoomId = int.Parse(Console.ReadLine());


                        Roommate roommateToAdd = new Roommate()
                        {
                            FirstName = rmFirstName,
                            LastName = rmLastName,
                            RentPortion = rmRent,
                            MoveInDate = rmMoveInDate,
                        };

                        roommateRepo.Insert(roommateToAdd, rmRoomId);

                        Console.WriteLine($"\t\n{roommateToAdd.FirstName} {roommateToAdd.LastName} has been added to the list of roommates with an Id of {roommateToAdd.Id}.");

                        Console.WriteLine("\nPress any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }
        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Show all roommates",
                "Show all chores",
                "View unassigned chores",
                "Search for room",
                "Search for a chore",
                "Search for a roommate",
                "Find roommates by room Id",
                "Add a room",
                "Add a chore",
                "Add a roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
