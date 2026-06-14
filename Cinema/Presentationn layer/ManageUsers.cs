static class ManageUsers
{
    public static void Show()
    {
        UserAccess acess = new();
        var users = acess.ShowUsers();

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
                acess.Delete(user);

            }
        }





        }
    }
