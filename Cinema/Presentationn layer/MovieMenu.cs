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
        Console.WriteLine("\n🎬 Airing Movies:\n");

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

    private void AddMovie()
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();

        Console.Write("Enter author: ");
        string author = Console.ReadLine();

        Console.Write("Enter genre: ");
        string genre = Console.ReadLine();

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

        _service.AddMovie(title, author, genre, duration, premier);

        Console.WriteLine("✅ Movie added!");
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

        Console.Write("New genre: ");
        string genre = Console.ReadLine();

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

        _service.UpdateMovie(id, title, author, genre, duration, premier);

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
    }

}