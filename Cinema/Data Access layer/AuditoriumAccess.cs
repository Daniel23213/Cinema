using Microsoft.Data.Sqlite;
public class AuditoriumAccess
{
    private const string _databaseLoc = "./Data Source/Cinema.db";
    
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
}