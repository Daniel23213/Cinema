static class ManageUsers
{
    public static void Show(UserModel currentUser)
    {
        UserService access = new();

        while (true)
        {
            Console.Clear();

            var users = access.GetAllUsers()
                              .Where(u => u.Id != currentUser.Id);

            foreach (var user in users)
            {
                Console.WriteLine(
                    $"ID: {user.Id} | " +
                    $"{user.FirstName} {user.LastName} | " +
                    $"Role: {user.Role}");
            }

            Console.WriteLine("\n[D] Delete user");
            Console.WriteLine("[M] Give user a monthly ticket");
            Console.WriteLine("[A] Assign Admin");
            Console.WriteLine("[R] Remove Admin");
            Console.WriteLine("[E] Exit");

            string input = Console.ReadLine();
            int id;

            switch (input.ToUpper())
            {
                case "D":

                    Console.Write("User ID: ");
                    id = Convert.ToInt32(Console.ReadLine());

                    access.DeleteUser(id);

                    Console.WriteLine("User deleted.");
                    break;

                case "A":

                    Console.Write("User ID: ");
                    id = Convert.ToInt32(Console.ReadLine());

                    access.ChangeRole(id, "Admin");

                    Console.WriteLine("Admin role assigned.");
                    break;

                case "R":

                    Console.Write("User ID: ");
                    id = Convert.ToInt32(Console.ReadLine());

                    access.ChangeRole(id, "User");

                    Console.WriteLine("Admin role removed.");
                    break;

                case "M":

                    Console.Write("User ID: ");
                    id = Convert.ToInt32(Console.ReadLine());

                    access.GiveUserMonthlyTicketByID(id);

                    Console.WriteLine("User received a monthlty ticket");
                    Console.WriteLine($"Valid until: {access.GetMonthlyTicketByID(id)}");
                    break;

                case "E":
                    return;
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }

    private static void UpdateUser(Action<int> action, string message)
    {
        Console.Write("User ID: ");
        int id = Convert.ToInt32(Console.ReadLine());

        action(id);

        Console.WriteLine(message);
    }
}