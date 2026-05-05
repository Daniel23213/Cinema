public class ReservationModel
{
    public int ReservationId {get; set;}
    public string UserName {get; set;}
    public SeatModel ReservedSeat {get; set;}

    private ReserveSeatAccess _db = new ReserveSeatAccess();

    public ReservationModel(int reservtionid, string username, SeatModel reserveSeat)
    {
        ReservationId = reservtionid;
        UserName = username;
        ReservedSeat = reserveSeat;

    }

    public string SeatAvailble(SeatModel selectedSeat, string customerName, UserModel user)
    {
        if (selectedSeat._isTaken)
        {
            return $"{selectedSeat.ID} is taken";
        }
        else
        {
            _db.SeatReserve(user.Id, selectedSeat.ID);
            selectedSeat.MakeSeatTaken();
            return $"Seat: {selectedSeat.ID} is sucessfully reserved on {customerName} ID: {user.Id}";
        }
    }


}