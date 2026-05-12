public class AuditoriumModel
{
    public int ID { get; }
    public List<SeatModel> Seats { get; set; }
    public MovieModel CurrentMovie { get; set; }
    
    public string Discription { get; set; }
    
    public AuditoriumModel(int id, List<SeatModel> seats, MovieModel currentMovie, string discription)
    {
        ID = id;
        Seats = seats;
        CurrentMovie = currentMovie;
        Discription = discription;
    }

    public override string ToString() => $"Auditorium {ID} has {Seats.Count} seats and the movie thats airing is {CurrentMovie}";
}