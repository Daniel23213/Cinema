//using Cinema.Presentationn_layer;

using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Channels;

public static class Menu
{
    private static readonly MovieMenu movieMenu = new();
    private static readonly MovieServiceLogic movieService = new();
    

    public static void ShowMenu()
    {
        bool running = true;
        UserModel isLogged = null;

        while (running)
        {
            Console.Clear();
            if (isLogged == null)
            {
                Console.WriteLine("To Book a ticket make account first!");
            }
            // User first Name and Last Name print put up in another if not to confuse the user
            if (isLogged != null)
            {
                Console.WriteLine($"Logged in as: {isLogged.FirstName} {isLogged.LastName}");
            }
            Console.WriteLine("\n[A]: Airing movies");
            if (isLogged != null)
            {

                Console.WriteLine("[B]: Buy tickets");
                Console.WriteLine("[C]: Cancel tickets");
                Console.WriteLine("[T]: Booked tickets");
                Console.WriteLine("[O]: Manage account");
                if (isLogged.Role == "Admin" || isLogged.Role == "SuperManager")
                {
                    Console.WriteLine("[M]: Manage movies");
                }

                if (isLogged.Role == "SuperManager")
                {
                    Console.WriteLine("[U]: Manage users");
                }

            }
            Console.WriteLine("[F]: Food menu");

            Console.WriteLine("[E]: Exit");
            if (isLogged == null)
            {
                Console.WriteLine("[R]: Register");
                Console.WriteLine("[L]: Login");
            }

            //use this when UserRole will be implemented

            string input = UserInputValidation.NullOrEmptyValidationLoop("Choose an option: ");

            switch (input.ToUpper())
            {
                case "A":
                    // Airing Movies
                    Console.Clear();

                    Console.WriteLine("\n=== AIRING MOVIES / SHOWINGS ===");
                    Console.WriteLine("A - Show all showings");
                    Console.WriteLine("F - Filter by genre");

                    string option = UserInputValidation.NullOrEmptyValidationLoop("\nChoose option: ");

                    if (option == "A")
                    {
                        Console.Clear();

                        var allShowings = movieService.GetShowings(isLogged);

                        Console.WriteLine("\n=== MOVIE SHOWINGS ===\n");

                        foreach (var showing in allShowings)
                        {
                            Console.WriteLine(showing);
                        }
                    }
                    else if (option == "F")
                    {
                        Console.Clear();
                        Console.WriteLine("\nAvailable genres:");

                        var genres = Enum.GetValues(typeof(MoviesGenres));

                        for (int i = 0; i < genres.Length; i++)
                        {
                            Console.WriteLine($"[{i + 1}] {genres.GetValue(i)}");
                        }

                        Console.Write("\nChoose genre: ");

                        if (int.TryParse(Console.ReadLine(), out int genreChoice)
                            && genreChoice >= 1
                            && genreChoice <= genres.Length)
                        {
                            MoviesGenres genre =
                                (MoviesGenres)genres.GetValue(genreChoice - 1);

                            Console.Clear();

                            var genreShowings = movieService.GetShowingsByGenre(genre);

                            Console.WriteLine($"\n=== SHOWINGS ({genre}) ===\n");

                            foreach (var showing in genreShowings)
                            {
                                Console.WriteLine(showing);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid genre.");
                        }
                    }

                    Pause();
                    break;

                case "B":
                    // Buy Ticket

                    var showings = movieService.GetShowings(isLogged);

                    Console.WriteLine("\n=== MOVIE SHOWINGS ===\n");

                    foreach (var showing in showings)
                    {
                        Console.WriteLine(showing);
                    }

                    string choiceInput = UserInputValidation.NullOrEmptyValidationLoop("Enter showing ID (or B to back): "); // Added option

                    if (choiceInput.ToLower() == "b") { break; } // Added check

                    if (!int.TryParse(choiceInput, out int choice))
                    {
                        Console.WriteLine("Please enter a valid number.");
                        Pause();
                        break;
                    }

                    SeatAccess seatAccess = new();
                    AuditoriumAccess auditoriumAccess = new();
                    AuditoriumModel auditoriumModel = auditoriumAccess.GetAuditoriumByShowingID(int.Parse(choiceInput));
                    auditoriumModel.PrintAuditoriumDiagram();
                    Console.Write("Please select a seat: ");

                    string seat = UserInputValidation.NullOrEmptyValidationLoop("Choose a seat (or B to back): "); // Added option

                    if (seat.ToLower() == "b") { break; } // Added check

                    int seatid = seatAccess.GetId(seat);

                    if (!seatAccess.IsSeatTaken(choice, seat))
                    {
                        Console.WriteLine("Seat reserved successfully.");

                        UserService user = new();
                        user.ReserveTicket(isLogged, seatid, choice);

                        Console.WriteLine("\n=== Proceed to culinary options ===");
                        Pause();

                        ReservationModel reserve = new ReservationModel();
                        List<String> Alergy = DieteryQuestionnarie.Dietary();

                        if(Alergy.Count > 0)
                        {
                            Console.WriteLine("Saved dietary options");
                            foreach(var item in Alergy)
                            {
                                Console.WriteLine($"- {item}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nothing is selected");
                        }
                    }

                    Pause();
                    break;

                case "T":
                    Console.Clear();
                    // view booked tickets
                    UserService user3 = new();
                    foreach (dynamic ticket in user3.ShowTickets(isLogged.Id)) 
                    {
                        Console.WriteLine($"ID: {ticket.ReservationId}, Movie: {ticket.MovieTitle}, Time: {ticket.ShowTime}, Seat: {ticket.Seat}");
                    }

                    Pause();
                    break;

                case "C":
                    Console.Clear();
                    // 1. Show the tickets first
                    UserService usercancelticket = new();
                    var tickets = usercancelticket.ShowTickets(isLogged.Id);

                    foreach (var ticket in tickets)
                    {
                        Console.WriteLine($"ID: {ticket.ReservationId}, Movie: {ticket.MovieTitle}, Time: {ticket.ShowTime}, Seat: {ticket.Seat}");
                    }

                    string reservationInput = UserInputValidation.NullOrEmptyValidationLoop("\nWhich ticket would you like to cancel? (Enter ID or B to go back): ");

                    if (reservationInput.ToLower() == "b") { break; }

                    if (!int.TryParse(reservationInput, out int reservationId))
                    {
                        Console.WriteLine("Invalid ID format.");
                        Pause();
                        break;
                    }

                    string answer = UserInputValidation.NullOrEmptyValidationLoop("Do you really want to cancel your ticket? (yes/no): ").ToLower();

                    if (answer == "yes" || answer == "y")
                    {
                        usercancelticket.CancelTicket(reservationId, isLogged.Id);
                        Console.WriteLine("Ticket cancelled.");
                    }
                    else
                    {
                        Console.WriteLine("Cancellation aborted.");
                    }
                    Pause();
                    break;

                case "F" or "f":
                    Console.Clear();
                    FoodMenuAccess FoodMenuAccesss = new FoodMenuAccess();

                    try
                    {
                        Console.WriteLine("=== Here is the Bar & Lounge menu ===");

                        Console.WriteLine("\n=== DRINKS ===");
                        foreach (var item in FoodMenuAccesss.GetItemsByCategory("drinks"))
                        {
                            Console.WriteLine($"{item.Category} | {item.Name} - €{item.Price:F2}");
                        }

                        Console.WriteLine("\n=== SNACKS ===");
                        foreach (var item in FoodMenuAccesss.GetItemsByCategory("snacks"))
                        {
                            Console.WriteLine($"{item.Category} | {item.Name} - €{item.Price:F2}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Something went wrong: {e.Message}");
                    }
                    
                    Pause();
                    break;

                case "O":
                    Console.Clear();

                    // Implement manage account
                    string manageInput = UserInputValidation.NullOrEmptyValidationLoop("You can change your password, or delete your account.\n\n[D]: Delete Account\n[C]: Change Password\n[B]: Back\n\nChoose: ");

                    if (manageInput.ToLower() == "b") { break; } // Check for back

                    UserService userAccess = new();
                    if (manageInput.ToLower() == "d")
                    {
                        string confirm = UserInputValidation.NullOrEmptyValidationLoop("Are you sure you want to delete your account? (y/n): "); // Validation loop

                        if (confirm.ToLower() == "y")
                        {
                            userAccess.DeleteUser(isLogged.Id);
                            Console.WriteLine("Your account has been deleted!");
                            isLogged = null;
                            Pause(); // Consistent pause
                            continue;
                        }
                    }
                    else if (manageInput.ToLower() == "c")
                    {
                        Console.Write("Enter the new password(Must be atleast 6 characters long): ");
                        string newpassword = RegisterMenu.CreateMyPasswordTextBox();
                        Console.WriteLine();

                        if (newpassword.ToLower() == "b") { break; } // Check for back

                        userAccess.ChangePassword(isLogged.Id, newpassword);

                        Console.WriteLine("Password changed!");
                        Pause(); // Consistent pause
                    }

                    break;
                case "R" or "r":
                    //Register sysyem

                    isLogged = RegisterMenu.ShowRegisterMenu();


                    break;
                case "L" or "l":
                    //login sysyem

                    isLogged = LoginMenu.Show();

                    break;
                case "U" or "u":

                    ManageUsers.Show(isLogged);
                    Pause();
                    break;
                case "M" or "m":
                    if (isLogged != null &&
                        (isLogged.Role == "Admin" || isLogged.Role == "SuperManager"))
                    {
                        movieMenu.ShowManageMoviesMenu();
                    }
                    else
                    {
                        Console.WriteLine("Access denied.");
                        Pause();
                    }
                    break;
                case "E" or "e":
                    Console.WriteLine("Exiting...");
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, please try again.");
                    Pause();
                    break;
            }
        }
    }
    private static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}