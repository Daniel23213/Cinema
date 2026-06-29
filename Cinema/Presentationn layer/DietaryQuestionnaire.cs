public static class DieteryQuestionnarie
{
    public static List<string> Dietary()
    {
        List<string> UserAnswers = [];

        Console.Clear();
        Console.WriteLine("Do you have any allergies and dietary whishes");
        Console.WriteLine("Type 'Yes' to view allergies and dietary restrictions, or 'No' to continue booking.");
        string input0 = Console.ReadLine().ToLower();

        if (input0 == "yes" || input0 == "ja")
        {
            foreach (var item in DietaryDataModel.CheckList)
            {
                Console.WriteLine($"---{item.Key}---");
                Console.WriteLine($"Description: {item.Value}");
                Console.WriteLine("Does this apply to you (Yes / No)");

                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "y" || answer == "ja")
                {
                    UserAnswers.Add(item.Key);
                }
            }
        }
        else if (input0 == "no" || input0 == "n")
        {
            // user skip the allergies and dietary list and continue on.
        }

        return UserAnswers;
    }
}
