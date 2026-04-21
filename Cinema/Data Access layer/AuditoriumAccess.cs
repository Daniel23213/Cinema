using Dapper;
using Microsoft.Data.Sqlite;
public class AuditoriumAccess
{
    private const string _databaseLoc = "./cinima.db";

    public AuditoriumAccess() {}

    public List<int> GetAuditoriumByID(int id)
    {
        List<int> seats = [];
        string sql = "SELECT seats_id FROM theater_has_seats WHERE theater_id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        List<int> seatids = connection.Query<int>(sql, new { @ID = id }).ToList();
        foreach (int seatid in seatids)
        {
            sql = "SELECT * FROM seats WHERE id = @ID";
            int seat = connection.QuerySingle<int>(sql, new { @ID = id });
            seats.Add(seat);
        }
        return seats;
    }
}