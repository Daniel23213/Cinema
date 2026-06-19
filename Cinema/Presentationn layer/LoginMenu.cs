using System.Text;

static class LoginMenu
{
   
    public static UserModel Show()
    {
        string email = UserInputValidation.NullOrEmptyValidationLoop("Email: ");

        Console.Write("Password: ");
        string password = CreateMyPasswordTextBox();

        UserService service =
            new UserService();

        UserModel user =
            service.Login(email, password);

        if (user == null)
        {
            Console.Write("Invalid credentials.");
            return null;
        }

        Console.WriteLine("Login successful.");
        return user;
    }
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
                if (cki.Key == ConsoleKey.Enter)
                {
                    break; // if user input a Enter break the loop
                }
                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (Password.Length > 0)
                    {
                        Password.Remove(Password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(cki.KeyChar)) //char.IsControl() check if its not arrow button or escape button
                {
                    Password.Append(cki.KeyChar);
                    Console.Write("*"); // console.write whitout line otherwise it will be going down
                }
            }

            // turning stuff out
            Console.TreatControlCAsInput = false;

            return Password.ToString();
        }
    }
}


