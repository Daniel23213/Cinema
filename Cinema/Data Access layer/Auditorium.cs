public class Auditorium
{
    public int ID { get; }
    public List<Seat> Seats { get; }
    
    public Auditorium(int id, List<Seat> seats)
    {
        ID = id;
        Seats = seats;
    }

    public FindSeat(int id)
    {
        foreach (Seat seat in Seats)
        {
            if (seat.ID == id)
                return seat;
        }
    }
}