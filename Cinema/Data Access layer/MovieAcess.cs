using Microsoft.Data.Sqlite;

public class MovieAcces : IMovieAcces
{
    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";

    public List<MovieModel> GetAiringMovies()
    {
        var movies = new List<MovieModel>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Title, Author, Genre, Duration, Premier, Age FROM movies";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new MovieModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                Enum.Parse<MoviesGenres>(reader.GetString(3)),
                TimeSpan.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5))
                , reader.GetInt32(6)

            ));
        }

        return movies;
    }

    public void AddMovie(string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier, int age)
    {
        var movies = new List<MovieModel>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO movies (Title, Duration, Author, Genre, Premier, Age)
            VALUES (@title, @duration, @author, @genre, @premier, @age)";

        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@duration", duration.ToString());
        command.Parameters.AddWithValue("@author", author);
        command.Parameters.AddWithValue("@genre", genre.ToString());
        command.Parameters.AddWithValue("@premier", premier.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@age", age);

        command.ExecuteNonQuery();
    }

    public void UpdateMovie(int id, string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier, int age)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE movies
            SET Title=@title, Author=@author, Genre=@genre, Duration=@duration, Premier=@premier, Age=@age
            WHERE Id=@id";

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@author", author);
        command.Parameters.AddWithValue("@genre", genre.ToString());
        command.Parameters.AddWithValue("@duration", duration.ToString());
        command.Parameters.AddWithValue("@premier", premier.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@age", age);

        command.ExecuteNonQuery();
    }

    public void DeleteMovie(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM movies WHERE Id=@id";
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }

    public bool SelectMovie(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id FROM movies WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        return reader.Read();
    }

    public void GetShowings(UserModel user)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
        SELECT 
            movie_showings.Id,
            movies.Title,
            movies.Age,
            movies.Genre,
            theater.Description,
            movie_showings.ShowTime,
            movie_showings.IsCulinary,
            movie_showings.ExtraPrice
        FROM movie_showings
        JOIN movies 
            ON movie_showings.Movie_Id = movies.Id
        JOIN theater 
            ON movie_showings.Theater_Id = theater.Id
        ORDER BY movie_showings.ShowTime;
        ";
        using var reader = command.ExecuteReader();

        Console.WriteLine("\n=== Movie Showings ===");

        while (reader.Read())
        {
            int requiredAge = reader.GetInt32(2);

            // Skip showing if user is too young
            if (user != null && user.Age < requiredAge)
            {
                continue;
            }

            bool isCulinary = reader.GetInt32(6) == 1;
            double extraPrice = reader.GetDouble(7);

            string culinaryText = isCulinary
                ? $" | Culinary Cinema (+{extraPrice})"
                : "";

            Console.WriteLine(
                $"Showing ID: {reader.GetInt32(0)} | " +
                $"Movie: {reader.GetString(1)} | " +
                $"Age: {reader.GetInt32(2)} | " +
                $"Genre: {reader.GetString(3)} | " +
                $"Theater: {reader.GetString(4)} | " +
                $"Time: {reader.GetString(5)}" +
                culinaryText
            );

        }
    }


    // From here its Showings related methods
    public void GetShowingsByGenre(MoviesGenres genre)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
        SELECT 
            movie_showings.Id,
            movies.Title,
            movies.Age,
            movies.Genre,
            theater.Description,
            movie_showings.ShowTime,
            movie_showings.IsCulinary,
            movie_showings.ExtraPrice
        FROM movie_showings
        JOIN movies ON movie_showings.Movie_Id = movies.Id
        JOIN theater ON movie_showings.Theater_Id = theater.Id
        WHERE movies.Genre = @genre
        ORDER BY movie_showings.ShowTime;
    ";

        command.Parameters.AddWithValue("@genre", genre.ToString());

        using var reader = command.ExecuteReader();

        Console.WriteLine($"\n=== SHOWINGS ({genre}) ===");

        while (reader.Read())
        {
            bool isCulinary = reader.GetInt32(6) == 1;
            double extraPrice = reader.GetDouble(7);

            string culinaryText = isCulinary
                ? $" | Culinary Cinema (+{extraPrice}e)"
                : "";

            Console.WriteLine(
                $"Showing ID: {reader.GetInt32(0)} | " +
                $"Movie: {reader.GetString(1)} | " +
                $"Age: {reader.GetInt32(2)} | " +
                $"Genre: {genre} | " +
                $"Theater: {reader.GetString(4)} | " +
                $"Time: {reader.GetString(5)}" +
                culinaryText
            );
        }
    }

    public bool AddMovieShowing(int movieId, int theaterId, DateTime showTime, bool isCulinary)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        double extraPrice = isCulinary ? 50 : 0;

        var command = connection.CreateCommand();

        command.CommandText = @"
        INSERT INTO movie_showings 
        (Movie_Id, Theater_Id, ShowTime, ExtraPrice, IsCulinary)
        VALUES 
        (@movieId, @theaterId, @showTime, @extraPrice, @isCulinary)";

        command.Parameters.AddWithValue("@movieId", movieId);
        command.Parameters.AddWithValue("@theaterId", theaterId);
        command.Parameters.AddWithValue("@showTime", showTime.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@extraPrice", extraPrice);
        command.Parameters.AddWithValue("@isCulinary", isCulinary ? 1 : 0);

        return command.ExecuteNonQuery() > 0;
    }

    public void PrintSeatsByShowingId(int showingId)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT seats.Seat, seats.IsTaken, seats.PricingType
            FROM movie_showings
            JOIN theater_has_seats 
                ON movie_showings.Theater_Id = theater_has_seats.Theater_Id
            JOIN seats 
                ON theater_has_seats.Seats_Id = seats.Id
            WHERE movie_showings.Id = @id;
        ";

        command.Parameters.AddWithValue("@id", showingId);

        using var reader = command.ExecuteReader();

        Console.WriteLine($"\n=== Seats for Showing {showingId} ===");

        while (reader.Read())
        {
            string seat = reader.GetString(0);
            bool taken = reader.GetInt32(1) == 1;
            string type = reader.GetString(2);

            Console.WriteLine($"Seat: {seat} | Taken: {taken} | Type: {type}");
        }
    }
}