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
            Console.WriteLine("[A] Assign Admin");
            Console.WriteLine("[R] Remove Admin");
            Console.WriteLine("[E] Exit");

            string input = Console.ReadLine();

            switch (input.ToUpper())
            {
                case "D":

                    Console.Write("User ID: ");
                    int deleteId = Convert.ToInt32(Console.ReadLine());

                    access.DeleteUser(deleteId);

                    Console.WriteLine("User deleted.");
                    break;

                case "A":

                    Console.Write("User ID: ");
                    int adminId = Convert.ToInt32(Console.ReadLine());

                    access.ChangeRole(adminId, "Admin");

                    Console.WriteLine("Admin role assigned.");
                    break;

                case "R":

                    Console.Write("User ID: ");
                    int userId = Convert.ToInt32(Console.ReadLine());

                    access.ChangeRole(userId, "User");

                    Console.WriteLine("Admin role removed.");
                    break;

                case "E":
                    return;
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}