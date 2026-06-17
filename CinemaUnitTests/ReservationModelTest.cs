using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ReservationSystem.Tests
{
    [TestClass]
    public class ReservationModelTests
    {
        [TestMethod]
        public void MakeSeatTaken_WhenCalled_SetsIsTakenToTrue()
        {
            var reservation = new ReservationModel();
            var seat = new SeatModel { ID = 10 };

            reservation.MakeSeatTaken();
            string result = reservation.SeatAvailble(seat, "Peter Parker");
            
            Assert.AreEqual("67 is taken", result);
        }

        [TestMethod]
        public void SeatAvailble_WhenSeatIsAlreadyTaken_ReturnsTakenMessage()
        {
            var reservation = new ReservationModel();
            var seat = new SeatModel { ID = 5 };
            reservation.MakeSeatTaken();

            string result = reservation.SeatAvailble(seat, "Anika");
            StringAssert.Contains(result, "is taken");
        }
    }

    public class SeatModel
    {
        public int ID { get; set; }
    }
}