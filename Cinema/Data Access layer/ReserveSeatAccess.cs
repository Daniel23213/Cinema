using System.ComponentModel.Design;
using Microsoft.Data.Sqlite;

public class ReserveSeatAccess
{
    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";

    public int SeatReserve(int usserId, int seatId)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var sql = "INSERT INTO reservation (usserId, seatId) VALUES (@usserId, @seatId); SELECT last_insert_rowid();";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@usserId", usserId);   
        command.Parameters.AddWithValue("@seatId", seatId);

        return Convert.ToInt32(command.ExecuteScalar());
    }
}