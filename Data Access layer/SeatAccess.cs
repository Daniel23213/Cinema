using Dapper;
using Microsoft.Data.Sqlite;
using System.Reflection.Emit;

public class SeatAccess
{
    private SqliteConnection _connection =
    new("Data Source=../../../Data Source/Cinema.db");

    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";

    public List<SeatModel> GetSeatsByTheater(int theater)
    {
        var seatsList = new List<SeatModel>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
        SELECT 
            s.Id,
            s.Seat,
            s.Width,
            s.Height,
            s.PricingType
        FROM 
            seats s
        JOIN 
            theater_has_seats ths ON s.Id = ths.Seats_Id
        JOIN 
            theater t ON ths.Theater_Id = t.Id
        WHERE 
            t.Description = @TheaterDescription";

        cmd.Parameters.AddWithValue("@TheaterDescription", theater);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int x = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
            int y = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
            string seatType = reader.IsDBNull(4) ? "Standard" : reader.GetString(4);

            var seat = new SeatModel(x, y, seatType, id);

            seatsList.Add(seat);
        }

        return seatsList;
    }

    public void AddSeat(string seatName, bool isTaken, string pricingType)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO seats (Seat, IsTaken, PricingType)
        VALUES (@Seat, @IsTaken, @PricingType)";

        command.Parameters.AddWithValue("@Seat", seatName);
        command.Parameters.AddWithValue("@IsTaken", isTaken ? 1 : 0);
        command.Parameters.AddWithValue("@PricingType", pricingType);

        command.ExecuteNonQuery();
    }

    public int GetId(int row , int col) 
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
    SELECT Id
    FROM seats
    WHERE LocationRow = @row AND LocationColumn = @col;
    ";

        command.Parameters.AddWithValue("@row", row);
        command.Parameters.AddWithValue("@col", col);

        var result = command.ExecuteScalar();
        int res = Convert.ToInt32(result);
        return res;
       

    }
    public int GetId(string seatName)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
    SELECT Id
    FROM seats
    WHERE Seat = @seatName;
    ";

        command.Parameters.AddWithValue("@seatName", seatName);

        var result = command.ExecuteScalar();
        int res = Convert.ToInt32(result);
        return res;


    }

    public bool ReserveSeat(UserModel user, string seatName, int showingId)
    {
        int seatId = GetId(seatName);

        if (seatId == 0)
        {
            Console.WriteLine("Seat not found!");
            return false;
        }

        if (IsSeatTaken(showingId, seatName))
        {
            Console.WriteLine("Seat already taken!");
            return false;
        }

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
        INSERT INTO reservation (Users_Id, Seats_Id, Showing_Id)
        VALUES (@userId, @seatId, @showingId);
    ";

        cmd.Parameters.AddWithValue("@userId", user.Id);
        cmd.Parameters.AddWithValue("@seatId", seatId);
        cmd.Parameters.AddWithValue("@showingId", showingId);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Ticket reserved successfully!");
        return true;
    }

    public void PrintSeatsByShowingId(int showingId)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
        SELECT 
            seats.Id,
            seats.Seat,
            seats.Width,
            seats.Height,
            seats.PricingType
        FROM movie_showings
        JOIN theater_has_seats 
            ON movie_showings.Theater_Id = theater_has_seats.Theater_Id
        JOIN seats 
            ON theater_has_seats.Seats_Id = seats.Id
        WHERE movie_showings.Id = @id
        ORDER BY seats.Width, seats.Height;
    ";

        cmd.Parameters.AddWithValue("@id", showingId);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine($"\n=== Seats for Showing {showingId} ===");

        while (reader.Read())
        {
            int seatId = reader.GetInt32(0);
            string seatName = reader.GetString(1);
            int row = reader.GetInt32(2);
            int col = reader.GetInt32(3);
            string type = reader.GetString(4);

            bool taken = IsSeatTaken(showingId, seatName);

            Console.WriteLine(
                $"Seat {seatName} (Row {row}, Col {col}) | " +
                $"{(taken ? "X Taken" : " Available")} | " +
                $"{type}"
            );
        }
    }
    public bool IsSeatTaken(int showingId, string seat)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
        SELECT COUNT(*)
        FROM reservation r
        JOIN seats s ON s.Id = r.Seats_Id
        WHERE r.Showing_Id = @showingId
        AND s.Seat = @seat;
    ";

        cmd.Parameters.AddWithValue("@showingId", showingId);
        cmd.Parameters.AddWithValue("@seat", seat);

        long count = (long)cmd.ExecuteScalar();

        return count > 0;
    }
}