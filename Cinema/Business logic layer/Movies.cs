using System.ComponentModel;
using System.Runtime.InteropServices;

public class Movies
{
    // nog een ID maken
    public string Title {get;}
    public string Author {get;}
    public string Genres {get;}
    public TimeSpan Duration { get; private set; } = TimeSpan.FromHours(1.5);
    public DateTime Premier {get; set;}

    public Movies(string title, string author, string genres)
    {
        Title = title;
        Author = author;
        Genres = genres;
    }

    public override string ToString()
    {
        return $"Name:{Title} by {Author} and the Duration is: {Duration}";
    } 


    
}