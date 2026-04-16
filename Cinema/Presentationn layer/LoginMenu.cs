static class LoginMenu 
{
    public static bool  Show() 
    {
        Console.WriteLine("\n--- Login ---");
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();
        UserAccess accountsAccess = new UserAccess();
        if (accountsAccess.Login(email, password) == true)
        {
            Console.WriteLine("Login sucessfull.");

            return true;
            

        }
        else
        {
            Console.WriteLine("Email is taken!");
            return false;
        }

    }
}

