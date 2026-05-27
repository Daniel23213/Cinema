using Dapper;
using Microsoft.Data.Sqlite;
using System.Reflection.Emit;

public  class SeatAccess
{
    private SqliteConnection _connection =
    new("Data Source=../../../Data Source/Cinema.db");

    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";


    public void AddSeat(string seatName, int width, int height, string pricingType)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO seats (Seat, Width, Height, PricingType)
        VALUES (@Seat, @Width, @Height, @PricingType)";

        command.Parameters.AddWithValue("@Seat", seatName);
        command.Parameters.AddWithValue("@Width", width);
        command.Parameters.AddWithValue("@Height", height);
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

    // sits can no longer be reserved
    //public bool ReserveSeat(string seat)
    //{
    //    using var connection = new SqliteConnection(ConnectionString);
    //    connection.Open();

    //    // 1. Check if seat exists and is free
    //    var checkCommand = connection.CreateCommand();
    //    checkCommand.CommandText = @"
    //    SELECT IsTaken
    //    FROM seats
    //    WHERE Seat = @Seat;
    //";

    //    checkCommand.Parameters.AddWithValue("@Seat", seat);

    //    var result = checkCommand.ExecuteScalar();

    //    if (result == null)
    //    {
    //        Console.WriteLine("Seat does not exist.");
    //        return false;
    //    }

    //    int isTaken = Convert.ToInt32(result);

    //    if (isTaken == 1)
    //    {
    //        Console.WriteLine("Seat is already taken.");
    //        return false;
    //    }

    //    // 2. Update seat to taken
    //    var updateCommand = connection.CreateCommand();
    //    updateCommand.CommandText = @"
    //    UPDATE seats
    //    SET IsTaken = 1
    //    WHERE Seat = @Seat;
    //";

    //    updateCommand.Parameters.AddWithValue("@Seat", seat);

    //    updateCommand.ExecuteNonQuery();

        
    //    return true;
    //}

    public SeatModel? GetById(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
        SELECT Id, Width, Height, PricingType
        FROM seats
        WHERE Id = @Id;";

        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            int seatId = Convert.ToInt32(reader["Id"]);

            int x = Convert.ToInt32(reader["Width"]);
            int y = Convert.ToInt32(reader["Height"]);

            string pricingType = reader["PricingType"].ToString() ?? "";

            string theater = "placeholder"; // need to add theater to the table in db and change where its used

            return new SeatModel(x, y, theater, pricingType, seatId);
        }

        return null;
    }
}