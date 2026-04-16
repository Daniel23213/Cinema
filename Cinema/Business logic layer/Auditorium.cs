public class Auditorium
{
    public int ID { get; }
    public List<Seat> Seats { get; }
    public string CurrentMovie { get; set; }
    
    public Auditorium(int id, List<Seat> seats, string CurrentMovie)
    {
        ID = id;
        Seats = seats;
    }

    public Seat? FindSeat(int id)
    {
        foreach (Seat seat in Seats)
        {
            if (seat.ID == id)
                return seat;
        }
        return null;
    }

    public override string ToString() => $"Auditorium {ID} has {Seats.Count} seats and the movie thats airing is {CurrentMovie}";
}