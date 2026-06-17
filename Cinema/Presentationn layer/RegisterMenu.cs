using System.Text;
public static class RegisterMenu
{
    public static UserModel ShowRegisterMenu()
    {
        Console.WriteLine("\n--- Register ---");

        string email = UserInputValidation.NullOrEmptyValidationLoop("Enter your email: ");

        Console.Write("Enter your password: ");
        string password = CreateMyPasswordTextBox();

        Console.WriteLine();

        string firstName = UserInputValidation.NullOrEmptyValidationLoop("Enter your first name: ");

        string lastName = UserInputValidation.NullOrEmptyValidationLoop("Enter your last name: ");

        int age = UserInputValidation.IntInputValidation("Enter your age: ");

        // Here you would typically call a method to create the account in the database
        // For example:
        UserModel newAccount = new(firstName, lastName, email, password, age);
        UserService accountsAccess = new();
        if(accountsAccess.Register(newAccount)== true) 
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
 
    // Vivesh code hashing input field when typing the password
    public static string CreateMyPasswordTextBox()
    {
        {
            StringBuilder Password = new StringBuilder();
            ConsoleKeyInfo cki;
            // Prevent example from ending if CTL+C is pressed.
            Console.TreatControlCAsInput = true;
 
            while (true)
            {
                cki = Console.ReadKey(true);
                // if user press Enter then we stop the loop
                if(cki.Key == ConsoleKey.Enter)
                {
                    break; // if user input a Enter break the loop
                }
                if(cki.Key == ConsoleKey.Backspace)
                {
                    if(Password.Length > 0)
                    {
                        Password.Remove(Password.Length -1, 1);
                        Console.Write("\b \b");
                    }
                }
                //char.IsControl() check if its not arrow button or escape button
                else if(!char.IsControl(cki.KeyChar)) 
                {
                    Password.Append(cki.KeyChar);

                    // console.write whitout line otherwise it will be going down
                    Console.Write("*");
                }
            }

            // turning hashing off
            Console.TreatControlCAsInput = false;

            return Password.ToString();
        }
    }

}