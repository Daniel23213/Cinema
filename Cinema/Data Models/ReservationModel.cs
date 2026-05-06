public class ReservationModel
{
    public int ReservationId {get; set;}
    public string UserName {get; set;}
    public int UserId {get; set;}
    public SeatModel ReservedSeat {get; set;}

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
        if (selectedSeat._isTaken)
        {
            return $"{selectedSeat.ID} is taken";
        }
        else
        {
           int NewId = _db.SeatReserve(UserId, selectedSeat.ID);
           ReservationId = NewId;
           selectedSeat.MakeSeatTaken();
           return $"Seat: {selectedSeat.ID} is sucessfully reserved on {customerName} ID: {ReservationId}";
        }
    }


}