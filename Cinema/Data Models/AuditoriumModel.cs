public class AuditoriumModel
{
    public int ID { get; }
    public int Length { get; }
    public int Width { get; }
    public string Discription { get; }
    
    public AuditoriumModel(int id, int length, int width, string discription)
    {
        ID = id;
        Length = length;
        Width = width;
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
                Size[i, j] = " ";
            }
        }
        List<SeatModel> seats = SeatAccess.GetSeatsByTheater(ID);
        foreach (SeatModel seat in seats)
        {
            string type = seat.SeatType switch
            {
                "VIP" => "$",
                "Premium" => "&",
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

    public override string ToString() => $"Auditorium ID: {ID}, Length: {Length}, Width: {Width}, Description: {Discription}";
}