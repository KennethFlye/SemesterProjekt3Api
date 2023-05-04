using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Controllers;

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
        //[InlineData()]
        public void TestGetSeats()
        {
            //Arrange - handled by the constructor in this case

            //Act - call the method


            //Assert - returns the correct object: either a Booking, or an error (null reference or invalidoperation)

        }

        [Fact]
        public void TestPostNewBooking()
        {
            //Arrange
            var newBooking = new Booking(); //only used once therefore not in constructor
                                            //could take different mock bookings to test both valid and invalid

            //Act
            var result = _bCtrl.Post(newBooking);

            //Assert
            Assert.IsType<CreatedResult>(result);
        }

        public void Dispose()
        {
            _bookingLogic = null;
        }

    }
}