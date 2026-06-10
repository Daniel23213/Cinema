using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

[TestClass]

public class UserServiceTests
{
    private UserService userService;

    [TestInitialize]
    public void Setup()
    {
        userService = new UserService();

        var baseDir = AppContext.BaseDirectory;

        var testFolder = new DirectoryInfo(baseDir);

        var targetPath = Path.GetFullPath(
            Path.Combine(testFolder.FullName,
            "..\\..\\..\\..\\Data Source"));

        Directory.CreateDirectory(targetPath);
        var sourceDb = Path.Combine(AppContext.BaseDirectory, "Cinema.db");
        var destDb = Path.Combine(targetPath, "Cinema.db");

        if (File.Exists(sourceDb))
        {
            File.Copy(sourceDb, destDb, true);
        }
    }


    // ------------------------
    // HAPPY PATH
    // ------------------------

    [TestMethod]
    public void Register_ValidUser_ReturnsTrue()
    {
        var user = new UserModel
        {
            FirstName = "Test",
            LastName = "User",
            Email = $"test_{Guid.NewGuid()}@mail.com",
            Password = UserModel.HashPassword("password123"),
            Age = 20
        };

        var result = userService.Register(user);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Login_ValidUser_ReturnsUser()
    {
        string email = $"login_{Guid.NewGuid()}@mail.com";

        var user = new UserModel
        {
            FirstName = "Login",
            LastName = "User",
            Email = email,
            Password = UserModel.HashPassword("password123"),
            Age = 25
        };

        userService.Register(user);

        var result = userService.Login(email, "password123");

        Assert.IsNotNull(result);
        Assert.AreEqual(email, result.Email);
    }

    [TestMethod]
    public void GetAllUsers_ReturnsAtLeastOneUser()
    {
        var users = userService.GetAllUsers();

        Assert.IsNotNull(users);
        Assert.IsTrue(users.Any());
    }

    [TestMethod]
    public void DeleteUser_ExistingUser_RemovesUser()
    {
        var email = $"delete_{Guid.NewGuid()}@mail.com";

        var user = new UserModel
        {
            FirstName = "Delete",
            LastName = "User",
            Email = email,
            Password = UserModel.HashPassword("password123"),
            Age = 30
        };

        userService.Register(user);

        var created = userService.GetAllUsers()
            .Last(u => u.Email == email);

        userService.DeleteUser(created.Id);

        var exists = userService.GetAllUsers()
            .Any(u => u.Id == created.Id);

        Assert.IsFalse(exists);
    }

    [TestMethod]
    public void ChangePassword_ValidPassword_AllowsLogin()
    {
        string email = $"pass_{Guid.NewGuid()}@mail.com";

        var user = new UserModel
        {
            FirstName = "Pass",
            LastName = "User",
            Email = email,
            Password = UserModel.HashPassword("oldpass"),
            Age = 25
        };

        userService.Register(user);

        var created = userService.GetAllUsers()
            .Last(u => u.Email == email);

        userService.ChangePassword(created.Id, "newpassword");

        var loginResult = userService.Login(email, "newpassword");

        Assert.IsNotNull(loginResult);
    }

    [TestMethod]
    public void ReserveTicket_ValidInput_DoesNotThrow()
    {
        var user = userService.GetAllUsers().First();

        int seatId = 1;
        int showingId = 1;

        userService.ReserveTicket(user, seatId, showingId);
    }

    // ------------------------
    // SAD PATH
    // ------------------------

    [TestMethod]
    public void Register_EmptyEmail_ReturnsFalse()
    {
        var user = new UserModel
        {
            FirstName = "Test",
            LastName = "User",
            Email = "",
            Password = UserModel.HashPassword("password"),
            Age = 20
        };

        var result = userService.Register(user);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Register_NegativeAge_ReturnsFalse()
    {
        var user = new UserModel
        {
            FirstName = "Test",
            LastName = "User",
            Email = $"bad_{Guid.NewGuid()}@mail.com",
            Password = UserModel.HashPassword("password"),
            Age = -1
        };

        var result = userService.Register(user);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Login_InvalidCredentials_ReturnsNull()
    {
        var result = userService.Login("fake@mail.com", "wrong");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void DeleteUser_NonExistingUser_DoesNothing()
    {
        userService.DeleteUser(-99999);

        Assert.IsTrue(true);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void ChangePassword_ShortPassword_Throws()
    {
        userService.ChangePassword(1, "123");
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void ReserveTicket_NullUser_Throws()
    {
        userService.ReserveTicket(null, 1, 1);
    }
}