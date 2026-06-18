Console.OutputEncoding = System.Text.Encoding.UTF8;

db db = new();
db.InitializeDatabase();
Console.WriteLine("Welcome to the Cinema Booking System!");
Menu.ShowMenu();