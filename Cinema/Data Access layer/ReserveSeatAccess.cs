using System.ComponentModel.Design;
using Microsoft.Data.Sqlite;

public class ReserveSeatAccess
{
    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";

    public int SeatReserve(int usserId, int seatId, bool isTaken)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var sql = "INSERT INTO reservation (usserId, seatId, isTaken) VALUES (@usserId, @seatId, @isTaken); SELECT last_insert_rowid();";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@usserId", usserId);   
        command.Parameters.AddWithValue("@seatId", seatId);
        command.Parameters.AddWithValue("@isTaken", isTaken);

        return Convert.ToInt32(command.ExecuteScalar());
    }    
}