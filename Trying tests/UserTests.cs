using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class UserServiceTests
{
    private UserService userService;

    [TestInitialize]
    public void Setup()
    {
        // You can pass a fake or null here if your service allows it
        userService = new UserService();
    }

    // ---------------- VALIDATION TESTS ----------------

    [TestMethod]
    public void Register_EmptyEmail_ReturnsFalse()
    {
        var user = new UserModel
        {
            FirstName = "Test",
            LastName = "User",
            Email = "",
            Password = "123456",
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
            Email = "test@mail.com",
            Password = "123456",
            Age = -5
        };

        var result = userService.Register(user);

        Assert.IsFalse(result);
    }


    // ---------------- PASSWORD LOGIC ----------------

    [TestMethod]
    public void ChangePassword_ShortPassword_ThrowsException()
    {
        Assert.ThrowsException<System.Exception>(() =>
        {
            userService.ChangePassword(1, "123");
        });
    }





    // ---------------- RESERVATION VALIDATION ----------------

    [TestMethod]
    public void ReserveTicket_NullUser_ThrowsException()
    {
        Assert.ThrowsException<System.Exception>(() =>
        {
            userService.ReserveTicket(null, 1, 1);
        });
    }

}