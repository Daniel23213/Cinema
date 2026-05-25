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
        command.CommandText = @"
            SELECT Id, Title, Author, Genre, Duration, Premier
            FROM movies";

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
            ));
        }

        return movies;
    }

    public List<MovieModel> GetAiringMovies(MoviesGenres genre)
    {
        var movies = new List<MovieModel>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Title, Author, Genre, Duration, Premier
            FROM movies
            WHERE Genre = @genre";

        command.Parameters.AddWithValue("@genre", genre.ToString());

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
            ));
        }

        return movies;
    }

    public void AddMovie(string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO movies (Title, Duration, Author, Genre, Premier)
            VALUES (@title, @duration, @author, @genre, @premier)";

        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@duration", duration.ToString());
        command.Parameters.AddWithValue("@author", author);
        command.Parameters.AddWithValue("@genre", genre.ToString());
        command.Parameters.AddWithValue("@premier", premier.ToString("yyyy-MM-dd HH:mm:ss"));

        command.ExecuteNonQuery();
    }

    public void UpdateMovie(int id, string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE movies
            SET Title = @title,
                Author = @author,
                Genre = @genre,
                Duration = @duration,
                Premier = @premier
            WHERE Id = @id";

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@author", author);
        command.Parameters.AddWithValue("@genre", genre.ToString());
        command.Parameters.AddWithValue("@duration", duration.ToString());
        command.Parameters.AddWithValue("@premier", premier.ToString("yyyy-MM-dd HH:mm:ss"));

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

    public void GetShowings()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT 
                movie_showings.Id,
                movies.Title,
                movies.Genre,
                theater.Description,
                movie_showings.ShowTime
            FROM movie_showings
            JOIN movies ON movie_showings.Movie_Id = movies.Id
            JOIN theater ON movie_showings.Theater_Id = theater.Id
            ORDER BY movie_showings.ShowTime;
        ";

        using var reader = command.ExecuteReader();

        Console.WriteLine("\n=== Movie Showings ===");

        while (reader.Read())
        {
            MoviesGenres genre = Enum.Parse<MoviesGenres>(reader.GetString(2));

            Console.WriteLine(
                $"ID: {reader.GetInt32(0)} | " +
                $"Movie: {reader.GetString(1)} | " +
                $"Genre: {genre} | " +
                $"Theater: {reader.GetString(3)} | " +
                $"Time: {reader.GetString(4)}"
            );
        }
    }

    public void GetShowingsByGenre(MoviesGenres genre)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT 
                movie_showings.Id,
                movies.Title,
                movies.Genre,
                theater.Description,
                movie_showings.ShowTime
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
            var parsedGenre = Enum.Parse<MoviesGenres>(reader.GetString(2));

            Console.WriteLine(
                $"ID: {reader.GetInt32(0)} | " +
                $"Movie: {reader.GetString(1)} | " +
                $"Genre: {parsedGenre} | " +
                $"Theater: {reader.GetString(3)} | " +
                $"Time: {reader.GetString(4)}"
            );
        }
    }

    public bool AddMovieShowing(int movieId, int theaterId, DateTime showTime)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO movie_showings (Movie_Id, Theater_Id, ShowTime)
            VALUES (@movieId, @theaterId, @showTime)";

        command.Parameters.AddWithValue("@movieId", movieId);
        command.Parameters.AddWithValue("@theaterId", theaterId);
        command.Parameters.AddWithValue("@showTime", showTime.ToString("yyyy-MM-dd HH:mm:ss"));

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