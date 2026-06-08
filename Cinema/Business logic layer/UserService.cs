public class UserService
{
    private readonly UserAccess _userAccess;

    public UserService()
    {
        _userAccess = new UserAccess();
    }

    public bool Register(UserModel user)
    {
        if (string.IsNullOrWhiteSpace(user.Email))
            return false;

        if (user.Age < 0)
            return false;

        return _userAccess.Write(user);
    }

    public UserModel Login(string email, string password)
    {
        return _userAccess.Login(email, password);
    }

    public IEnumerable<UserModel> GetAllUsers()
    {
        return _userAccess.ShowUsers();
    }

    public void DeleteUser(int id)
    {
        _userAccess.Delete(id);
    }

    public void DeleteUser(UserModel user)
    {
        _userAccess.Delete(user.Id);
    }


    public void ChangePassword(int id, string password)
    {
        if (password.Length < 6)
            throw new Exception("Password too short.");

        _userAccess.UpdatePassword(id, password);
    }

    public void ReserveTicket(UserModel user, int seatId, int showingId)
    {
        if (user == null)
            throw new Exception("User must be logged in.");

        _userAccess.ReserveToUser(user, seatId, showingId);
    }
}