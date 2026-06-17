//using Cinema.Presentationn_layer;

using System.Net.Sockets;
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

                    string choiceInput = UserInputValidation.NullOrEmptyValidationLoop("Enter showing ID: ");

                    if (!int.TryParse(choiceInput, out int choice))
                    {
                        Console.WriteLine("Please enter a valid number.");
                        Pause();
                        break;
                    }

                    SeatAccess seatAccess = new();

                    seatAccess.PrintSeatsByShowingId(choice);

                    string seat = UserInputValidation.NullOrEmptyValidationLoop("Choose a seat: ");
                    int seatid = seatAccess.GetId(seat);

                    if (!seatAccess.IsSeatTaken(choice, seat))
                    {
                        Console.WriteLine("Seat reserved successfully.");

                        UserService user = new();
                        user.ReserveTicket(isLogged, seatid, choice);
                    }

                    Pause();
                    break;

                case "T":
                    // view booked tickets
                    UserService user3 = new();
                    foreach (dynamic ticket in user3.ShowTickets(isLogged.Id)) 
                    {
                        Console.WriteLine(ticket);
                    }

                    Pause();
                    break;

                case "C":
                    // cancel ticket
                    UserService usercancelticket = new();
                    foreach (dynamic ticket in usercancelticket.ShowTickets(isLogged.Id))
                    {
                        Console.WriteLine(ticket);
                    }

                    int reservationId = UserInputValidation.IntInputValidation("Which ticket would you like to cancel? (enter ReservationId)");
                    {
                        string answer = UserInputValidation.NullOrEmptyValidationLoop("Do you really want to cancel ur ticket?").ToLower();

                        if(answer == "yes" || answer == "y") 
                        {
                            usercancelticket.CancelTicket(reservationId, isLogged.Id);
                        }
                        else 
                        {
                            Pause();
                        }
                    }
                    Pause();
                    break;

                case "F":
                    // food menu
                    Console.WriteLine("Food menu feature coming soon...");
                    Pause();
                    break;

                case "O":
                    //implement manage account
                    string manageInput = UserInputValidation.NullOrEmptyValidationLoop("You can change your password, or delete your account.\n Choose an Option\n[D]: Delete Account\n[C]: Change Password\n");

                    UserService userAccess = new();
                    if (manageInput == "D" || manageInput == "d")
                    {
                        // Implement delete account

                        userAccess.DeleteUser(isLogged.Id);
                        Console.WriteLine("Your account has been deleted!");
                        isLogged = null;
                        continue;


                    }
                    else if (manageInput == "C" || manageInput == "c")
                    {
                        // Implement change password
                        // add hashing when changing the passwo
                        Console.Write("Enter the new password(Must be atleast 6 characters long): ");
                        string newpassword = RegisterMenu.CreateMyPasswordTextBox();

                        userAccess.ChangePassword(isLogged.Id, newpassword);

                        Console.ReadLine();
                        Console.WriteLine("Password changed!");
                        Console.ReadLine();
                    }
                    break;
                    break;
                case "R":
                    //Register sysyem

                    isLogged = RegisterMenu.ShowRegisterMenu();


                    break;
                case "L":
                    //login sysyem

                    isLogged = LoginMenu.Show();

                    break;
                case "U":

                    ManageUsers.Show();
                    Pause();
                    break;
                case "M":
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
                case "E":
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