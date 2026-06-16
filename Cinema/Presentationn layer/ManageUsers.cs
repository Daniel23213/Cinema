static class ManageUsers
{
    public static void Show()
    {
        UserService acess = new();
        var users = acess.GetAllUsers();

        foreach(var user in users) 
        {
            Console.WriteLine(user);
        }

        Console.WriteLine("Delete a user");
        int del_user = Convert.ToInt32(Console.ReadLine());
        foreach (var user in users)
        {
            if(user.Id == del_user) 
            {
                acess.DeleteUser(user.Id);

            }
        }





        }
    }
