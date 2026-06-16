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

    public void PrintAuditoriumDiagram()
    {
        SeatAccess access = new();
        string[,] Diagram = new string[Length, Width];
        List<SeatModel> seats = access.GetSeatsByTheater(ID);
        Console.WriteLine(seats.Count);
        foreach (SeatModel seat in seats)
        {
            Diagram[seat.Coordinates.y, seat.Coordinates.x] = seat.SeatType;
        }
        Console.Write("  ");
        for (int i = 0; i < Diagram.GetLength(1); i++)
        {
            Console.Write($"{i,2} ");
        }
        Console.WriteLine();
        for (int i = 0; i < Diagram.GetLength(0); i++)
        {
            Console.Write($"{(char)('A' + i),2}");
            for (int j = 0; j < Diagram.GetLength(1); j++)
            {
                
                if (Diagram[i, j] == "VIP")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (Diagram[i, j] == "Premium")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                Console.Write($"|{Diagram[i, j].Name,2}");
            }
            Console.WriteLine();
        }
    }

    public override string ToString() => $"Auditorium ID: {ID}, Length: {Length}, Width: {Width}, Description: {Discription}";
}