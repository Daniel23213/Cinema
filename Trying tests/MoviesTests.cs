//using Cinema;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class MoviesTests
{
    private MovieAcces movieAccess;

    [TestInitialize]
    public void Setup()
    {
        movieAccess = new MovieAcces();
    }

    [TestMethod]
    public void ShowMoviesTableColumns()
    {
        using var connection = new SqliteConnection(
            @"Data Source=C:\Users\Plewka\Desktop\game\Cinema\Cinema\Data Source\Cinema.db");

        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA table_info(movies);";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine(reader.GetString(1));
        }
    }


    //Happy Scenarios

    [TestMethod]
    [DataRow("Batman")]
    [DataRow("Superman")]
    [DataRow("Spiderman")]
    public void AddMovie_ValidMovie_MovieAdded(string title)
    {
        string author = "Test Author";
        MoviesGenres genre = MoviesGenres.Action;
        TimeSpan duration = TimeSpan.FromMinutes(120);
        DateTime premier = DateTime.Now;
        int age = 16;

        movieAccess.AddMovie(title, author, genre, duration, premier, age);

        var movies = movieAccess.GetAiringMovies();

        Assert.IsTrue(movies.Any(m => m.Title == title));
    }

    [TestMethod]
    public void UpdateMovie_ExistingMovie_MovieUpdated()
    {
        var movie = movieAccess.GetAiringMovies().First();

        string newTitle = "Updated Movie";

        movieAccess.UpdateMovie(movie.Id, newTitle, movie.Author, movie.Genre, movie.Duration, movie.Premier, movie.Age);

        var updatedMovie = movieAccess.GetAiringMovies()
                            .First(m => m.Id == movie.Id);

        Assert.AreEqual(newTitle, updatedMovie.Title);
    }

    [TestMethod]
    public void DeleteMovie_ExistingMovie_MovieDeleted()
    {
        movieAccess.AddMovie(
            "DeleteMe",
            "Author",
            MoviesGenres.Action,
            TimeSpan.FromMinutes(100),
            DateTime.Now,
            12);

        var movie = movieAccess.GetAiringMovies()
                      .Last(m => m.Title == "DeleteMe");

        movieAccess.DeleteMovie(movie.Id);

        Assert.IsFalse(movieAccess.SelectMovie(movie.Id));
    }

    [TestMethod]
    public void GetAiringMovies_MoviesExist_ReturnsMovies()
    {
        var movies = movieAccess.GetAiringMovies();

        Assert.IsNotNull(movies);
        Assert.IsTrue(movies.Count > 0);
    }

    [TestMethod]
    public void SelectMovie_ExistingMovie_ReturnsTrue()
    {
        var movie = movieAccess.GetAiringMovies().First();

        bool result = movieAccess.SelectMovie(movie.Id);

        Assert.IsTrue(result);
    }

    //Sad Scenarios

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-100)]
    [DataRow(99999)]
    public void SelectMovie_NonExistingMovie_ReturnsFalse(int movieId)
    {
        bool result = movieAccess.SelectMovie(movieId);

        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-100)]
    [DataRow(99999)]
    public void DeleteMovie_NonExistingMovie_MovieStillNotFound(int movieId)
    {
        movieAccess.DeleteMovie(movieId);

        bool exists = movieAccess.SelectMovie(movieId);

        Assert.IsFalse(exists);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-100)]
    [DataRow(99999)]
    public void UpdateMovie_NonExistingMovie_NoMovieUpdated(int movieId)
    {
        movieAccess.UpdateMovie(
            movieId,
            "Test Movie",
            "Author",
            MoviesGenres.Action,
            TimeSpan.FromMinutes(120),
            DateTime.Now,
            16);

        Assert.IsFalse(movieAccess.SelectMovie(movieId));
    }
}