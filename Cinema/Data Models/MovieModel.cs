using System.ComponentModel;
using System.Runtime.InteropServices;

public class MovieModel
{
    public int Id { get; set; }
    public string Title { get; }
    public string Author { get; }
    public string Genre { get; }
    public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1.5);
    public DateTime Premier { get; set; }

    public MovieModel(int id, string title, string author, string genre, TimeSpan duration, DateTime premier)
    {
        Id = id;
        Title = title;
        Author = author;
        Genre = genre;
        Duration = duration;
        Premier = premier;
    }

    public override string ToString()
    {
        return $"ID: {Id} Name:{Title} by {Author} and the Duration is: {Duration}";
    }



}