static class LoginMenu 
{
    public static UserModel  Show() 
    {
        Console.WriteLine("\n--- Login ---");
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();
        UserAccess accountsAccess = new UserAccess();
        if (accountsAccess.Login(email, password) != null)
        {
            Console.WriteLine("Login sucessfull.");

            return accountsAccess.Login(email, password);
            

        }
        else
        {
            Console.WriteLine("Wrong name or  password!");
            return null;
        }

    }
}

