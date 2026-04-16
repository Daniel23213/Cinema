using Dapper;
using Microsoft.Data.Sqlite;
using System.Security.Principal;




public class UserAccess
{
    private SqliteConnection _connection =
    new SqliteConnection("Data Source=../../../Data Source/Cinema.db");


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

    public bool Login(string email , string password)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        int rows = _connection.QueryFirstOrDefault<int>(sql, new { Email = email, Pasword = UserModel.HashPassword(password)});

        if (rows > 0) 
        {
            return true;
        }
        return false;
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
    }



}