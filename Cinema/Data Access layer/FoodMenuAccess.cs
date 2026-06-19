using System.Text.Json;

public class FoodMenuAccess
{
    private const string FilePath = "Data Source/BarLoungeMenu.json";

    public List<MenuItem> GetItemsByCategory(string categoryKey)
    {
        var items = new List<MenuItem>();

        // puting json in to a string
        string loungeMenu = File.ReadAllText(FilePath);

        using (JsonDocument doc = JsonDocument.Parse(loungeMenu))
        {
            // getting the json itself, because thats how json was structured
            JsonElement root = doc.RootElement.GetProperty("bar_lounge_menu");
            // get the type of food you need
            JsonElement list = root.GetProperty(categoryKey);

            // making object to be used somewhere else
            foreach (JsonElement element in list.EnumerateArray())
            {
                items.Add(new MenuItem
                {
                    Name = element.GetProperty("name").GetString(),
                    Price = element.GetProperty("price").GetDouble(),
                    Category = element.GetProperty("category").GetString()
                });
            }
        }

        return items;
    }
}

         