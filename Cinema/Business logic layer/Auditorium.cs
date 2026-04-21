public class Auditorium
{
    public int ID { get; }
    public List<int> Seats { get; }
    public string CurrentMovie { get; set; }
    
    
    public Auditorium(int id, List<int> seats, string currentMovie)
    {
        ID = id;
        Seats = seats;
        CurrentMovie = currentMovie;
    }

    public override string ToString() => $"Auditorium {ID} has {Seats.Count} seats and the movie thats airing is {CurrentMovie}";
}