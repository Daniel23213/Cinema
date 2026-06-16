public static class UserInputValidation
{
    public static string NullOrEmptyValidationLoop(string message)
    {
        string input = null;
        do
        {
            Console.WriteLine(message);

            input = Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(input));

        return input;
    }

    public static int IntInputValidation(string message)
    {
        string inputString = null;
        int inputInt = default(int);

        do
        {
            inputString = UserInputValidation.NullOrEmptyValidationLoop(message);
        }
        while (!int.TryParse(inputString, out inputInt));

        return inputInt;
    }
}