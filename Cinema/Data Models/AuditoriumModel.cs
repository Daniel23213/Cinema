public class AuditoriumModel
{
    public int ID { get; }
    public int Length { get; }
    public int Width { get; }
    public string Discription { get; }
    
    public AuditoriumModel(int id, int length, int width, string discription)
    {
        ID = id;
        Length = length;
        Width = width;
        Discription = discription;
    }

    public override string ToString() => $"Auditorium ID: {ID}, Length: {Length}, Width: {Width}, Description: {Discription}";
}