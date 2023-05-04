using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;

namespace BusinessLogic.Tests
{
    public class TestBookingLogic : IDisposable
    {

        private BookingLogic _bookingLogic;

        public TestBookingLogic()
        {
            _bookingLogic = new BookingLogic();
        }


        [Fact]
        public void TestPostNewBooking()
        {
            //Arrange - make a mock booking
            Booking mockBooking = new Booking();

            //Act - try posting
            var result = _bookingLogic.AddBooking(mockBooking);

            //Assert - see if it passes
            Assert.True(result);

            //OBS should also check for invalid bookings

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

            //Assert - returns either null or a booking
            if (result != null)
            {
                Assert.Equal(bookingId, result.BookingId);
            }
            else
            {
                Assert.Throws<InvalidOperationException>(() => result); //OBS implement - things may only be catched
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetSeatsByShowingId(int showingId)
        {
            //Arrange
            Seat mockSeat = new Seat();

            //Act
            var result = _bookingLogic.GetSeatsById(showingId);

            //Assert - returns the correct object: either a Booking, or an error (null reference or invalidoperation)
            if(result.Count > 0)
            {
                var firstHit = result.First(); //another act
                Assert.NotEmpty(result);
                //Assert.Equal(firstHit.GetType, mockSeat); //make a mockseat with a showroom number or something
            }
            else if(result.Count < 1)
            {
                Assert.Empty(result); //maybe too simple
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