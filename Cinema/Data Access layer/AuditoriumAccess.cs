using Dapper;
using Microsoft.Data.Sqlite;
public class AuditoriumAccess
{
    private const string _databaseLoc = "./Cinema.db";

    public AuditoriumModel GetAuditoriumByID(int id)
    {
        string sql = "SELECT seats_id FROM theater_has_seats WHERE theater_id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        List<int> seatids = connection.Query<int>(sql, new { @ID = id }).ToList();
        List<SeatModel> seats = [];
        foreach (int seatid in seatids)
        {
            sql = "SELECT * FROM seats WHERE id = @ID";
            SeatModel seat = connection.QueryFirst<SeatModel>(sql, new { @ID = id });
            seats.Add(seat);
        }

        
        sql = "SELECT movie_id FROM theater WHERE id = @ID";
        MovieModel movie = connection.QuerySingle<MovieModel>(sql, new { @ID = id });

        sql = "SELECT Discription FROM theater WHERE id = @ID";
        string discription = connection.QuerySingle<string>(sql, new { @ID = id });
        
        AuditoriumModel auditorium = new(id, seats, movie, discription);
        return auditorium;
    }

    public List<AuditoriumModel> GetAllAuditorium()
    {
        string sql = "SELECT COUNT(*) FROM theater";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        int amount = connection.QuerySingle<int>(sql);

        List<AuditoriumModel> auditoriums = [];
        for (int i = 1; i <= amount; i++)
        {
            auditoriums.Add(GetAuditoriumByID(i));
        }

        return auditoriums;
    }
}