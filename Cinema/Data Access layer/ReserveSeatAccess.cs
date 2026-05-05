using Microsoft.Data.Sqlite;

public class ReserveSeatAccess
{
    private const string ConnectionString = "Data Source=../../../Data Source/Cinema.db";

    public void SeatReserve(int usserId, int seatId)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var sql = "INSERT INTO reservation (usserId, seatId) VALUES (@usserId, @seatId)";

        using var command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@usserId", usserId);   
        command.Parameters.AddWithValue("@seatId", seatId);

        command.ExecuteNonQuery();
    }
}