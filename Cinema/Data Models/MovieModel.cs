using System.ComponentModel;
using System.Runtime.InteropServices;

public class MovieModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public MoviesGenres Genre { get; set; }
    public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1.5);
    public DateTime Premier { get; set; }
    public int Age { get; set; }

    public MovieModel(int id, string title, string author, string genre, TimeSpan duration, DateTime premier, int age)
    {
        Id = id;
        Title = title;
        Author = author;
        Genre = genre;
        Duration = duration;
        Premier = premier;
        Age = age; 
    }

    public override string ToString()
    {
        return $"ID: {Id} Name:{Title} by {Author} and the Duration is: {Duration} Age: {Age}";
    }



}