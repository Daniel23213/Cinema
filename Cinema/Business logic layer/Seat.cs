public class Seat
{
    private (int x, int y) _coordinates;
    private string _auditorium;
    private string _typePricing;

    public double Price { get; set; }
    public string SeatType { get; set; }
    public (int x, int y) Coordinates
    {
        get { return _coordinates; }
        set
        {
            if (value.x <= 0 || value.y <= 0)
            {
                throw new ArgumentException($"'{value.x}, {value.y}' are not valid coordinates.");
            }

            _coordinates = value;
        }
    }
    public string Auditorium
    {
        get { return _auditorium; }
        set
        {
            if (!ValidAuditoriums.Contains(value))
            {
                throw new ArgumentException($"'{value}' is not a valid auditorium.");
            }

            _auditorium = value;
        }
    }

    public Seat(int x, int y, string auditorium, string typePricing)
    {
        Coordinates = (x, y);
        Auditorium = auditorium;
        SeatType = typePricing;
    }
}