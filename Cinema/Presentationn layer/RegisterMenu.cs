public static class RegisterMenu
{
    public static void ShowRegisterMenu()
    {
        Console.WriteLine("\n--- Register ---");
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();
        Console.Write("Enter your first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter your last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter your age: ");
        int  age = Convert.ToInt32(Console.ReadLine());
        // Here you would typically call a method to create the account in the database
        // For example:
        UserModel newAccount = new UserModel(firstName, lastName, email, password, age);
        UserAccess accountsAccess = new UserAccess();
        accountsAccess.Write(newAccount);
        Console.WriteLine("Registration successful! You can now log in with your credentials.");
    }
}