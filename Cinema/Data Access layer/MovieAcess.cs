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
        command.CommandText = "SELECT Id, Title, Author, Genre, Duration, Premier FROM movies";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new MovieModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                TimeSpan.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5))
            ));
        }

        return movies;
    }

    public void AddMovie(string title, string author, string genre, TimeSpan duration, DateTime premier)
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
        command.Parameters.AddWithValue("@genre", genre);
        command.Parameters.AddWithValue("@premier", premier.ToString("yyyy-MM-dd HH:mm:ss"));

        command.ExecuteNonQuery();
    }

    public void UpdateMovie(int id, string title, string author, string genre, TimeSpan duration, DateTime premier)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE movies
            SET Title=@title, Author=@author, Genre=@genre, Duration=@duration, Premier=@premier
            WHERE Id=@id";

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@author", author);
        command.Parameters.AddWithValue("@genre", genre);
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
        command.CommandText = "SELECT Id, Title, Author, Genre, Duration, Premier FROM movies WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        return reader.Read();
    }
    public bool AddMovieShowing(int id, int theaterId, DateTime showTime)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO movie_showings (Movie_Id, Theater_Id, ShowTime)
            VALUES (@movieId, @theaterId, @showTime)";
        command.Parameters.AddWithValue("@movieId", id);
        command.Parameters.AddWithValue("@theaterId", theaterId);
        command.Parameters.AddWithValue("@showTime", showTime.ToString("yyyy-MM-dd HH:mm:ss"));
        return command.ExecuteNonQuery() > 0;
    }


    // From here its Showings related methods
    public void GetShowings()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
        SELECT 
            movie_showings.Id,
            movies.Title,
            theater.Description,
            movie_showings.ShowTime
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
            Console.WriteLine(
                $"Showing ID: {reader.GetInt32(0)} | " +
                $"Movie: {reader.GetString(1)} | " +
                $"Theater: {reader.GetString(2)} | " +
                $"Time: {reader.GetString(3)}"
            );

        }
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
    WHERE movie_showings.Id = @Id;
    ";

        command.Parameters.AddWithValue("@Id", showingId);

        using var reader = command.ExecuteReader();

        Console.WriteLine($"\n=== Seats for Showing {showingId} ===");

        while (reader.Read())
        {
            string seat = reader.GetString(0);
            bool isTaken = reader.GetInt32(1) == 1;
            string priceType = reader.GetString(2);

            Console.WriteLine($"Seat: {seat} | Taken: {isTaken} | Type: {priceType}");
        }
    }


}