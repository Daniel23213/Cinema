public static class Menu
{
    private static void AddMovieMenu()
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        Console.Write("Enter author: ");
        string author = Console.ReadLine();

        Console.Write("Enter genre: ");
        string genre = Console.ReadLine();

        Console.Write("Enter duration in minutes: ");
        int minutes = int.Parse(Console.ReadLine());
        TimeSpan duration = TimeSpan.FromMinutes(minutes);

        Console.Write("Enter premiere date (yyyy-MM-dd): ");
        DateTime premier = DateTime.Parse(Console.ReadLine());

        MovieService service = new MovieService();

        service.AddMovie(title, author, genre, duration, premier);

        Console.WriteLine("✅ Movie added successfully!");
    }

    private static void ShowAiringMovies()
    {
        var service = new MovieService();

        bool inScreen = true;

        while (inScreen)
        {
            Console.Clear();
            Console.WriteLine("\n🎬 Airing Movies:\n");

            var movies = service.GetAiringMovies();

            if (movies.Count == 0)
            {
                Console.WriteLine("No airing movies right now.");
                return;
            }
            else
            {
                foreach (var movie in movies)
                {
                    Console.WriteLine(movie);
                }
            }

            Console.WriteLine("\n[E] Go back");

            Console.Write("\nChoose: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "E":
                    inScreen = false;
                    break;
                default:
                    Console.WriteLine("Invalid option...");
                    break;
            }
        }
        Console.Clear();
    }

    public static void ShowMenu()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n[1]: Airing movies");
            Console.WriteLine("[2]: Buy tickets");
            Console.WriteLine("[3]: Booked tickets");
            Console.WriteLine("[4]: Cancel tickets");
            Console.WriteLine("[5]: Food menu");
            Console.WriteLine("[6]: Manage account");
            //use this when UserRole will be implemented
            //if (role == UserRole.Manager || role == UserRole.SuperManager)
            //{
            //Console.WriteLine("[8]: Manage movies");
            //}

            //if (role == UserRole.SuperManager)
            //{
            //Console.WriteLine("[9]: Manage users");
            //}
            Console.WriteLine("[7]: Exit");

            Console.Write("Choose an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    //Add movie will go to adming
                    AddMovieMenu();
                    ShowAiringMovies();
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
                    //implement food menu
                    Console.WriteLine("Food menu feature coming soon...");
                    break;

                case "5":
                    //implement manage account
                    Console.WriteLine("Manage account feature coming soon...");
                    break;

                case "6":
                    //implement cancel ticket
                    Console.WriteLine("Cancel ticket feature coming soon...");
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

                case "7":
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