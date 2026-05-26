using Dapper;
using Microsoft.Data.Sqlite;
using System.Reflection.Emit;

public  class SeatAccess
{
    private SqliteConnection _connection =
    new("Data Source=../../../Data Source/Cinema.db");

    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";


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

    public int GetId(string seat) 
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
        SELECT Id
        FROM seats
        WHERE Seat = @Seat;
    ";

        command.Parameters.AddWithValue("@Seat", seat);

        var result = command.ExecuteScalar();
        int res = Convert.ToInt32(result);
        return res;
       

    }

    public bool ReserveSeat(string seat)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        // 1. Check if seat exists and is free
        var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = @"
        SELECT IsTaken
        FROM seats
        WHERE Seat = @Seat;
    ";

        checkCommand.Parameters.AddWithValue("@Seat", seat);

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

        // 2. Update seat to taken
        var updateCommand = connection.CreateCommand();
        updateCommand.CommandText = @"
        UPDATE seats
        SET IsTaken = 1
        WHERE Seat = @Seat;
    ";

        updateCommand.Parameters.AddWithValue("@Seat", seat);

        updateCommand.ExecuteNonQuery();

        
        return true;
    }
}