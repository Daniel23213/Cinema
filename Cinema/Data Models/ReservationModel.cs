public class ReservationModel
{
    public bool _isTaken { get; private set; } = false;
    public int ReservationId { get; set; }
    public string UserName { get; set; }
    public int UserId { get; set; }
    public SeatModel ReservedSeat { get; set; }

    private ReserveSeatAccess _db = new();

    public ReservationModel()
    {
        // keep this empty
    }

    public ReservationModel(int reservtionid, string username, SeatModel reserveSeat, int userId)
    {
        ReservationId = reservtionid;
        UserId = userId;
        UserName = username;
        ReservedSeat = reserveSeat;

    }


}