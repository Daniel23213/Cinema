using Dapper;
using Microsoft.Data.Sqlite;
using System.Text;

class db
{
    private const string DatabaseLoc = "../../../Data Source/Cinema.db";

    private string SeatCSV => FindFile("Seats.csv");
    private string TheaterHasSeatsCSV => FindFile("theater_has_seats.csv");


private void SeedSeats(SqliteConnection connection)
{
    Console.WriteLine($"DEBUG: Attempting to open file at: {Path.GetFullPath(SeatCSV)}");

    if (!File.Exists(SeatCSV)) {
        Console.WriteLine("CRITICAL: File does not exist at that path!");
        return;
    }

    var command = connection.CreateCommand();
    command.CommandText = "INSERT INTO seats (Id, Seat, Width, Height, PricingType) VALUES (@Id, @Seat, @Width, @Height, @PricingType)";

    using var transaction = connection.BeginTransaction();
    command.Transaction = transaction;

    int lineCount = 0;
    try
    {
        using (StreamReader reader = new StreamReader(SeatCSV))
        {
            string? header = reader.ReadLine();
            Console.WriteLine($"DEBUG: Header read as: {header}");

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] s = line.Split(',');
                
                // FORCE: This will tell us if your comma separation is working
                if (s.Length < 5) {
                    Console.WriteLine($"DEBUG: Skipping line (Only {s.Length} columns found): {line}");
                    continue;
                }

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Id", s[0].Trim());
                command.Parameters.AddWithValue("@Seat", $"{s[1].Trim()}-{s[2].Trim()}");
                command.Parameters.AddWithValue("@Width", s[2].Trim());
                command.Parameters.AddWithValue("@Height", s[1].Trim());
                command.Parameters.AddWithValue("@PricingType", s[4].Trim());

                command.ExecuteNonQuery();
                lineCount++;
            }
        }
        transaction.Commit();
        Console.WriteLine($"SUCCESS: Seeded {lineCount} rows.");
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        Console.WriteLine($"FATAL ERROR in SeedSeats: {ex.Message}");
    }
}

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
    ('Inception', '02:28:00', 'Nolan', 'SciFi', '2025-01-16 21:00:00', 12),
    ('Batman Begins', '02:20:00', 'Nolan', 'Action', '2025-01-17 20:00:00', 12),
    ('Deadpool', '01:48:00', 'Marvel', 'Comedy', '2025-01-18 22:00:00', 18),
    ('Cars', '01:57:00', 'Pixar', 'Animation', '2025-01-19 14:00:00', 0),
    ('The Godfather', '02:55:00', 'Coppola', 'Crime', '2025-01-20 19:00:00', 18),
    ('Harry Potter', '02:32:00', 'Rowling', 'Fantasy', '2025-01-21 17:00:00', 10),
    ('Doctor Strange', '01:55:00', 'Marvel', 'Fantasy', '2025-01-22 20:00:00', 12),
    ('Finding Nemo', '01:40:00', 'Pixar', 'Animation', '2025-01-23 13:00:00', 0),
    ('Gladiator', '02:35:00', 'Scott', 'Historical', '2025-01-24 21:00:00', 16),
    ('Venom', '01:52:00', 'Marvel', 'Action', '2025-01-25 22:00:00', 16),
    ('Moana', '01:47:00', 'Disney', 'Animation', '2025-01-26 15:00:00', 0),
    ('Black Panther', '02:14:00', 'Marvel', 'Action', '2025-01-27 20:00:00', 12),
    ('Coco', '01:45:00', 'Pixar', 'Animation', '2025-01-28 14:00:00', 0),
    ('Spider-Man', '02:10:00', 'Marvel', 'Action', '2025-01-29 18:00:00', 12),
    ('Up', '01:36:00', 'Pixar', 'Adventure', '2025-01-30 13:00:00', 0),
    ('The Dark Knight', '02:32:00', 'Nolan', 'Action', '2025-02-01 21:00:00', 16),
    ('Avatar', '02:42:00', 'Cameron', 'SciFi', '2025-02-02 20:00:00', 12),
    ('Minions', '01:31:00', 'Illumination', 'Comedy', '2025-02-03 12:00:00', 0),
    ('Thor Ragnarok', '02:10:00', 'Marvel', 'Action', '2025-02-04 19:00:00', 12),
    ('Encanto', '01:42:00', 'Disney', 'Animation', '2025-02-05 14:00:00', 0),
    ('Dune', '02:35:00', 'Villeneuve', 'SciFi', '2025-02-06 21:00:00', 14);
    ";

        command.ExecuteNonQuery();
    }
    private void SeedMovieShowings(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText = @"
    INSERT INTO movie_showings 
    (Movie_Id, Theater_Id, ShowTime, IsCulinary, ExtraPrice)
    VALUES
    (1,1,'2025-06-01 18:00:00',0,0),
    (2,2,'2025-06-01 20:00:00',1,50),
    (3,1,'2025-06-01 14:00:00',0,0),
    (4,1,'2025-06-02 19:00:00',1,50),
    (5,2,'2025-06-02 17:00:00',0,0),
    (6,2,'2025-06-02 13:00:00',0,0),
    (7,1,'2025-06-03 22:00:00',1,50),
    (8,2,'2025-06-03 20:30:00',0,5),
    (9,2,'2025-06-03 12:00:00',0,0),
    (10,1,'2025-06-04 21:00:00',1,50),

    (11,2,'2025-06-04 18:00:00',0,0),
    (12,2,'2025-06-04 22:00:00',1,50),
    (13,1,'2025-06-05 14:00:00',0,0),
    (14,2,'2025-06-05 20:00:00',1,50),
    (15,1,'2025-06-05 17:00:00',0,0),
    (16,1,'2025-06-06 19:30:00',1,50),
    (17,2,'2025-06-06 13:00:00',0,0),
    (18,1,'2025-06-06 21:00:00',1,50),
    (19,1,'2025-06-07 22:30:00',0,5),
    (20,2,'2025-06-07 15:00:00',0,0),

    (21,1,'2025-06-07 20:00:00',1,50),
    (22,1,'2025-06-08 14:00:00',0,0),
    (23,2,'2025-06-08 18:30:00',1,50),
    (24,1,'2025-06-08 12:30:00',0,0),
    (25,1,'2025-06-09 21:00:00',1,50),
    (26,2,'2025-06-09 20:00:00',0,5),
    (27,2,'2025-06-09 11:00:00',0,0),
    (28,1,'2025-06-10 19:00:00',1,50),
    (29,2,'2025-06-10 14:00:00',0,0),
    (30,1,'2025-06-10 21:30:00',1,50);
    ";

        command.ExecuteNonQuery();
    }
    public void InitializeDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseLoc}");
        connection.Open();
        DiagnoseDatabase(connection); // <--- Add this
        Console.ReadLine();

        using (var pragma = connection.CreateCommand())
        {
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();
        }

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

        string theaterTable = @"
        CREATE TABLE IF NOT EXISTS theater (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Description TEXT NOT NULL,
            Width INTEGER,
            Length INTEGER
        );";

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

        string seatsTable = @"
        CREATE TABLE IF NOT EXISTS seats (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Seat TEXT NOT NULL,
            Width INTEGER,
            Height INTEGER,
            PricingType TEXT
        );";

        string theaterSeatsTable = @"
        CREATE TABLE IF NOT EXISTS theater_has_seats (
            Theater_Id INTEGER,
            Seats_Id INTEGER,

            PRIMARY KEY (Theater_Id, Seats_Id),

            FOREIGN KEY (Theater_Id) REFERENCES theater(Id),
            FOREIGN KEY (Seats_Id) REFERENCES seats(Id)
        );";

        string reservationTable = @"
        CREATE TABLE IF NOT EXISTS reservation (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Users_Id INTEGER NOT NULL,
            Seats_Id INTEGER NOT NULL,
            Showing_Id INTEGER NOT NULL,
            isTaken INTEGER DEFAULT 0,

            FOREIGN KEY (Users_Id) REFERENCES users(Id),
            FOREIGN KEY (Seats_Id) REFERENCES seats(Id),
            FOREIGN KEY (Showing_Id) REFERENCES movie_showings(Id)
        );";

        Execute(connection, usersTable);
        Execute(connection, moviesTable);
        Execute(connection, theaterTable);
        Execute(connection, movieShowingsTable);
        Execute(connection, seatsTable);
        Execute(connection, theaterSeatsTable);
        Execute(connection, reservationTable);

        using var countCommand = connection.CreateCommand();
        countCommand.CommandText = "SELECT COUNT(*) FROM movies";

        long count = (long)countCommand.ExecuteScalar();

        if (count == 0)
        {
            Console.WriteLine("Starting seed sequence...");

            try
            {
                Console.WriteLine("Attempting to seed Theaters...");
                SeedTheaters(connection);
            }
            catch (Exception ex) { Console.WriteLine($"Failed at SeedTheaters: {ex.Message}"); }

            try
            {
                Console.WriteLine("Attempting to seed Movies...");
                SeedMovies(connection);
            }
            catch (Exception ex) { Console.WriteLine($"Failed at SeedMovies: {ex.Message}"); }

            try
            {
                Console.WriteLine("Attempting to seed MovieShowings...");
                SeedMovieShowings(connection);
            }
            catch (Exception ex) { Console.WriteLine($"Failed at SeedMovieShowings: {ex.Message}"); }

            try
            {
                Console.WriteLine("Attempting to seed Seats...");
                SeedSeats(connection);
            }
            catch (Exception ex) { Console.WriteLine($"Failed at SeedSeats: {ex.Message}"); }

            try
            {
                Console.WriteLine("Attempting to seed TheaterHasSeats...");
                SeedTheaterHasSeats(connection);
            }
            catch (Exception ex) { Console.WriteLine($"Failed at SeedTheaterHasSeats: {ex.Message}"); }

            Console.WriteLine("Seeding sequence finished.");

            Console.ReadLine();
        }

        using var check = connection.CreateCommand();
        check.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";

        count = (long)countCommand.ExecuteScalar();

        connection.Close();
    }

    private void Execute(SqliteConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    private void SeedTheaters(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText = @"
        INSERT INTO theater (Id, Width, Length, Description) VALUES
        ('1', '12', '14', 'Has a total of 150 seats'),
        ('2', '18', '19', 'Has a total of 300 seats'),
        ('3', '30', '20', 'Has a total of 500 seats')";

        command.ExecuteNonQuery();
    }

    private void SeedTheaterHasSeats(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO theater_has_seats (Theater_Id, Seats_Id)
        VALUES (@Theater_Id, @Seats_Id)";

        // Use a transaction so that if one row fails, the whole process rolls back
        using var transaction = connection.BeginTransaction();
        command.Transaction = transaction;

        try
        {
            using (StreamReader reader = new StreamReader(TheaterHasSeatsCSV))
            {
                reader.ReadLine(); // Skip the header row
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] s = line.Split(',');
                    if (s.Length < 2) continue; // Ignore empty/malformed lines

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Theater_Id", s[0]);
                    command.Parameters.AddWithValue("@Seats_Id", s[1]);

                    command.ExecuteNonQuery();
                }
            }

            transaction.Commit();
            Console.WriteLine("Theater-Seats mapping successfully seeded.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"ERROR in SeedTheaterHasSeats: {ex.Message}");
        }
    }

    public void DiagnoseDatabase(SqliteConnection connection)
    {
        string[] tables = { "seats", "theater", "theater_has_seats" };
        foreach (var table in tables)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(*) FROM {table}";
            long count = (long)cmd.ExecuteScalar();
            Console.WriteLine($"DEBUG: Table '{table}' contains {count} rows.");
        }
    }
    private string FindFile(string fileName)
    {
        // This looks in the same relative path as your DatabaseLoc
        string relativePath = Path.Combine("../../../Data Source/", fileName);
        string fullPath = Path.GetFullPath(relativePath);

        if (!File.Exists(fullPath))
        {
            Console.WriteLine($"DEBUG: Failed to find file at: {fullPath}");

            // Secondary check: If the relative path failed, 
            // check if it's in the bin/Debug folder (the application's base directory)
            string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data Source", fileName);
            if (File.Exists(baseDir))
            {
                Console.WriteLine($"DEBUG: Found file in BaseDirectory instead: {baseDir}");
                return baseDir;
            }
        }

        return fullPath;
    }
}