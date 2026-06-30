public static class Diagram
{
    public static void PrintAuditoriumDiagram(int AuditoriumId)
    {
        SeatAccess seatAccess = new();
        var measurements = AuditoriumAccess.GetAuditoriumMeasurementsByID(AuditoriumId);
        SeatModel[,] diagram = new SeatModel[measurements.length + 1, measurements.width + 1];
        List<SeatModel> seats = seatAccess.GetSeatsByTheater(AuditoriumId);

        // Assign seat to location
        foreach (SeatModel seat in seats)
        {
            diagram[seat.Coordinates.x, seat.Coordinates.y] = seat;
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
                if (seatAccess.IsSeatTaken(currentSeat.ID))
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($" X ");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                Console.ForegroundColor = currentSeat.SeatType switch
                {
                    "luxe plus" => ConsoleColor.Red,
                    "luxe" => ConsoleColor.Yellow,
                    _ => ConsoleColor.Blue
                };
                Console.Write($"{currentSeat.Name,3}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
        }
        Console.WriteLine("Grey Seats: Unavaliable");
        Console.WriteLine("Red Seats: VIP");
        Console.WriteLine("Yellow Seats: Premium");
        Console.WriteLine("Blue Seats: Standard");
    }
}