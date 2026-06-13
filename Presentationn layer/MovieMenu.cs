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
            Console.WriteLine("\nManage Movies\n");
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
        Console.WriteLine("\n=== AIRING MOVIES ===\n");

        var movies = _service.GetAiringMovies();

        if (movies.Count == 0)
        {
            Console.WriteLine("No movies available.");
        }
        else
        {
            foreach (var movie in movies)
            {
                string ageText = movie.Age > 0
                    ? $" | Age: {movie.Age}+"
                    : "";

                Console.WriteLine(
                    $"ID: {movie.Id} | " +
                    $"Title: {movie.Title} | " +
                    $"Author: {movie.Author} | " +
                    $"Genre: {movie.Genre} | " +
                    $"Duration: {(int)movie.Duration.TotalMinutes} min | " +
                    $"Premier: {movie.Premier:yyyy-MM-dd}" +
                    ageText
                );
            }
        }

        Pause();
    }

    public void ShowMovies()
    {

    }

    private void AssignMovie()
    {
        Console.Clear();
        Console.WriteLine("Enter B at any time to go back.\n");

        var movies = _service.GetAiringMovies();

        Console.WriteLine("=== Available Movies ===\n");

        if (movies.Count == 0)
        {
            Console.WriteLine("No movies available.");
            Pause();
            return;
        }

        PrintMovies();

        Console.WriteLine();

        Console.Write("Enter movie ID: ");
        string movieInput = Console.ReadLine();

        if (movieInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(movieInput, out int id))
        {
            Console.WriteLine("Invalid ID!");
            Pause();
            return;
        }

        Console.Write("Is this Culinary Cinema? (y/n): ");
        string culinaryInput = Console.ReadLine();

        if (culinaryInput?.ToLower() == "b")
        {
            return;
        }

        bool isCulinary = culinaryInput?.ToLower() == "y";

        int theaterId;

        List<string> Alergy = [];
        if (isCulinary)
        {
            theaterId = 1;
            Alergy = Dietary();
        }
        else
        {
            Console.Write("Enter theater ID: ");
            string theaterInput = Console.ReadLine();

            if (theaterInput?.ToLower() == "b")
            {
                return;
            }

            if (!int.TryParse(theaterInput, out theaterId))
            {
                Console.WriteLine("Invalid theater ID!");
                Pause();
                return;
            }
        }

        Console.Write("Enter show time (yyyy-MM-dd HH:mm): ");
        string showTimeInput = Console.ReadLine();

        if (showTimeInput?.ToLower() == "b")
        {
            return;
        }

        if (!DateTime.TryParse(showTimeInput, out DateTime showTime))
        {
            Console.WriteLine("Invalid show time!");
            Pause();
            return;
        }

        if (_service.AddMovieShowing(id, theaterId, showTime, isCulinary, Alergy))
        {
            Console.WriteLine("Movie showing added successfully!");

            if (isCulinary)
            {
                Console.WriteLine("Culinary Cinema enabled (+€50).");
            }
        }
        else
        {
            Console.WriteLine("Failed to add movie showing.");
        }

        Pause();
    }

    private void AddMovie()
    {
        Console.Clear();
        Console.WriteLine("Enter B at any time to return to the previous menu.\n");
        Console.Write("Enter title: ");
        string title = Console.ReadLine();
        if (title?.ToLower() == "b")
        {
            return;
        }

        Console.Write("Enter author: ");

        string author = Console.ReadLine();
        if (author?.ToLower() == "b")
        {
            return;
        }

        Console.WriteLine("Select genre:");

        var genres = Enum.GetValues(typeof(MoviesGenres));

        int index = 1;
        foreach (var g in genres)
        {
            Console.WriteLine($"[{index}] {g}");
            index++;
        }

        Console.Write("Choose: ");
        string genreInput = Console.ReadLine();

        if (genreInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(genreInput, out int choice) ||
            choice < 1 || choice > genres.Length)
        {
            Console.WriteLine("Invalid genre!");
            Pause();
            return;
        }

        MoviesGenres genre = (MoviesGenres)genres.GetValue(choice - 1);

        Console.Write("Enter duration in minutes: ");
        string durationInput = Console.ReadLine();

        if (durationInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(durationInput, out int minutes))
        {
            Console.WriteLine("Invalid duration!");
            Pause();
            return;
        }

        TimeSpan duration = TimeSpan.FromMinutes(minutes);

        Console.Write("Enter premiere date (yyyy-MM-dd): ");
        string dateInput = Console.ReadLine();

        if (dateInput?.ToLower() == "b")
        {
            return;
        }

        if (!DateTime.TryParse(dateInput, out DateTime premier))
        {
            Console.WriteLine("Invalid date!");
            Pause();
            return;
        }

        Console.Write("Enter Age: ");
        string ageInput = Console.ReadLine();

        if (ageInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(ageInput, out int age))
        {
            Console.WriteLine("Invalid age!");
            Pause();
            return;
        }


        _service.AddMovie(title, author, genre, duration, premier, age);

        Console.Clear();
        Console.WriteLine("Movie added!");
        Console.WriteLine("Do you want to add to auditorium and time(y/n):\n");
        string input = Console.ReadLine();
        if (input.ToLower() == "yes" || input.ToLower() == "y")
        {
            Console.Clear();
            var movies = _service.GetAiringMovies();

            int id = movies.Max(m => m.Id);

            Console.WriteLine($"\nMovie '{title}' selected automatically (ID: {id})");

            Console.WriteLine("Is this Culinary Cinema? (y/n): ");
            bool isCulinary = Console.ReadLine()?.ToLower() == "y";

            int theaterId;

            if (isCulinary)
            {
                theaterId = 1;
            }
            else
            {
                Console.WriteLine("Enter theater ID: ");

                if (!int.TryParse(Console.ReadLine(), out theaterId))
                {
                    Console.WriteLine("Invalid theater ID!");
                    Pause();
                    return;
                }
            }

            Console.WriteLine("Enter show time (yyyy-MM-dd HH:mm): ");

            if (!DateTime.TryParse(Console.ReadLine(), out DateTime showTime))
            {
                Console.WriteLine("Invalid show time!");
                Pause();
                return;
            }


            if (_service.AddMovieShowing(id, theaterId, showTime, isCulinary))
            {
                Console.WriteLine("Movie showing added!");

                if (isCulinary)
                {
                    Console.WriteLine("Culinary Cinema enabled (+€50)");
                }
            }
            else
            {
                Console.WriteLine("Failed to add movie showing.");
            }
        }
        Pause();
    }

    private void UpdateMovie()
    {
        Console.Clear();

        Console.WriteLine("=== AVAILABLE MOVIES ===\n");

        PrintMovies();

        Console.WriteLine();
        Console.WriteLine("Enter B at any time to go back.\n");
        Console.Write("Enter movie ID:");
        string movieInput = Console.ReadLine();

        if (movieInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(movieInput, out int id))
        {
            Console.WriteLine("Invalid ID!");
            Pause();
            return;
        }

        Console.Write("New title: ");
        string title = Console.ReadLine();
        if (title?.ToLower() == "b")
        {
            return;
        }

        Console.Write("New author: ");
        string author = Console.ReadLine();
        if (author?.ToLower() == "b")
        {
            return;
        }

        Console.WriteLine("\nSelect genre:");
        var genres = Enum.GetValues(typeof(MoviesGenres));

        for (int i = 0; i < genres.Length; i++)
        {
            Console.WriteLine($"[{i + 1}] {genres.GetValue(i)}");
        }

        Console.WriteLine("[B] Back");
        Console.Write("Choose: ");

        string genreInput = Console.ReadLine();

        if (genreInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(genreInput, out int genreChoice) ||
            genreChoice < 1 ||
            genreChoice > genres.Length)
        {
            Console.WriteLine("Invalid genre!");
            Pause();
            return;
        }

        MoviesGenres genre =
            (MoviesGenres)genres.GetValue(genreChoice - 1);

        Console.Write("New duration (minutes): ");
        string durationInput = Console.ReadLine();

        if (durationInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(durationInput, out int minutes))
        {
            Console.WriteLine("Invalid duration!");
            Pause();
            return;
        }

        TimeSpan duration = TimeSpan.FromMinutes(minutes);

        Console.Write("New premiere date (yyyy-MM-dd): ");
        string dateInput = Console.ReadLine();

        if (dateInput?.ToLower() == "b")
        {
            return;
        }

        if (!DateTime.TryParse(dateInput, out DateTime premier))
        {
            Console.WriteLine("Invalid date!");
            Pause();
            return;
        }
        Console.Write("Enter Age: ");
        string ageInput = Console.ReadLine();

        if (ageInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(ageInput, out int age))
        {
            Console.WriteLine("Invalid age!");
            Pause();
            return;
        }

        _service.UpdateMovie(id, title, author, genre, duration, premier, age);

        Console.WriteLine("Movie updated!");
        Pause();
    }

    private void DeleteMovie()
    {
        Console.Clear();

        Console.WriteLine("=== AVAILABLE MOVIES ===\n");

        PrintMovies();

        Console.WriteLine();

        Console.Write("Enter movie ID (or B to go back): ");
        string movieInput = Console.ReadLine();

        if (movieInput?.ToLower() == "b")
        {
            return;
        }

        if (!int.TryParse(movieInput, out int id))
        {
            Console.WriteLine("Invalid ID!");
            Pause();
            return;
        }

        Console.Write("Are you sure? (y/n): ");
        string confirm = Console.ReadLine();

        if (confirm?.ToLower() != "y")
        {
            Console.WriteLine("Delete cancelled.");
            Pause();
            return;
        }

        _service.DeleteMovie(id);

        Console.WriteLine("Movie deleted!");
        Pause();
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    private void PrintMovies()
    {
        foreach (var movie in _service.GetAiringMovies())
        {
            string ageText = movie.Age > 0
                ? $" | Age: {movie.Age}+"
                : "";

            Console.WriteLine(
                $"ID: {movie.Id} | " +
                $"Title: {movie.Title} | " +
                $"Genre: {movie.Genre}" +
                ageText
            );
        }
    }
}