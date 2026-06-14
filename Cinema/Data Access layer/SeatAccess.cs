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

    public bool ReserveSeat(int row, int column)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        // Check if seat exists and is free
        var checkCommand = connection.CreateCommand();

        checkCommand.CommandText = @"
    SELECT IsTaken
    FROM seats
    WHERE LocationRow = @row
    AND LocationColumn = @column;
    ";

        checkCommand.Parameters.AddWithValue("@row", row);
        checkCommand.Parameters.AddWithValue("@column", column);

        var result = checkCommand.ExecuteScalar();

        if (result == null)
        {
            Console.WriteLine("Seat does not exist.");
            return false;
        }

        int isTaken = Convert.ToInt32(result);

        if (isTaken == 1)
        {
            Console.WriteLine("Seat is already taken.");
            return false;
        }

        // Reserve seat
        var updateCommand = connection.CreateCommand();

        updateCommand.CommandText = @"
    UPDATE seats
    SET IsTaken = 1
    WHERE LocationRow = @row
    AND LocationColumn = @column;
    ";

        updateCommand.Parameters.AddWithValue("@row", row);
        updateCommand.Parameters.AddWithValue("@column", column);

        updateCommand.ExecuteNonQuery();

        Console.WriteLine($"Seat ({row}, {column}) reserved.");

        return true;
    }
}