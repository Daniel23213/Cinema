using Microsoft.Data.Sqlite;

class db
{
    private const string DatabaseLoc = "../../../Data Source/Cinema.db"; // ✅ simple & reliable

    public void InitializeDatabase()
    {
        Console.WriteLine("DB Path: " + Path.GetFullPath(DatabaseLoc));

        using var connection = new SqliteConnection($"Data Source={DatabaseLoc}");
        connection.Open();

        // Enable foreign keys
        using (var pragma = connection.CreateCommand())
        {
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();
        }

        // USERS
        string usersTable = @"
        CREATE TABLE IF NOT EXISTS users (
            Id INTEGER PRIMARY KEY,
            Firstname TEXT NOT NULL,
            Lastname TEXT NOT NULL,
            Email TEXT NOT NULL,
            Password TEXT NOT NULL,
            Age INTEGER NOT NULL,
            Role TEXT
        );";

        // MOVIES
        string moviesTable = @"
        CREATE TABLE IF NOT EXISTS movies (
            Id INTEGER PRIMARY KEY,
            Title TEXT,
            Duration TEXT,
            Author TEXT,
            Genre TEXT,
            Premier DATETIME
        );";

        // THEATER
        string theaterTable = @"
        CREATE TABLE IF NOT EXISTS theater (
            Id INTEGER PRIMARY KEY,
            Movies_Id INTEGER,
            Description TEXT,
            FOREIGN KEY (Movies_Id) REFERENCES movies(Id)
        );";

        // SEATS
        string seatsTable = @"
        CREATE TABLE IF NOT EXISTS seats (
            Id INTEGER PRIMARY KEY,
            Seat TEXT,
            IsTaken INTEGER,
            PricingType TEXT
        );";

        // THEATER_HAS_SEATS
        string theaterSeatsTable = @"
        CREATE TABLE IF NOT EXISTS theater_has_seats (
            Theater_Id INTEGER,
            Seats_Id INTEGER,
            PRIMARY KEY (Theater_Id, Seats_Id),
            FOREIGN KEY (Theater_Id) REFERENCES theater(Id),
            FOREIGN KEY (Seats_Id) REFERENCES seats(Id)
        );";

        // RESERVATION
        string reservationTable = @"
        CREATE TABLE IF NOT EXISTS reservation (
            Users_Id INTEGER,
            Seats_Id INTEGER,
            PRIMARY KEY (Users_Id, Seats_Id),
            FOREIGN KEY (Users_Id) REFERENCES users(Id),
            FOREIGN KEY (Seats_Id) REFERENCES seats(Id)
        );";

        // Execute all
        Execute(connection, usersTable);
        Execute(connection, moviesTable);
        Execute(connection, theaterTable);
        Execute(connection, seatsTable);
        Execute(connection, theaterSeatsTable);
        Execute(connection, reservationTable);

        // 🔍 Debug: show tables
        using var check = connection.CreateCommand();
        check.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";

        using var reader = check.ExecuteReader();

        Console.WriteLine("Tables in DB:");
        while (reader.Read())
        {
            Console.WriteLine("- " + reader.GetString(0));
        }

        connection.Close();
    }

    private void Execute(SqliteConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }
}