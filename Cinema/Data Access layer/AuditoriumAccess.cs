using Dapper;
using Microsoft.Data.Sqlite;
public class AuditoriumAccess
{
    private const string _databaseLoc = "./Cinema.db";

    public AuditoriumModel GetAuditoriumByID(int id)
    {
        List<SeatModel> seats = [];
        string sql = "SELECT seats_id FROM theater_has_seats WHERE theater_id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        List<int> seatids = connection.Query<int>(sql, new { @ID = id }).ToList();
        foreach (int seatid in seatids)
        {
            sql = "SELECT * FROM seats WHERE id = @ID";
            SeatModel seat = connection.QuerySingle<SeatModel>(sql, new { @ID = seatid });
            seats.Add(seat);
        }
        sql = "SELECT Movie FROM theater_has_seats WHERE theater_id = @ID";
        MovieModel movie = connection.QuerySingle<MovieModel>(sql, new { @ID = id });
        AuditoriumModel auditorium = new(id, seats, movie);
        return auditorium;
    }

    public void UpdateSeats(AuditoriumModel auditorium)
    {
        int id = auditorium.ID;
        List<SeatModel> seats = [];
        string sql = "SELECT seats_id FROM theater_has_seats WHERE theater_id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        List<int> seatids = connection.Query<int>(sql, new { @ID = id }).ToList();
        foreach (int seatid in seatids)
        {
            sql = "SELECT * FROM seats WHERE id = @ID";
            SeatModel seat = connection.QuerySingle<SeatModel>(sql, new { @ID = seatid });
            seats.Add(seat);
        }
        auditorium.Seats = seats;
    }
}