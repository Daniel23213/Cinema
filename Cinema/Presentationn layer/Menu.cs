//using Cinema.Presentationn_layer;

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
            Console.WriteLine("\n[1]: Airing movies");
            if (isLogged != null)
            {

                Console.WriteLine("[2]: Buy tickets");
                Console.WriteLine("[4]: Cancel tickets");
                Console.WriteLine("[3]: Booked tickets");
                Console.WriteLine("[6]: Manage account");
                if (isLogged.Role == "Admin" || isLogged.Role == "SuperManager")
                {
                    Console.WriteLine("[M]: Manage movies");
                }

                if (isLogged.Role == "SuperManager")
                {
                    Console.WriteLine("[U]: Manage users");
                }

            }
            Console.WriteLine("[5]: Food menu");

            Console.WriteLine("[E]: Exit");
            if (isLogged == null)
            {
                Console.WriteLine("[R]: Register");
                Console.WriteLine("[L]: Login");
            }

            //use this when UserRole will be implemented



            Console.Write("Choose an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();

                    Console.WriteLine("\n=== AIRING MOVIES / SHOWINGS ===");
                    Console.WriteLine("1 - Show all showings");
                    Console.WriteLine("2 - Filter by genre");
                    Console.Write("\nChoose option: ");

                    string option = Console.ReadLine();

                    if (option == "1")
                    {
                        Console.Clear();

                        var allShowings = movieService.GetShowings(isLogged);

                        Console.WriteLine("\n=== MOVIE SHOWINGS ===\n");

                        foreach (var showing in allShowings)
                        {
                            Console.WriteLine(showing);
                        }
                    }
                    else if (option == "2")
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


                case "2":

                    var showings = movieService.GetShowings(isLogged);

                    Console.WriteLine("\n=== MOVIE SHOWINGS ===\n");

                    foreach (var showing in showings)
                    {
                        Console.WriteLine(showing);
                    }

                    Console.Write("Enter showing ID: ");
                    string choiceInput = Console.ReadLine();

                    if (!int.TryParse(choiceInput, out int choice))
                    {
                        Console.WriteLine("Please enter a valid number.");
                        Pause();
                        break;
                    }

                    SeatAccess seatAccess = new();

                    seatAccess.PrintSeatsByShowingId(choice);

                    string seat = Console.ReadLine();
                    int seatid = seatAccess.GetId(seat);

                    if (!seatAccess.IsSeatTaken(choice, seat))
                    {
                        Console.WriteLine("Seat reserved successfully.");

                        UserService user = new();
                        user.ReserveTicket(isLogged, seatid, choice);
                    }

                    Pause();
                    break;

                case "3":
                    //implement view booked tickets
                    UserAccess user3 = new();
                    foreach (dynamic ticket in user3.ShowTickets(isLogged.Id)) 
                    {
                        Console.WriteLine(ticket);
                    }

                    Pause();
                    break;

                case "4":
                    //implement cancel ticket
                    Console.WriteLine("Cancel ticket feature coming soon...");
                    Pause();
                    break;

                case "5":
                    //implement food menu
                    Console.WriteLine("Food menu feature coming soon...");
                    Pause();
                    break;

                case "6":
                    //implement manage account
                    Console.WriteLine("Manage account feature coming soon...");
                    Console.WriteLine("You can change your password, or delete your account.\n Choose an Option \n Delete Account - D \n Change Password - C");
                    string manageInput = Console.ReadLine();
                    UserService userAccess = new();
                    if (manageInput == "D" || manageInput == "d")
                    {
                        // Implement delete account

                        userAccess.DeleteUser(isLogged);
                        Console.WriteLine("Your account has been deleted!");
                        isLogged = null;
                        continue;


                    }
                    else if (manageInput == "C" || manageInput == "c")
                    {
                        // Implement change password
                        string newpassword = Console.ReadLine();
                        userAccess.ChangePassword(isLogged.Id, newpassword);
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

                    ManageUsers.Show();
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