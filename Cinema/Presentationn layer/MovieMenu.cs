public class MovieMenu
{
    private readonly MovieServiceLogic _service;

    public MovieMenu()
    {
        _service = new MovieServiceLogic();
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
        Console.Clear();

        var movies = _service.GetAiringMovies();

        Console.WriteLine("=== Available Movies ===\n");

        if (movies.Count == 0)
        {
            Console.WriteLine("No movies available.");
            Pause();
            return;
        }

        foreach (var movie in movies)
        {
            string ageText = movie.Age > 0
                ? movie.Age.ToString()
                : "All Ages";

            Console.WriteLine(
                $"ID: {movie.Id} | " +
                $"Title: {movie.Title} | " +
                $"Genre: {movie.Genre} | " +
                $"Age: {ageText}"
            );
        }

        Console.WriteLine();

        Console.Write("Enter movie ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            Pause();
            return;
        }

        Console.Write("Is this Culinary Cinema? (y/n): ");
        bool isCulinary = Console.ReadLine()?.Trim().ToLower() == "y";

        int theaterId;

        if (isCulinary)
        {
            theaterId = 1;
            Console.WriteLine("Culinary Cinema automatically assigned to Auditorium 1.");
        }
        else
        {
            Console.Write("Enter theater ID: ");
            if (!int.TryParse(Console.ReadLine(), out theaterId))
            {
                Console.WriteLine("Invalid theater ID!");
                Pause();
                return;
            }
        }

        Console.Write("Enter show time (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime showTime))
        {
            Console.WriteLine("Invalid show time!");
            Pause();
            return;
        }

        MovieAcces movieAcces = new();

        if (movieAcces.AddMovieShowing(id, theaterId, showTime, isCulinary))
        {
            Console.WriteLine("Movie showing added successfully!");
            if (isCulinary)
            {
                theaterId = 1;
            }

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