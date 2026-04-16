using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class UserModel
{
    public int Id; 
    public string FirstName; 
    public string LastName;
    public string Email;
    public string Password;
    private int counter = 0;
    public int Age;

    public UserModel(string firstName, string lastName, string email, string password , int age)
    {
        Id = counter++;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = Convert.ToString(password.GetHashCode());
        Age = age;
    }

}

