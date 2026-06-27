public static class DietaryDataModel
{
    public static readonly Dictionary<string, string> CheckList = new()
        {
            { "Peanuts", "Severe peanut allergy risk" },
            { "Tree Nuts", "Almonds, walnuts, cashews, pecans, etc." },
            { "Dairy / Milk", "Allergic to dairy proteins" },
            { "Eggs", "Egg allergy" },
            { "Wheat / Gluten", "Celiac disease or wheat allergy" },
            { "Soy", "Soy products" },
            { "Fish", "Finfish (e.g., salmon, cod)" },
            { "Shellfish", "Crustaceans and mollusks (e.g., shrimp, crab, clams)" },
            { "Sesame", "Sesame seeds and oil" },
            { "Vegetarian", "No meat, poultry, or seafood" },
            { "Vegan", "No animal products (meat, dairy, eggs, honey)" },
            { "Lactose Intolerant", "Difficulty digesting dairy" },
            { "Halal", "Requires Halal certified meats / no pork / no alcohol" },
            { "Kosher", "Requires Kosher certified foods" },
            { "No Pork", "Avoids pork products strictly" }
        };
}