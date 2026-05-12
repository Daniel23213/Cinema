public enum Prices
{
    normal = 12,
    luxe = 14,
    luxePlus = 16,
}

public enum Ages
{
    senior = 65,
    child = 17,
}

public static class PriceCalculatorLogic
{
    private const decimal seniorDiscount = 0.85M;
    private const decimal childDiscount = 0.85M;

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

    public static decimal CalculatePrice(string seatType, int age)
    {
        decimal basePrice = CalculatePrice(seatType);

        if (age >= (int)Ages.senior)
        {
            return basePrice * seniorDiscount;
        }
        if (age <= (int)Ages.child)
        {
            return basePrice * childDiscount;
        }

        return basePrice;
    }
}