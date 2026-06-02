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

    public SeatModel? GetById(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
        SELECT Id, LocationRow, LocationColumn, PricingType
        FROM seats
        WHERE Id = @Id;";

        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            int seatId = Convert.ToInt32(reader["Id"]);

            int x = Convert.ToInt32(reader["LocationRow"]);
            int y = Convert.ToInt32(reader["LocationColumn"]);

            string pricingType = reader["PricingType"].ToString() ?? "";

            string theater = "placeholder"; // need to add theater to the table in db and change where its used

            return new SeatModel(x, y, theater, pricingType, seatId);
        }

        return null;
    }
}