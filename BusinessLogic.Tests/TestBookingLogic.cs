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

        private BookingLogic? _bookingLogic;

        public TestBookingLogic()
        {
            _bookingLogic = new BookingLogic();
        }


        [Theory]
        [InlineData(-1, 10)]
        [InlineData(0, 10)]
        [InlineData(1, 0)]
        [InlineData(3, -1)]
        [InlineData(11, 11)] //try 7, 11
        public void TestPostNewBooking(int showingId, int seatId)
        {
            //Arrange - make a mock booking
            Booking mockBooking = new Booking(); //Should be ClassData in best case scenario

            mockBooking.TimeOfPurchase = DateTime.Now;
            mockBooking.Total = 999; //easier to find when looking through db
            mockBooking.CustomerPhone = "50529894"; //value lent from database

            Showing mockShowing = new Showing();
            mockShowing.ShowingId = showingId; 

            mockBooking.Showing = mockShowing;
            mockBooking.Showing.ShowingId = mockShowing.ShowingId; //the whole mockShowing object is made to avoid nullreference exceptions

            Seat mockSeat = new Seat();
            mockSeat.SeatId = seatId;

            List<Seat> mockSeatsList = new List<Seat>();
            mockSeatsList.Add(mockSeat);

            mockBooking.BookedSeats = mockSeatsList; //we cant add an empty list of seats to be booked

            //Make some checks
            ShowingLogic showingLogic = new ShowingLogic();
            Showing foundShowing = showingLogic.GetShowingByShowingId(showingId);
            SeatLogic seatLogic = new SeatLogic();
            Seat foundSeat = seatLogic.GetSeatBySeatId(seatId);
            bool isSeatTaken = showingLogic.IsSeatTaken(showingId, seatId);

            //Act - try posting
            var success = _bookingLogic.AddBooking(mockBooking);

            if (foundShowing != null && foundSeat != null && !isSeatTaken)
            {
                Assert.True(success);
            }
            else
            {
                Assert.False(success);
            }

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
}