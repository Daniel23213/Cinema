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
            Console.WriteLine("\nManage Movies\n");
            Console.WriteLine("[S] Show Movies");
            Console.WriteLine("[A] Add Movie");
            Console.WriteLine("[U] Update Movie");
            Console.WriteLine("[D] Delete Movie");
            Console.WriteLine("[M] Assign a movie");
            Console.WriteLine("[E] Back");

            Console.Write("\nChoose: ");
            string input = UserInputValidation.NullOrEmptyValidationLoop("\nChoose: ");

            switch (input.ToUpper())
            {
                case "S":
                    GetAiringMovies();
                    break;

                case "A":
                    AddMovie();
                    break;

                case "U":
                    UpdateMovie();
                    break;

                case "D":
                    DeleteMovie();
                    break;
                case "M":
                    AssignMovie();
                    break;

                case "E":
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

        string movieInput = UserInputValidation.NullOrEmptyValidationLoop("Enter movie ID: ");

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

        string culinaryInput = UserInputValidation.NullOrEmptyValidationLoop("Is this Culinary Cinema? (y/n): ");

        if (culinaryInput?.ToLower() == "b")
        {
            return;
        }

        bool isCulinary = culinaryInput?.ToLower() == "y";

        int theaterId;

        if (isCulinary)
        {
            theaterId = 1;
        }
        else
        {
            string theaterInput = UserInputValidation.NullOrEmptyValidationLoop("Enter theater ID: ");

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

        string showTimeInput = UserInputValidation.NullOrEmptyValidationLoop("Enter show time (yyyy-MM-dd HH:mm): ");

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

        if (_service.AddMovieShowing(id, theaterId, showTime, isCulinary))
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

        string message = "Enter B at any time to return to the previous menu.\nEnter title: ";
        string title = UserInputValidation.NullOrEmptyValidationLoop(message);


        if (title?.ToLower() == "b")
        {
            return;
        }

        string author = UserInputValidation.NullOrEmptyValidationLoop("Enter author: ");

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

        string genreInput = UserInputValidation.NullOrEmptyValidationLoop("Choose: ");

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

        string durationInput = UserInputValidation.NullOrEmptyValidationLoop("Enter duration in minutes: ");

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

        string dateInput = UserInputValidation.NullOrEmptyValidationLoop("Enter premiere date (yyyy-MM-dd): ");

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

        string ageInput = UserInputValidation.NullOrEmptyValidationLoop("Enter Age: ");

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

        string input = UserInputValidation.NullOrEmptyValidationLoop("Do you want to add to auditorium and time(y/n):\n");

        if (input.ToLower() == "yes" || input.ToLower() == "y")
        {
            Console.Clear();
            var movies = _service.GetAiringMovies();

            int id = movies.Max(m => m.Id);

            Console.WriteLine($"\nMovie '{title}' selected automatically (ID: {id})");

            string isCulinaryString = UserInputValidation.NullOrEmptyValidationLoop("Is this Culinary Cinema? (y/n): ");
            bool isCulinary = false;
            int theaterId;

            if (isCulinaryString.ToLower() == "y" || isCulinaryString.ToLower() == "yes")
            {
                isCulinary = true;
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

        string messageTime = "Enter B at any time to go back.\n" + "Enter movie ID:";
        string movieInput = UserInputValidation.NullOrEmptyValidationLoop(messageTime);

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

        string title = UserInputValidation.NullOrEmptyValidationLoop("New title: ");

        if (title?.ToLower() == "b")
        {
            return;
        }

        string author = UserInputValidation.NullOrEmptyValidationLoop("New author: ");

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

        string messageGenre = "[B] Back" + "\n" + "Choose: ";
        string genreInput = UserInputValidation.NullOrEmptyValidationLoop(messageGenre);

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

        string durationInput = UserInputValidation.NullOrEmptyValidationLoop("New duration (minutes): ");

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

        string dateInput = UserInputValidation.NullOrEmptyValidationLoop("New premiere date (yyyy-MM-dd): ");

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

        string ageInput = UserInputValidation.NullOrEmptyValidationLoop("Enter Age: ");


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

        string movieInput = UserInputValidation.NullOrEmptyValidationLoop("Enter movie ID (or B to go back): ");

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

        string confirm = UserInputValidation.NullOrEmptyValidationLoop("Are you sure? (y/n): ");

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