public class Seat : IEquatable<Seat>
{
    private bool _isTaken = false;
    private (int x, int y) _coordinates;
    private static int counter = 0;

    public int ID { get; }
    public decimal Price { get; set; }
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
    public string Theater { get; set; }

    public Seat(int x, int y, string theater, string seatType)
    {
        Coordinates = (x, y);
        Theater = theater;
        SeatType = seatType;

        Price = PriceCalculator.CalculatePrice(seatType);
        ID = counter++;
    }

    public void MakeSeatTaken()
    {
        _isTaken = true;
    }

    public override string ToString()
    {
        return $"ID: {ID}\nTheater: {Theater}\nSeatType: {SeatType}\nCoordinates: {_coordinates}\nPrice: {Price}";
    }

    public bool Equals(Seat other)
    {
        if (other is null) { return false; }

        return this._coordinates == other._coordinates 
            && this.Price == other.Price
            && this.Theater == other.Theater
            && this.SeatType == other.SeatType;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) { return false; }

        if (obj is Seat other) { return Equals(other); } else { return false; }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_coordinates, Price, Theater, SeatType);
    }
}