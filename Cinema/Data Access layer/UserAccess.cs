using Dapper;
using Microsoft.Data.Sqlite;

public class UserAccess : IUserAccess
{
    private readonly string _connectionString =
        "Data Source=../../../Data Source/Cinema.db";

    private SqliteConnection CreateConnection()
        => new SqliteConnection(_connectionString);

    // ---------------- USERS ----------------

    public bool InsertUser(UserModel user)
    {
        using var conn = CreateConnection();
        conn.Open();

        string sql = @"
            INSERT INTO users (FirstName, Lastname, Email, Password, Age)
            VALUES (@FirstName, @LastName, @Email, @Password, @Age);
        ";

        return conn.Execute(sql, user) > 0;
    }

    public UserModel GetByEmail(string email)
    {
        using var conn = CreateConnection();
        conn.Open();

        return conn.QueryFirstOrDefault<UserModel>(
            "SELECT * FROM users WHERE Email = @Email",
            new { Email = email }
        );
    }

    public IEnumerable<UserModel> GetAllUsers()
    {
        using var conn = CreateConnection();
        conn.Open();

        return conn.Query<UserModel>("SELECT * FROM users");
    }

    public void DeleteUser(int id)
    {
        using var conn = CreateConnection();
        conn.Open();

        conn.Execute("DELETE FROM users WHERE Id = @Id", new { Id = id });
    }

    public void UpdatePassword(int id, string hashedPassword)
    {
        using var conn = CreateConnection();
        conn.Open();

        conn.Execute(
            "UPDATE users SET Password = @Password WHERE Id = @Id",
            new { Id = id, Password = hashedPassword }
        );
    }

    // ---------------- RESERVATIONS ----------------

    public void InsertReservation(int userId, int seatId, int showingId)
    {
        using var conn = CreateConnection();
        conn.Open();

        string sql = @"
        INSERT INTO reservation (Users_Id, Seats_Id, Showing_Id)
        VALUES (@UserId, @SeatId, @ShowId);
    ";

        conn.Execute(sql, new
        {
            UserId = userId,
            SeatId = seatId,
            ShowId = showingId
        });
    }

    public List<dynamic> GetTickets(int userId)
    {
        using var conn = CreateConnection();
        conn.Open();

        string sql = @"
            SELECT 
                r.Id AS ReservationId,
                u.Firstname,
                u.Lastname,
                m.Title AS MovieTitle,
                ms.ShowTime,
                s.Seat
            FROM reservation r
            JOIN users u ON u.Id = r.Users_Id
            JOIN movie_showings ms ON ms.Id = r.Showing_Id
            JOIN movies m ON m.Id = ms.Movie_Id
            JOIN seats s ON s.Id = r.Seats_Id
            WHERE r.Users_Id = @userId;
        ";

        return conn.Query(sql, new { userId }).ToList();
    }

    public bool DeleteReservation(int reservationId, int userId)
    {
        using var conn = CreateConnection();
        conn.Open();

        return conn.Execute(@"
            DELETE FROM reservation
            WHERE Id = @id AND Users_Id = @userId;
        ", new { id = reservationId, userId }) > 0;
    }
}