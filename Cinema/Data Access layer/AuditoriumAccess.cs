using Dapper;
using Microsoft.Data.Sqlite;
public class AuditoriumAccess
{
    private const string _databaseLoc = "./cinima.db";

    public Auditorium GetAuditoriumByID(int id)
    {
        List<Seat> seats = [];
        string sql = "SELECT seats_id FROM theater_has_seats WHERE theater_id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        List<int> seatids = connection.Query<int>(sql, new { @ID = id }).ToList();
        foreach (int seatid in seatids)
        {
            sql = "SELECT * FROM seats WHERE id = @ID";
            Seat seat = connection.QuerySingle<Seat>(sql, new { @ID = seatid });
            seats.Add(seat);
        }
        sql = "SELECT Movie FROM theater_has_seats WHERE theater_id = @ID";
        Movie movie = connection.QuerySingle<Movie>(sql, new { @ID = id });
        Auditorium auditorium = new(id, seats, movie);
        return auditorium;
    }

    public void UpdateSeats(Auditorium auditorium)
    {
        int id = auditorium.ID;
        List<Seat> seats = [];
        string sql = "SELECT seats_id FROM theater_has_seats WHERE theater_id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        List<int> seatids = connection.Query<int>(sql, new { @ID = id }).ToList();
        foreach (int seatid in seatids)
        {
            sql = "SELECT * FROM seats WHERE id = @ID";
            Seat seat = connection.QuerySingle<Seat>(sql, new { @ID = seatid });
            seats.Add(seat);
        }
        auditorium.Seats = seats;
    }
}