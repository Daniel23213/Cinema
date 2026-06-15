public class MovieMenu
{
    private readonly MovieServiceLogic _service;

    public MovieMenu()
    {
        _service = new MovieServiceLogic();
    }

    //allergies and dietary
    public List<string> Dietary ()
    {
        Dictionary<string, string> CheckList = new ()
        {
            { "Peanuts", "Severe peanut allergy risk" },
            { "Tree Nuts", "Almonds, walnuts, cashews, pecans, etc." },
            { "Dairy / Milk", "Allergic to dairy proteins" },
            { "Eggs", "Egg allergy" },
            { "Wheat / Gluten", "Celiac disease or wheat allergy" },
            { "Soy", "Soy products" },
            { "Fish", "Finfish (e.g., salmon, cod)" },
            { "Shellfish", "Crustaceans and mollusks (e.g., shrimp, crab, clams)" },
            { "Sesame", "Sesame seeds and oil" },
            { "Vegetarian", "No meat, poultry, or seafood" },
            { "Vegan", "No animal products (meat, dairy, eggs, honey)" },
            { "Lactose Intolerant", "Difficulty digesting dairy" },
            { "Halal", "Requires Halal certified meats / no pork / no alcohol" },
            { "Kosher", "Requires Kosher certified foods" },
            { "No Pork", "Avoids pork products strictly" }
        };

        List<string> UserAnswers = [];

        Console.Clear();
        Console.WriteLine("Do you have any allergies and dietary whishes");
        Console.WriteLine("Type 'Yes' to view allergies and dietary restrictions, or 'No' to continue booking.");
        string input0 = Console.ReadLine().ToLower();

        if (input0 == "yes" || input0 == "ja")
        {
            foreach (var item in CheckList)
            {
                Console.WriteLine($"---{item.Key}---");
                Console.WriteLine($"Description: {item.Value}");
                Console.WriteLine("Does this apply to you (Yes / No)");

                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "y" || answer == "ja")
                {
                    UserAnswers.Add(item.Key);
                }
            }
        }
        else if (input0 == "no" || input0 == "n")
        {
            // user skip the allergies and dietary list and continue on.
        }

        return UserAnswers;
    }

    public void ShowManageMoviesMenu()
    {
        bool managing = true;

        while (managing)
        {
            Console.Clear();
            Console.WriteLine("\n🎬 Manage Movies\n");
            Console.WriteLine("[1] Show Movies");
            Console.WriteLine("[2] Add Movie");
            Console.WriteLine("[3] Update Movie");
            Console.WriteLine("[4] Delete Movie");
            Console.WriteLine("[5] Assign a movie");
            Console.WriteLine("[E] Back");

            Console.Write("\nChoose: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    GetAiringMovies();
                    break;

                case "2":
                    AddMovie();
                    break;

                case "3":
                    UpdateMovie();
                    break;

                case "4":
                    DeleteMovie();
                    break;
                case "5":
                    AssignMovie();
                    break;

                case "E":
                case "e":
                    managing = false;
                    break;

                default:
                    Console.WriteLine("Invalid option...");
                    break;
            }
        }
    }

    public void GetAiringMovies()
    {
        Console.Clear();
        Console.WriteLine("\nAiring Movies:\n");

        var movies = _service.GetAiringMovies();

        if (movies.Count == 0)
        {
            Console.WriteLine("No movies available.");
        }
        else
        {
            foreach (var movie in movies)
            {
                Console.WriteLine(movie);
            }
        }

        Pause();
    }

    public void ShowMovies()
    {

    }

    private void AssignMovie()
    {
        Console.WriteLine("Enter movie ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            Pause();
            return;
        }
        Console.WriteLine("Enter theater ID: ");
        if (!int.TryParse(Console.ReadLine(), out int theaterId))
        {
            Console.WriteLine("Invalid theater ID!");
            Pause();
            return;
        }
        Console.WriteLine("Enter show time (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime showTime))
        {
            Console.WriteLine("Invalid show time!");
            Pause();
            return;
        }

        Console.WriteLine("Is this Culinary Cinema? (y/n): ");
        bool isCulinary = Console.ReadLine()?.ToLower() == "y";

        MovieAcces movieAcces = new();

        if (movieAcces.AddMovieShowing(id, theaterId ,showTime, isCulinary))
        {
            Console.WriteLine("Movie showing added!");

            if (isCulinary)
            {
                Console.WriteLine("Culinary Cinema enabled (+€50)");
            }
        }

        Pause();
    }

    private void AddMovie()
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        Console.Write("Enter author: ");
        string author = Console.ReadLine();

        Console.WriteLine("Select genre:");

        var genres = Enum.GetValues(typeof(MoviesGenres));

        int index = 1;
        foreach (var g in genres)
        {
            Console.WriteLine($"[{index}] {g}");
            index++;
        }

        Console.Write("Choose: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) ||
            choice < 1 || choice > genres.Length)
        {
            Console.WriteLine("Invalid genre!");
            Pause();
            return;
        }

        MoviesGenres genre = (MoviesGenres)genres.GetValue(choice - 1);

        Console.Write("Enter duration in minutes: ");
        if (!int.TryParse(Console.ReadLine(), out int minutes))
        {
            Console.WriteLine("Invalid duration!");
            Pause();
            return;
        }

        TimeSpan duration = TimeSpan.FromMinutes(minutes);

        Console.Write("Enter premiere date (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime premier))
        {
            Console.WriteLine("Invalid date!");
            Pause();
            return;
        }
        Console.Write("Enter Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        _service.AddMovie(title, author, genre, duration, premier, age);

        Console.WriteLine("Movie added!");
        Console.WriteLine("Do you want to add to auditorium and time(y/n):\n");
        string input = Console.ReadLine();
        if (input.ToLower() == "yes" || input.ToLower() == "y")
        {
            Console.WriteLine("Enter movie ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID!");
                Pause();
                return;
            }
            Console.WriteLine("Enter theater ID: ");
            if (!int.TryParse(Console.ReadLine(), out int theaterId))
            {
                Console.WriteLine("Invalid theater ID!");
                Pause();
                return;
            }
            Console.WriteLine("Enter show time (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime showTime))
            {
                Console.WriteLine("Invalid show time!");
                Pause();
                return;
            }
            Console.WriteLine("Is this Culinary Cinema? (y/n): ");
            bool isCulinary = Console.ReadLine()?.ToLower() == "y";

            MovieAcces movieAcces = new();

            if (movieAcces.AddMovieShowing(id, theaterId, showTime, isCulinary))
            {
                Console.WriteLine("Movie showing added!");

                if (isCulinary)
                {
                    Console.WriteLine("Culinary Cinema enabled (+€50)");
                }
            }
        }
        Pause();
    }

    private void UpdateMovie()
    {
        Console.Write("Enter movie ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            Pause();
            return;
        }

        Console.Write("New title: ");
        string title = Console.ReadLine();

        Console.Write("New author: ");
        string author = Console.ReadLine();

        Console.Write("Enter genre (Action, Comedy, Drama...): ");
        if (!Enum.TryParse<MoviesGenres>(Console.ReadLine(), true, out MoviesGenres genre))
        {
            Console.WriteLine("Invalid genre!");
            Pause();
            return;
        }

        Console.Write("New duration (minutes): ");
        if (!int.TryParse(Console.ReadLine(), out int minutes))
        {
            Console.WriteLine("Invalid duration!");
            Pause();
            return;
        }

        TimeSpan duration = TimeSpan.FromMinutes(minutes);

        Console.Write("New premiere date: ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime premier))
        {
            Console.WriteLine("Invalid date!");
            Pause();
            return;
        }
        Console.Write("Enter Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        _service.UpdateMovie(id, title, author, genre, duration, premier, age);

        Console.WriteLine("✏️ Movie updated!");
        Pause();
    }

    private void DeleteMovie()
    {
        Console.Write("Enter movie ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            Pause();
            return;
        }

        _service.DeleteMovie(id);

        Console.WriteLine("🗑️ Movie deleted!");
        Pause();
    }

    private void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

}