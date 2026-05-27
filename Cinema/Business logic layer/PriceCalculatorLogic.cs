/*
    When changing the values for ages, prices or discounts modify the enums or const,
    do not change values in code, there is a build in logic that applies
    the highest discount available so no need to adjust the order of discounts
*/

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
        private const decimal childDiscount = 0.75M;
        private const decimal studentDiscount = 0.80M;

        private static decimal GetBasePrice(string seatType)
        {
            return seatType.ToLower() switch
            {
                "normal" => (int)Prices.normal,
                "luxe" => (int)Prices.luxe,
                "luxe plus" => (int)Prices.luxePlus,
                _ => throw new ArgumentException($"'{seatType}' is not a valid seat type.")
            };
        }

        public static decimal GetPrice(string seatType, int age, bool isStudent = false)
        {
            decimal basePrice = GetBasePrice(seatType);
            decimal discount = 1.0M;

            if (age >= (int)Ages.senior) { discount = seniorDiscount; }
            else if (age <= (int)Ages.child) { discount = childDiscount; }

            if (isStudent)
            {
                if (discount > studentDiscount) { discount = studentDiscount; }
            }

            return basePrice * discount;
        }
    }
