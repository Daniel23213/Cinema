using Dapper;
using Microsoft.Data.Sqlite;
class DatabaseHandler
{
    private const string _databaseLoc = "./auditorium.db";

    public DatabaseHandler() {}

    public Seat GetSeatFromID(int id)
    {
        string sql = "SELECT * FROM seats WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        return connection.QuerySingle<Seat>(sql, new { @ID = id });
    }

    public void BookSeat(int id, bool istaken)
    {
        string sql = "UPDATE seats SET istaken = @Istaken WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        connection.QuerySingle<string>(sql, new { @Istaken = istaken, @ID = id });
    }

    public Auditorium GetAuditoriumFromID(int id)
    {
        string sql = "SELECT * FROM auditorium WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        return connection.QuerySingle<Auditorium>(sql, new { @ID = id });
    }

    public Auditorium GetAuditoriumFromID(Seat seat)
    {
        int id = seat.ID;
        string sql = "SELECT * FROM auditorium WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        return connection.QuerySingle<Auditorium>(sql, new { @ID = id });
    }
}