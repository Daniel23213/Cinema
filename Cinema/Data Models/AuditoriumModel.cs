public class AuditoriumModel
{
    public int ID { get; }
    public int Length { get; }
    public int Width { get; }
    public List<SeatModel> Seats { get; set; }
    public MovieModel CurrentMovie { get; set; }
    
    public string Discription { get; set; }
    
    public AuditoriumModel(int id, int length, int width, List<SeatModel> seats, MovieModel currentMovie, string discription)
    {
        ID = id;
        Length = length;
        Width = width;
        Seats = seats;
        CurrentMovie = currentMovie;
        Discription = discription;
    }

    public string ShowAuditoriumDiagram()
    {
        string Diagram = "";
        string[,] Size = new string[Length, Width];
        for (int i = 0; i < Size.GetLength(0); i++)
        {
            for (int j = 0; j < Size.GetLength(1); j++)
            {
                Size[i, j] = ".";
            }
        }
        foreach (SeatModel seat in Seats)
        {
            string type = seat.SeatType switch
            {
                "Standard" => "$",
                "Premium" => "&",
                "VIP" => "%",
                _ => "#"
            };
            Size[seat.Coordinates.x, seat.Coordinates.y] = type;
        }
        for (int i = 0; i < Size.GetLength(0); i++)
        {
            for (int j = 0; j < Size.GetLength(1); j++)
            {
                Diagram += Size[i, j];
            }
            Diagram += "\n";
        }
        return Diagram;
    }

    public override string ToString() => $"Auditorium {ID} has {Seats.Count} seats and the movie thats airing is {CurrentMovie}";
}