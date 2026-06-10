public interface IUserAccess
{
    // ---------------- USERS ----------------

    bool InsertUser(UserModel user);

    UserModel GetByEmail(string email);

    IEnumerable<UserModel> GetAllUsers();

    void DeleteUser(int id);

    void UpdatePassword(int id, string hashedPassword);

    // ---------------- RESERVATIONS ----------------

    void InsertReservation(int userId, int seatId, int showingId);

    List<dynamic> GetTickets(int userId);

    bool DeleteReservation(int reservationId, int userId);
}