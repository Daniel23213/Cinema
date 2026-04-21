public class Auditorium
{
    public int ID { get; }
    public List<Seat> Seats { get; }
    public string CurrentMovie { get; set; }
    
    
    public Auditorium(int id, List<Seat> seats, string currentMovie)
    {
        ID = id;
        Seats = seats;
        CurrentMovie = currentMovie;
    }

    public override string ToString() => $"Auditorium {ID} has {Seats.Count} seats and the movie thats airing is {CurrentMovie}";
}