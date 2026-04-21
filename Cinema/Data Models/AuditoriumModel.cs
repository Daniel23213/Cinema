public class AuditoriumModel
{
    public int ID { get; }
    public List<SeatModel> Seats { get; set; }
    public MovieModel CurrentMovie { get; set; }
    
    
    public AuditoriumModel(int id, List<SeatModel> seats, MovieModel currentMovie)
    {
        ID = id;
        Seats = seats;
        CurrentMovie = currentMovie;
    }

    public override string ToString() => $"Auditorium {ID} has {Seats.Count} seats and the movie thats airing is {CurrentMovie}";
}