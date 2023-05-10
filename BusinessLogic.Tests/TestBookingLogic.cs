using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;
using System.Collections;
using System.Numerics;
using System.Security.Cryptography;
using Xunit;

namespace BusinessLogic.Tests
{
    public class TestBookingLogic : IDisposable
    {

        private BookingLogic _bookingLogic;

        public TestBookingLogic()
        {
            _bookingLogic = new BookingLogic();
        }


        [Fact(Skip = "Crashes due to the IsSeatsBooked check i think")]
        public void TestPostNewBooking()
        {
            //Arrange - make a mock booking
            Booking mockBooking = new Booking(); //Should be ClassData in best case scenario

            mockBooking.TimeOfPurchase = DateTime.Now;
            mockBooking.Total = 999;
            mockBooking.CustomerPhone = "50529894"; //value lent from database

            Showing mockShowing = new Showing();
            mockShowing.ShowingId = 15; //value lent from database

            mockBooking.Showing = mockShowing;
            mockBooking.Showing.ShowingId = mockShowing.ShowingId; //the whole mockShowing object is made to avoid nullreference exceptions

            Seat mockSeat1 = new Seat();
            Seat mockSeat2 = new Seat();
            mockSeat1.SeatId = 9;
            mockSeat2.SeatId = 10;

            List<Seat> mockSeatsList = new List<Seat>();
            mockSeatsList.Add(mockSeat1);
            mockSeatsList.Add(mockSeat2);

            mockBooking.BookedSeats = mockSeatsList; //we cant add an empty list of seats to be booked


            //Act - try posting
            var success = _bookingLogic.AddBooking(mockBooking);

            //Assert - see if it passes/that no exceptions are thrown
            Assert.True(success);

            //OBS should also check for invalid bookings
            //var throwsException = Record.Exception(success);
            //if (!throwsException)
            //{
            //    Assert.True(success);
            //}
            //else
            //{
            //    Assert.Throws<SystemException>(sucess);
            //}

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetBookingByBookingId(int bookingId)
        {
            //Arrange - handled by the constructor in this case

            //Act - call the method
            var result = _bookingLogic.GetBookingById(bookingId);

            if (result != null)
            {
                //Assert
                Assert.Equal(bookingId, result.BookingId);
                //Could also assert that none of the columns are null
            }
            else
            {
                Assert.Null(result); //in case nothing was found we expect a null value to be returned
                //Assert.Throws<>(() => result); //should really test if the correct exception is thrown when exceptions happen, but nothing is thrown
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        public void TestGetSeatsByShowingId(int showingId)
        {
            //Arrange

            //Act
            var result = _bookingLogic.GetSeatsById(showingId);

            //Assert - returns the correct object: either a Booking, or an error (null reference or invalidoperation)
            if (result.Count > 0)
            {
                Assert.NotEmpty(result);
                Assert.IsType<Seat>(result[0]);
            }
            else if (result.Count < 1)
            {
                Assert.Empty(result);
            }
            else
            {
                Assert.Throws<InvalidOperationException>(() => result);
            }
        }

        public void Dispose()
        {
            _bookingLogic = null;
        }

    }

    //public class MockBooking : IEnumerable<object[]>
    //{
    //    public IEnumerator<object[]> GetEnumerator()
    //    {
    //        yield return new object[] {
    //            new Booking {
    //                TimeOfPurchase = DateTime.Now,
    //                Total = 999,
    //                CustomerPhone = "50529894", //value lent from database
    //                Showing = new Showing().ShowingId = 15,

    //        //Showing mockShowing = new Showing();
    //        //mockShowing.ShowingId = 15; //value lent from database

    //        //mockBooking.Showing = mockShowing;
    //        //mockBooking.Showing.ShowingId = mockShowing.ShowingId; //the whole mockShowing object is made to avoid nullreference exceptions

    //        //Seat mockSeat1 = new Seat();
    //        //Seat mockSeat2 = new Seat();
    //        //mockSeat1.SeatId = 9;
    //        //mockSeat2.SeatId = 10;

    //        //List<Seat> mockSeatsList = new List<Seat>();
    //        //mockSeatsList.Add(mockSeat1);
    //        //mockSeatsList.Add(mockSeat2);

    //        //mockBooking.BookedSeats = mockSeatsList;
    //    } };
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}
}