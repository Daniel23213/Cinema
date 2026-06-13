using Dapper;
using Microsoft.Data.Sqlite;
public class AuditoriumAccess
{
    private const string _databaseLoc = "./Cinema.db";
    
    public AuditoriumModel GetAuditoriumByID(int id)
    {
        string sql = "SELECT * FROM theater WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        AuditoriumModel Auditorium = connection.QueryFirst<AuditoriumModel>(sql, new { @ID = id });
        return Auditorium;
    }
}