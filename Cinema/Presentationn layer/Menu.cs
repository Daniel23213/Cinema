//using Cinema.Presentationn_layer;

public static class Menu
{
    private static MovieMenu movieMenu = new MovieMenu();

    public static void ShowMenu()
    {
        bool running = true;
        UserModel isLogged = null;

        while (running)
        {
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
                    movieMenu.GetAiringMovies();
                    break;

                case "2":
                    //implement buy tickets movies
                    Console.WriteLine("Buy tickets feature coming soon...");
                    break;

                case "3":
                    //implement view booked tickets
                    Console.WriteLine("View booked tickets feature coming soon...");
                    break;

                case "4":
                    //implement cancel ticket
                    Console.WriteLine("Cancel ticket feature coming soon...");
                    break;

                case "5":
                    //implement food menu
                    Console.WriteLine("Food menu feature coming soon...");
                    break;

                case "6":
                    //implement manage account
                    Console.WriteLine("Manage account feature coming soon...");
                    Console.WriteLine("You can change your password, or delete your account.\n Choose an Option \n Delete Account - D \n Change Password - C");
                    string manageInput = Console.ReadLine();
                    UserAccess userAccess = new UserAccess();
                    if (manageInput == "D" || manageInput == "d")
                    {
                        // Implement delete account

                        userAccess.Delete(isLogged);
                        Console.WriteLine("Your account has been deleted!");
                        isLogged = null;
                        continue;


                    }
                    else if (manageInput == "C" || manageInput == "c")
                    {
                        // Implement change password
                        string newpassword = Console.ReadLine();
                        userAccess.UpdatePassword(isLogged.Id, newpassword);
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
                    }
                    break;

                //use this when UserRole will be implemented
                //case "8":
                //if (role == UserRole.Manager || role == UserRole.SuperManager)
                //{
                //Console.WriteLine("Manager feature coming soon...");
                //}
                //else
                //{
                //Console.WriteLine("Access denied.");
                //}
                //break;

                //case "9":
                //if (role == UserRole.SuperManager)
                //{
                //Console.WriteLine("Super manager users feature coming soon...");
                //}
                //else
                //{
                //Console.WriteLine("Access denied.");
                //}
                //break;

                case "E" or "e":
                    Console.WriteLine("Exiting...");
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }
}