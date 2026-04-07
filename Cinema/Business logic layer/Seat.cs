// to set prices go to TypePricing, you can change and add values, also cange calid pricings
public class Seat
{
    public static List<string> ValidAuditoriums = new List<string> { "a", "b", "c" }; // add here the names of valid auditoriums, so this can be expanded
    public static List<string> ValidPricing = new List<string> { "normal", "luxe", "super luxe" }; // add here the names of valid pricings, so this can be expanded

    private (int x, int y) _coordinates;
    private string _auditorium;
    private string _typePricing;

    public double Price { get; set; }
    public string TypePricing
    {
        get { return _typePricing; }
        set
        {
            if (!ValidPricing.Contains(value))
            {
                throw new ArgumentException($"'{value}' is not a valid pricing type.");
            }

            _typePricing = value;

            Price = TypePricing switch
            {
                "normal" => 12.0,
                "luxe" => 14.0,
                "super luxe" => 16.0,
                _ => throw new ArgumentException($"'{value}' is not a valid Price.")
            };
        }
    }
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
        TypePricing = typePricing;
    }
}