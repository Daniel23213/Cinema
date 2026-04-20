using Microsoft.Data.Sqlite;

public class MovieRepository
{
    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";

    public void AddMovie(string title, string author, string genre, TimeSpan duration, DateTime premier)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO movies (Title, Duration, Author, Genre, Premier)
            VALUES (@title, @duration, @author, @genre, @premier);
        ";

        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@duration", duration.ToString());
        command.Parameters.AddWithValue("@author", author);
        command.Parameters.AddWithValue("@genre", genre);
        command.Parameters.AddWithValue("@premier", premier);

        command.ExecuteNonQuery();
    }

    public List<Movies> GetAiringMovies()
    {
        List<Movies> movies = new List<Movies>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Title, Author, Genre, Duration, Premier
            FROM movies
        ";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Movies movie = new Movies(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                TimeSpan.Parse(reader.GetString(4)),
                DateTime.Parse(reader.GetString(5))
            );

            movies.Add(movie);
        }

        return movies;
    }
}