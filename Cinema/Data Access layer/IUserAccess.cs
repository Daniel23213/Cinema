public interface IUserAccess
{
    bool Write(UserModel user);
    UserModel Login(string email, string password);
    IEnumerable<UserModel> ShowUsers();
    void Delete(int id);
    void UpdatePassword(int id, string password);
    void ReserveToUser(UserModel user, int seatId, int showingId);
}