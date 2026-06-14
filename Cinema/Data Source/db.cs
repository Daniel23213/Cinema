using Microsoft.Data.Sqlite;
using System.Text;

public class db
{
    private const string DatabaseLoc = "./Data Source/Cinema.db";
    private const string SeatCSV = "./Data Source/auditroiums.csv";

    private void SeedSeats(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        using (StreamReader reader = new StreamReader(SeatCSV))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] seat = line.Split(',');
                command.CommandText = @"
                INSERT INTO seats (Id, Name, LocationRow, LocationColumn, PricingType)
                VALUES (@Id, @Name, @LocationRow, @LocationColumn, @PricingType)";

                command.Parameters.AddWithValue("@Id", seat[0]);
                command.Parameters.AddWithValue("@Name", seat[1]);
                command.Parameters.AddWithValue("@LocationRow", seat[2]);
                command.Parameters.AddWithValue("@LocationColumn", seat[3]);
                command.Parameters.AddWithValue("@PricingType", seat[4]);
                command.ExecuteNonQuery();
            }
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

        // THEATER
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
            Name TEXT NOT NULL,
            LocationRow INTEGER,
            LocationColumn INTEGER,
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
            isTaken INTEGER DEFAULT 0,

            FOREIGN KEY (Users_Id) REFERENCES users(Id),
            FOREIGN KEY (Seats_Id) REFERENCES seats(Id),
            FOREIGN KEY (Showing_Id) REFERENCES movie_showings(Id)
        );";

        // Create tables
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
            try
            {
                SeedMovies(connection);
                SeedMovieShowings(connection);

                Console.WriteLine("Database seeded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Debug: show all tables
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

    // =========================
    // SEED THEATERS
    // =========================

    private void SeedTheaters(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText = @"
        INSERT INTO theater (Id, Description)
        VALUES
        (1, 'Main Theater'),
        (2, 'VIP Theater');
        ";

        command.ExecuteNonQuery();
    }

    // =========================
    // SEED SEATS
    // =========================

    //private void SeedSeats(SqliteConnection connection)
    //{
    //    var command = connection.CreateCommand();

    //    command.CommandText = @"
    //    INSERT INTO seats
    //    (Id, LocationRow, LocationColumn, IsTaken, PricingType)
    //    VALUES
    //    (1,1,1,0,'normal'),
    //    (2,1,2,0,'normal'),
    //    (3,1,3,0,'normal'),
    //    (4,1,4,0,'normal'),
    //    (5,1,5,0,'normal'),

    //    (6,2,1,0,'normal'),
    //    (7,2,2,0,'normal'),
    //    (8,2,3,0,'normal'),
    //    (9,2,4,0,'normal'),
    //    (10,2,5,0,'normal'),

    //    (11,3,1,0,'normal'),
    //    (12,3,2,0,'normal'),
    //    (13,3,3,0,'luxe'),
    //    (14,3,4,0,'luxe'),
    //    (15,3,5,0,'normal'),

    //    (16,4,1,0,'normal'),
    //    (17,4,2,0,'normal'),
    //    (18,4,3,0,'luxe'),
    //    (19,4,4,0,'luxe'),
    //    (20,4,5,0,'normal'),

    //    (21,5,1,0,'normal'),
    //    (22,5,2,0,'normal'),
    //    (23,5,3,0,'normal'),
    //    (24,5,4,0,'normal'),
    //    (25,5,5,0,'normal'),

    //    (26,1,1,0,'normal'),
    //    (27,1,2,0,'normal'),
    //    (28,1,3,0,'normal'),
    //    (29,1,4,0,'normal'),
    //    (30,1,5,0,'normal'),

    //    (31,2,1,0,'normal'),
    //    (32,2,2,0,'normal'),
    //    (33,2,3,0,'normal'),
    //    (34,2,4,0,'normal'),
    //    (35,2,5,0,'normal'),

    //    (36,3,1,0,'normal'),
    //    (37,3,2,0,'normal'),
    //    (38,3,3,0,'VIP'),
    //    (39,3,4,0,'VIP'),
    //    (40,3,5,0,'normal'),

    //    (41,4,1,0,'normal'),
    //    (42,4,2,0,'normal'),
    //    (43,4,3,0,'VIP'),
    //    (44,4,4,0,'VIP'),
    //    (45,4,5,0,'normal'),

    //    (46,5,1,0,'normal'),
    //    (47,5,2,0,'normal'),
    //    (48,5,3,0,'normal'),
    //    (49,5,4,0,'normal'),
    //    (50,5,5,0,'normal');
    //    ";

    //    command.ExecuteNonQuery();
    //}

    //// =========================
    //// BIND SEATS TO THEATERS
    //// =========================

    private void SeedTheaterSeats(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        var sql = new StringBuilder();

        // Seats 1-25 => Theater 1
        for (int i = 1; i <= 25; i++)
        {
            sql.AppendLine(
                $"INSERT INTO theater_has_seats (Theater_Id, Seats_Id) VALUES (1, {i});"
            );
        }

        // Seats 26-50 => Theater 2
        for (int i = 26; i <= 50; i++)
        {
            sql.AppendLine(
                $"INSERT INTO theater_has_seats (Theater_Id, Seats_Id) VALUES (2, {i});"
            );
        }

        command.CommandText = sql.ToString();

        command.ExecuteNonQuery();
    }

    // =========================
    // SEED MOVIES
    // =========================

    //private void SeedMovies(SqliteConnection connection)
    //{
    //    var command = connection.CreateCommand();

    //    command.CommandText = @"
    //    INSERT INTO movies 
    //    (Title, Duration, Author, Genre, Premier, Age)
    //    VALUES
    //    ('Avengers', '02:30:00', 'Marvel', 'Action', '2025-01-01', 12),
    //    ('Joker', '02:02:00', 'DC', 'Drama', '2025-01-02', 18),
    //    ('Toy Story', '01:30:00', 'Pixar', 'Comedy', '2025-01-03', 6),
    //    ('Interstellar', '02:49:00', 'Nolan', 'SciFi', '2025-01-05', 12),
    //    ('Titanic', '03:15:00', 'Cameron', 'Drama', '2025-01-06', 12);
    //    ";

    //    command.ExecuteNonQuery();
    //}

    //// =========================
    //// SEED SHOWINGS
    //// =========================

    //private void SeedMovieShowings(SqliteConnection connection)
    //{
    //    var command = connection.CreateCommand();

    //    command.CommandText = @"
    //    INSERT INTO movie_showings 
    //    (Movie_Id, Theater_Id, ShowTime, IsCulinary, ExtraPrice)
    //    VALUES
    //    (1,1,'2025-06-01 18:00:00',0,0),
    //    (2,2,'2025-06-01 20:00:00',1,50),
    //    (3,1,'2025-06-01 14:00:00',0,0),
    //    (4,2,'2025-06-02 19:00:00',1,50),
    //    (5,1,'2025-06-02 17:00:00',0,0);
    //    ";

    //    command.ExecuteNonQuery();
    //}
}