using Dapper;
using Microsoft.Data.Sqlite;
using System.Security.Principal;




public class UserAccess
{
    private SqliteConnection _connection =
    new("Data Source=../../../Data Source/Cinema.db");


    private string Table = "users";

    public bool Write(UserModel account)
    {

        int count = _connection.ExecuteScalar<int>(
     $"SELECT COUNT(*) FROM {Table} WHERE Email = @Email",
     new { Email = account.Email }
 );

        if (count > 0)
        {
            return false; // user already exists
        }
        string sql = $"INSERT INTO {Table} (FirstName, Lastname, Email, Password, Age ) VALUES (@FirstName, @LastName, @Email, @Password, @Age)";
        _connection.Execute(sql, account);
        return true;
    }

    public UserModel GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        return _connection.QueryFirstOrDefault<UserModel>(sql, new { Email = email });
    }

    public UserModel Login(string email, string password)
    {
        password = UserModel.HashPassword(password);
        string sql = $"SELECT * FROM {Table} WHERE email = @Email AND password = @Password";
        return _connection.QueryFirstOrDefault<UserModel>(sql, new { Email = email, Password = password });
    }

    public void Update(UserModel account)
    {
        string sql = $"UPDATE {Table} SET email = @EmailAddress, password = @Password, fullname = @FullName WHERE id = @Id";
        _connection.Execute(sql, account);
    }

    public void Delete(UserModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Execute(sql, new { Id = account.Id });
        account = null;
    }
    public IEnumerable<UserModel> ShowUsers()
    {
     
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<UserModel>(sql).ToList();
    }

    public void Delete(int  id)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Execute(sql, new { Id = id });
    }
    public void UpdatePassword(int id ,string password)
    {
        string sql = $"UPDATE {Table} SET  Password = @Password  WHERE id = @Id";
        _connection.Execute(sql, new { Id = id, Password = UserModel.HashPassword(password) });
    }

    public void ReserveToUser(UserModel user , int seatId, int show_id)
    {
        string sql = "INSERT INTO reservation (Users_Id, Seats_Id, Showing_Id) VALUES (@UserId, @SeatId, @ShowId)";
        _connection.Execute(sql, new { UserId = user.Id, SeatId = seatId, ShowId = show_id });
    }



}