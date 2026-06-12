public class ReservationModel
{
    public bool _isTaken { get; private set; } = false;
    public int ReservationId { get; set; }
    public string UserName { get; set; }
    public int UserId { get; set; }
    public SeatModel ReservedSeat { get; set; }

    private ReserveSeatAccess _db = new();

    public ReservationModel(int reservtionid, string username, SeatModel reserveSeat, int userId)
    {
        ReservationId = reservtionid;
        UserId = userId;
        UserName = username;
        ReservedSeat = reserveSeat;

    }

    public string SeatAvailble(SeatModel selectedSeat, string customerName)
    {
        if (_isTaken)
        {
            return $"{selectedSeat.ID} is taken";
        }
        else
        {
           int NewId = _db.SeatReserve(UserId, selectedSeat.ID, _isTaken);
           ReservationId = NewId;
           MakeSeatTaken();
           return $"Seat: {selectedSeat.ID} is sucessfully reserved on {customerName} ID: {ReservationId}";
        }
    }
    public void MakeSeatTaken()
    {
        _isTaken = true;
    }

    //allergies and dietary
    public List<string> Dietary ()
    {
        Dictionary<string, string> CheckList = new ()
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

        List<string> UserAnswers = [];

        Console.Clear();
        Console.WriteLine("Do you have any allergies and dietary whishes");
        Console.WriteLine("Type 'Yes' to view allergies and dietary restrictions, or 'No' to continue booking.");
        string input0 = Console.ReadLine().ToLower();

        if (input0 == "yes" || input0 == "ja")
        {
            foreach (var item in CheckList)
            {
                Console.WriteLine($"---{item.Key}---");
                Console.WriteLine($"Description: {item.Value}");
                Console.WriteLine("Does this apply to you (Yes / No)");

                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "y" || answer == "ja")
                {
                    UserAnswers.Add(item.Key);
                }
            }
        }
        else if (input0 == "no" || input0 == "n")
        {
            // user skip the allergies and dietary list and continue on.
        }

        return UserAnswers;
    }

}   

// As a customer, I want to specify allergies and dietary requirements so suitable food alternatives can be prepared.

// Acceptance Criteria:
// Allergy form during booking
// Dietary options stored with reservation
// Chef/admin can view requirements