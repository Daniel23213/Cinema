using Dapper;
using Microsoft.Data.Sqlite;
using System.Reflection.Emit;

public  class SeatAccess
{
    private SqliteConnection _connection =
    new SqliteConnection("Data Source=../../../Data Source/Cinema.db");

    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";


    public static void AddSeat(string id, bool isTaken, decimal price, string seatType, (int, int) coordinates, string theater)
    {

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