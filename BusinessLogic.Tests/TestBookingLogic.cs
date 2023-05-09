using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;
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
            ////Arrange - handled by the constructor in this case

            ////Act - call the method
            //NullReferenceException exp = (NullReferenceException)Record.Exception(() => _bookingLogic.GetBookingById(bookingId));
            //Assert.Equal("", exp.ToString());

            ////Assert - returns either null or a booking
            //if (exp == null)
            //{
            //    Assert.Equal(bookingId, _bookingLogic.GetBookingById(bookingId).BookingId);
            //}
            //else
            //{
            //    Assert.Throws<NullReferenceException>(() => _bookingLogic.GetBookingById(bookingId));
            //}
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