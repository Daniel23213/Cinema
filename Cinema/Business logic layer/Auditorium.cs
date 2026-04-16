public class Auditorium
{
    public int ID { get; }
    public List<int> Seats { get; }
    public string CurrentMovie { get; set; }
    
    
    public Auditorium(int id, string currentMovie)
    {
        ID = id;
        Seats = AuditoriumAccess.GetSeatsIDFromAuditoriumID(id);
        CurrentMovie = currentMovie;
    }

    public Seat? FindSeat(int id)
    {
        foreach (int seat in Seats)
        {
            if (seat.ID == id)
                return seat;
        }
        return null;
    }

    public override string ToString() => $"Auditorium {ID} has {Seats.Count} seats and the movie thats airing is {CurrentMovie}";
}