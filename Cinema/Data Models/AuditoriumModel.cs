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
        SeatAccess access = new();
        string Diagram = "";
        string[,] Size = new string[Length, Width];
        for (int i = 0; i < Size.GetLength(0); i++)
        {
            for (int j = 0; j < Size.GetLength(1); j++)
            {
                Size[i, j] = " ";
            }
        }
        List<SeatModel> seats = access.GetSeatsByTheater(ID);
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
    public void PrintAuditoriumDiagram()
    {
        Console.Clear();

        SeatAccess access = new();
        SeatModel[,] diagram = new SeatModel[Length + 1, Width + 1];
        List<SeatModel> seats = access.GetSeatsByTheater(ID);

        Console.WriteLine($"DEBUG: Loaded {seats.Count} seats for theater ID {this.ID}");

        // Assign seat to location
        foreach (SeatModel seat in seats)
        {
            if (seat.Coordinates.y < diagram.GetLength(0) && seat.Coordinates.x < diagram.GetLength(1))
            {
                diagram[seat.Coordinates.y, seat.Coordinates.x] = seat;
            }
        }

        // Print header numbers
        Console.Write("   ");
        for (int i = 1; i < diagram.GetLength(1); i++)
        {
            Console.Write($" {i,3}");
        }
        Console.WriteLine();

        
        // Print auditorium
        SeatModel? currentSeat;
        for (int i = diagram.GetLength(0) - 1; i > 0; i--)
        {
            Console.Write($"{(char)('A' + i - 1),2} ");
            for (int j = 1; j < diagram.GetLength(1); j++)
            {
                Console.Write("|");
                currentSeat = diagram[i, j];
                if (currentSeat is null)
                {
                    Console.Write("   ");
                    continue;
                }
                if (access.IsSeatTaken(currentSeat.ID))
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($" X ");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                Console.ForegroundColor = currentSeat.SeatType.ToLower() switch
                {
                    "luxe plus" => ConsoleColor.Red,    // VIP
                    "luxe" => ConsoleColor.Yellow, // Premium
                    "normal" => ConsoleColor.Blue,   // Standard
                    _ => ConsoleColor.Gray    // Catch-all
                };

                Console.Write($"{" O "}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
        }
        Console.WriteLine("Grey Seats: Unavaliable");
        Console.WriteLine("Red Seats: VIP");
        Console.WriteLine("Yellow Seats: Premium");
        Console.WriteLine("Blue Seats: Standard");
    }

    public override string ToString() => $"Auditorium ID: {ID}, Length: {Length}, Width: {Width}, Description: {Discription}";
}