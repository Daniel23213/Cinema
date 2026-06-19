using Dapper;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;
public class AuditoriumAccess
{
    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";
    private const string _databaseLoc = "./Cinema.db";
    
    public AuditoriumModel GetAuditoriumByID(int id)
    {
        string sql = "SELECT * FROM theater WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        AuditoriumModel Auditorium = connection.QueryFirst<AuditoriumModel>(sql, new { @ID = id });
        return Auditorium;
    }
}