public static class PriceCalculatorLogic
{
    // you can overload this method to allow for additional requirements when calculating the price
    public static decimal CalculatePrice(string seatType)
    {
        return seatType switch
        {
            "normal" => 12,
            "luxe" => 14,
            "luxe plus" => 16,
            _ => throw new ArgumentException($"'{seatType}' is not a valid seat type.")
        };
    }
}