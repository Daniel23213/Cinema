public enum Prices
{
    normal = 12,
    luxe = 14,
    luxePlus = 16,
}

public static class PriceCalculatorLogic
{
    // you can overload this method to allow for additional requirements when calculating the price
    public static decimal CalculatePrice(string seatType)
    {
        return seatType.ToLower() switch
        {
            "normal" => (int)Prices.normal,
            "luxe" => (int)Prices.luxe,
            "luxe plus" => (int)Prices.luxePlus,
            _ => throw new ArgumentException($"'{seatType}' is not a valid seat type.")
        };
    }
}