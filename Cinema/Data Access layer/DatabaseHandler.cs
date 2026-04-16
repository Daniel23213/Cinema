using Dapper;
using Microsoft.Data.Sqlite;
class DatabaseHandler
{
    private const string _databaseLoc = "./auditorium.db";
    private static readonly List<string> _csvLoc = new() {"./auditorium.csv"};
    public string Name;

    public DatabaseHandler(string table) => Name = table;

    public static void InitialiseTables()
    {
        using SqliteConnection connection = new($"Data Source={_databaseLoc}");
        connection.Open();

        foreach (string auditorium in _csvLoc)
        {
            string name = Path.GetFileNameWithoutExtension(auditorium);
            string sql = $@"
                CREATE TABLE IF NOT EXISTS {name} (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    auditorium STRING NOT NULL,
                    row INTEGER NOT NULL,
                    column INTEGER NOT NULL,
                    type STRING NOT NULL,
                    booked BOOL NOT NULL DEFAULT 0,
                );";
            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
        connection.Close();
    }

    public string GetSeatFromID(int id, string auditorium)
    {
        string sql = "SELECT * FROM auditorium = @Auditorium WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        return connection.QuerySingle<string>(sql, new { @Auditorium = auditorium, @ID = id });
    }

    public void BookSeat(int id, string auditorium)
    {
        string sql = "UPDATE auditorium = @Auditorium SET booked = @Booked WHERE id = @ID";
        SqliteConnection connection = new($"Data Source={_databaseLoc}");
        connection.QuerySingle<string>(sql, new { @Auditorium = auditorium, @ID = id });
    }
}