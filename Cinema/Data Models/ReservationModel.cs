public class ReservationModel
{
    public bool _isTaken { get; private set; } = false;

    public int ReservationId { get; set; }
    public string UserName { get; set; }
    public int UserId { get; set; }
    public SeatModel ReservedSeat { get; set; }

    private ReserveSeatAccess _db = new ReserveSeatAccess();

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
           int NewId = _db.SeatReserve(UserId, selectedSeat.ID);
           ReservationId = NewId;
           MakeSeatTaken();
           return $"Seat: {selectedSeat.ID} is sucessfully reserved on {customerName} ID: {ReservationId}";
        }
    }
    public void MakeSeatTaken()
    {
        _isTaken = true;
    }


}