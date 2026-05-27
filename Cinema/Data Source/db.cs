using Microsoft.Data.Sqlite;

class db
{
    private const string DatabaseLoc = "../../../Data Source/Cinema.db"; // ✅ simple & reliable

    private void SeedMovies(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText = @"
        INSERT INTO movies (Title, Duration, Author, Genre, Premier, Age) VALUES
        ('Avengers', '02:30:00', 'Marvel', 'Action', '2025-01-01 18:00:00', 12),
        ('Joker', '02:02:00', 'DC', 'Drama', '2025-01-02 20:00:00', 18),
        ('Toy Story', '01:30:00', 'Pixar', 'Comedy', '2025-01-03 16:00:00', 6),
        ('Interstellar', '02:49:00', 'Nolan', 'SciFi', '2025-01-05 19:00:00', 12),
        ('Titanic', '03:15:00', 'Cameron', 'Drama', '2025-01-06 17:00:00', 12),
        ('Shrek', '01:35:00', 'DreamWorks', 'Comedy', '2025-01-07 15:00:00', 0),
        ('John Wick', '01:50:00', 'Stahelski', 'Action', '2025-01-11 22:00:00', 16),
        ('The Matrix', '02:16:00', 'Wachowski', 'SciFi', '2025-01-09 21:00:00', 16),
        ('Frozen', '01:42:00', 'Disney', 'Animation', '2025-01-10 14:00:00', 0),
        ('Inception', '02:28:00', 'Nolan', 'SciFi', '2025-01-16 21:00:00', 12);
    ";

        command.ExecuteNonQuery();
    }

    public void InitializeDatabase()
    {
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
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
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
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Title TEXT NOT NULL,
            Duration TEXT NOT NULL,
            Author TEXT,
            Genre TEXT,
            Premier DATETIME,
            Age INTEGER NOT NULL
        );";

        // THEATER / AUDITORIUMS
        string theaterTable = @"
        CREATE TABLE IF NOT EXISTS theater (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Description TEXT NOT NULL
        );";

        // MOVIE SHOWINGS
        string movieShowingsTable = @"
        CREATE TABLE IF NOT EXISTS movie_showings (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Movie_Id INTEGER NOT NULL,
            Theater_Id INTEGER NOT NULL,
            ShowTime DATETIME NOT NULL,
            IsCulinary INTEGER DEFAULT 0,
            ExtraPrice REAL DEFAULT 0,


            FOREIGN KEY (Movie_Id) REFERENCES movies(Id),
            FOREIGN KEY (Theater_Id) REFERENCES theater(Id)
        );";

        // SEATS
        string seatsTable = @"
        CREATE TABLE IF NOT EXISTS seats (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Seat TEXT NOT NULL,
            IsTaken INTEGER DEFAULT 0,
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

        // RESERVATIONS
        string reservationTable = @"
        CREATE TABLE IF NOT EXISTS reservation (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Users_Id INTEGER NOT NULL,
            Seats_Id INTEGER NOT NULL,
            Showing_Id INTEGER NOT NULL,

            FOREIGN KEY (Users_Id) REFERENCES users(Id),
            FOREIGN KEY (Seats_Id) REFERENCES seats(Id),
            FOREIGN KEY (Showing_Id) REFERENCES movie_showings(Id)
        );";

        // Execute tables
        Execute(connection, usersTable);
        Execute(connection, moviesTable);
        Execute(connection, theaterTable);
        Execute(connection, movieShowingsTable);
        Execute(connection, seatsTable);
        Execute(connection, theaterSeatsTable);
        Execute(connection, reservationTable);

        SeedMovies(connection);



        // Debug: show all tables
        using var check = connection.CreateCommand();
        check.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";

        using var reader = check.ExecuteReader();

        connection.Close();
    }

    private void Execute(SqliteConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }
}