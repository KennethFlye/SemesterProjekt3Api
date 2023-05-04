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

        }

        public void Dispose()
        {
            _bookingLogic = null;
        }

    }
}