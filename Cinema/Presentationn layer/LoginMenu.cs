static class LoginMenu
{
   
    public static UserModel Show()
    {
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = RegisterMenu.CreateMyPasswordTextBox();

        Console.WriteLine();

       
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


