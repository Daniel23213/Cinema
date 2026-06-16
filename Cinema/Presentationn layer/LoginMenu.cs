static class LoginMenu
{
   
    public static UserModel Show()
    {
        string email = UserInputValidation.NullOrEmptyValidationLoop("Email: ");

        string password = UserInputValidation.NullOrEmptyValidationLoop("Password: ");

        UserService service =
            new UserService();

        UserModel user =
            service.Login(email, password);

        if (user == null)
        {
            Console.WriteLine("Invalid credentials.");
            return null;
        }

        Console.WriteLine("Login successful.");
        return user;
    }
}


