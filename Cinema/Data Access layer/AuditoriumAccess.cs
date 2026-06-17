using Microsoft.Data.Sqlite;
public class AuditoriumAccess
{
    private const string _databaseLoc = "./Cinema.db";
    
    public AuditoriumModel GetAuditoriumByID(int id)
    {
        string query = "SELECT * FROM theater WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        connection.Open();
        using SqliteCommand command = new(query, connection);
        command.Parameters.AddWithValue("@ID", id);
        SqliteDataReader result = command.ExecuteReader();
        result.Read();
        AuditoriumModel? auditorium = new
        (
            Convert.ToInt32(result["Id"]),
            Convert.ToInt32(result["Width"]),
            Convert.ToInt32(result["Length"]),
            Convert.ToString(result["Description"])
            
        );
        connection.Close();
        return auditorium;
    }

    public AuditoriumModel GetAuditoriumByShowingID(int id)
    {
        string query = @"SELECT theater.Id, theater.Width, theater.Length, theater.Description FROM movie_showings JOIN theater ON movie_showings.Theater_Id = theater.Id WHERE movie_showings.Id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        connection.Open();
        using SqliteCommand command = new(query, connection);
        command.Parameters.AddWithValue("@ID", id);
        SqliteDataReader result = command.ExecuteReader();
        result.Read();
        AuditoriumModel? auditorium = new
        (
            Convert.ToInt32(result["Id"]),
            Convert.ToInt32(result["Width"]),
            Convert.ToInt32(result["Length"]),
            Convert.ToString(result["Description"])
            
        );
        connection.Close();
        return auditorium;
    }
}