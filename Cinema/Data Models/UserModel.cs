using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class UserModel : IEquatable<UserModel>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }

    public string Role { get; set; }

    public UserModel(string firstName, string lastName, string email, string password , int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = HashPassword(password);
        Age = age;
    }
    public UserModel()
    {

    }

    public static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public bool Equals(UserModel other) 
    {
        if(other == null) 
        {
            return false;
        }
        if(Email == other.Email) 
        {
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"Id{Id} \n FirstName {FirstName} \n LastName {LastName}, \n Email {Email}, \n Age {Age}";
    }

}

