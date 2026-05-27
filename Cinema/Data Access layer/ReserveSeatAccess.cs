using Microsoft.Data.Sqlite;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;

public class ReserveSeatAccess
{
    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";

    public int SeatReserve(int userId, int seatId, string seatType, int age, int showingId)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();



        decimal price = PriceCalculatorLogic.GetPrice(seatType, age);

        //var sql = "INSERT INTO reservation (usserId, seatId, isTaken) VALUES (@usserId, @seatId, @isTaken); SELECT last_insert_rowid();";
        var sql = @"INSERT INTO reservation (Users_Id, Seats_Id, Showing_Id, Price) 
                VALUES (@UserId, @SeatId, @ShowingId, @Price); 
                SELECT last_insert_rowid();";

        using var command = new SqliteCommand(sql, connection);
        //command.Parameters.AddWithValue("@usserId", usserId);   
        //command.Parameters.AddWithValue("@seatId", seatId);
        //command.Parameters.AddWithValue("@isTaken", isTaken);

        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@SeatId", seatId);
        command.Parameters.AddWithValue("@ShowingId", showingId);
        command.Parameters.AddWithValue("@Price", price);

        return Convert.ToInt32(command.ExecuteScalar());
    }

    
}