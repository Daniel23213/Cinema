using System.Text;

public static class RegisterMenu
{
    public static UserModel ShowRegisterMenu()
    {
        Console.WriteLine("\n--- Register ---");
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = CreateMyPasswordTextBox();
        Console.Write("Enter your first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter your last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter your age: ");
        int  age = Convert.ToInt32(Console.ReadLine());
        // Here you would typically call a method to create the account in the database
        // For example:
        UserModel newAccount = new(firstName, lastName, email, password, age);
        UserAccess accountsAccess = new();
        if(accountsAccess.Write(newAccount)== true) 
        {
            Console.WriteLine("Register sucessfull.");
            newAccount.ToString();
            return newAccount;
        }
        else 
        {
            Console.WriteLine("Email is taken!");
            return null;
        }   
    }

    //Vivesh code here hashing input field when making a password
    public static string CreateMyPasswordTextBox()
    {
        {
            StringBuilder Password = new StringBuilder();
            ConsoleKeyInfo cki;
            // Prevent example from ending if CTL+C is pressed.
            Console.TreatControlCAsInput = true;

            Console.WriteLine("Enter your password: \n");
            do {
                cki = Console.ReadKey(true);
                if(cki.Key == ConsoleKey.Enter)
                {
                    break; // if user input a Enter break the loop
                }
                Password.Append(cki.KeyChar);
                Console.WriteLine("*");
            } while (cki.Key != ConsoleKey.Enter);
            return Password.ToString();
        }
    }
}