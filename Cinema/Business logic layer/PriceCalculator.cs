public static class PriceCalculator
{
    // you can overload this method to allow for additional requirements when calculating the price
    public static int CalculatePrice(string seatType)
    {
        return seatType switch
        {
            "normal" => 12,
            "luxe" => 12,
            "luxe plus" => 12,
            _ => throw new ArgumentException($"'{seatType}' is not a valid seat type.")
        };
    }
}