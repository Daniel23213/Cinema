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
    }

    // Happy Scenarios

    [TestMethod]
    public void Register_ValidUser_ReturnsTrue()
    {
        var user = new UserModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = $"test{Guid.NewGuid()}@mail.com",
            Password = UserModel.HashPassword("password123"),
            Age = 25
        };

        bool result = userService.Register(user);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Login_ExistingUser_ReturnsUser()
    {
        string email = $"login{Guid.NewGuid()}@mail.com";
        string password = "password123";

        var user = new UserModel
        {
            FirstName = "Login",
            LastName = "Test",
            Email = email,
            Password = UserModel.HashPassword(password),
            Age = 20
        };

        userService.Register(user);

        var result = userService.Login(email, password);

        Assert.IsNotNull(result);
        Assert.AreEqual(email, result.Email);
    }

    [TestMethod]
    public void GetAllUsers_ReturnsUsers()
    {
        var users = userService.GetAllUsers();

        Assert.IsNotNull(users);
        Assert.IsTrue(users.Any());
    }

    [TestMethod]
    public void DeleteUser_ExistingUser_RemovesUser()
    {
        var user = new UserModel
        {
            FirstName = "Delete",
            LastName = "Me",
            Email = $"delete{Guid.NewGuid()}@mail.com",
            Password = UserModel.HashPassword("password123"),
            Age = 30
        };

        userService.Register(user);

        var createdUser = userService
            .GetAllUsers()
            .Last(u => u.Email == user.Email);

        userService.DeleteUser(createdUser.Id);

        bool exists = userService
            .GetAllUsers()
            .Any(u => u.Id == createdUser.Id);

        Assert.IsFalse(exists);
    }

    [TestMethod]
    public void ChangePassword_ValidPassword_ChangesPassword()
    {
        string email = $"changepass{Guid.NewGuid()}@mail.com";

        var user = new UserModel
        {
            FirstName = "Password",
            LastName = "Test",
            Email = email,
            Password = UserModel.HashPassword("oldpassword"),
            Age = 25
        };

        userService.Register(user);

        var createdUser = userService
            .GetAllUsers()
            .Last(u => u.Email == email);

        userService.ChangePassword(createdUser.Id, "newpassword");

        var loginResult = userService.Login(email, "newpassword");

        Assert.IsNotNull(loginResult);
    }

    [TestMethod]
    public void ReserveTicket_ValidUser_DoesNotThrow()
    {
        var user = userService.GetAllUsers().First();

        userService.ReserveTicket(user, 1, 1);

        Assert.IsTrue(true);
    }

    // Sad Scenarios

    [TestMethod]
    public void Register_EmptyEmail_ReturnsFalse()
    {
        var user = new UserModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "",
            Password = UserModel.HashPassword("password123"),
            Age = 20
        };

        bool result = userService.Register(user);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Register_WhitespaceEmail_ReturnsFalse()
    {
        var user = new UserModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "   ",
            Password = UserModel.HashPassword("password123"),
            Age = 20
        };

        bool result = userService.Register(user);

        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-10)]
    [DataRow(-100)]
    public void Register_NegativeAge_ReturnsFalse(int age)
    {
        var user = new UserModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = $"age{Guid.NewGuid()}@mail.com",
            Password = UserModel.HashPassword("password123"),
            Age = age
        };

        bool result = userService.Register(user);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Login_InvalidCredentials_ReturnsNull()
    {
        var result = userService.Login(
            "notfound@mail.com",
            "wrongpassword");

        Assert.IsNull(result);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-100)]
    [DataRow(99999)]
    public void DeleteUser_NonExistingUser_DoesNothing(int id)
    {
        userService.DeleteUser(id);

        bool exists = userService
            .GetAllUsers()
            .Any(u => u.Id == id);

        Assert.IsFalse(exists);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void ReserveTicket_NullUser_ThrowsException()
    {
        userService.ReserveTicket(null, 1, 1);
    }

}