using Microsoft.Data.Sqlite;
public static class AuditoriumAccess
{
    private const string _databaseLoc = "../../../Data Source/Cinema.db";
    
    public static (int width, int length) GetAuditoriumMeasurementsByID(int id)
    {
        string query = "SELECT Width, Length FROM theater WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        connection.Open();
        using SqliteCommand command = new(query, connection);
        command.Parameters.AddWithValue("@ID", id);
        SqliteDataReader result = command.ExecuteReader();
        result.Read();
        (int Width, int Length) measurements = (Convert.ToInt32(result["Width"]), Convert.ToInt32(result["Length"]));
        connection.Close();
        return measurements;
    }

    public static int GetAuditoriumIdByShowingId(int showingId)
    {
        string query = @"SELECT theater.Id FROM movie_showings JOIN theater ON movie_showings.Theater_Id = theater.Id WHERE movie_showings.Id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        connection.Open();
        using SqliteCommand command = new(query, connection);
        command.Parameters.AddWithValue("@ID", showingId);
        SqliteDataReader result = command.ExecuteReader();
        result.Read();
        int id = Convert.ToInt32(result["Id"]);
        connection.Close();
        return id;
    }
}