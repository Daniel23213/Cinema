public class UserService
{
    private readonly UserAccess _db = new();

    // ---------------- REGISTER ----------------

    public bool Register(UserModel user)
    {
        if (string.IsNullOrWhiteSpace(user.Email))
            return false;

        if (user.Age < 0)
            return false;

        if (_db.GetByEmail(user.Email) != null)
            return false;

        return _db.InsertUser(user);
    }

    // ---------------- LOGIN ----------------

    public UserModel Login(string email, string password)
    {
        var hashed = UserModel.HashPassword(password);
        return _db.GetByEmail(email)?.Password == hashed
            ? _db.GetByEmail(email)
            : null;
    }

    // ---------------- USERS ----------------

    public IEnumerable<UserModel> GetAllUsers()
    {
        return _db.GetAllUsers();
    }

    public void DeleteUser(int id)
    {
        _db.DeleteUser(id);
    }

    // ---------------- PASSWORD ----------------

    public void ChangePassword(int id, string newPassword)
    {
        if (newPassword.Length < 6)
            throw new Exception("Password too short");

        _db.UpdatePassword(id, UserModel.HashPassword(newPassword));
    }

    // ---------------- RESERVATION ----------------

    public void ReserveTicket(UserModel user, int seatId, int showingId)
    {
        if (user == null)
            throw new Exception("User is null");

        _db.InsertReservation(user.Id, seatId, showingId);
    }

    public List<dynamic> ShowTickets(int userId)
    {
        return _db.GetTickets(userId);
    }

    public bool CancelTicket(int reservationId, int userId)
    {
        return _db.DeleteReservation(reservationId, userId);
    }
}