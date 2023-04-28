using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SemesterProjekt3Api.Controllers;
using SemesterProjekt3Api.Model;

namespace ControllerTests
{

    public class TestBookingsController : IDisposable
    {

        private BookingsController _bCtrl;

        public TestBookingsController()
        {
            _bCtrl = new BookingsController();
        }

        [Fact]
        public void TestGetSeats()
        {
            //Arrange

            //Act
            var result = _bCtrl.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result); //evt check for all returns

        }

        [Fact]
        //[ClassData(Booking b = new())] obs change param and fact/theory if implemented
        public void TestPostNewBooking()
        {
            //Arrange
            var newBooking = new Booking(); //only used once therefore not in constructor

            //Act
            var result = _bCtrl.Post(newBooking);

            //Assert
            Assert.IsType<CreatedResult>(result);
        }

        //internal Booking getTestBooking(int x)
        //{
        //    Booking[] testBookings = new Booking[5]
        //    {
        //        new Booking(), new Booking(), new Booking(), new Booking(), new Booking()
        //    };
        //    return testBookings[x];
        //}
        ////OR USE https://hamidmosalla.com/2017/02/25/xunit-theory-working-with-inlinedata-memberdata-classdata/

        public void Dispose()
        {
            _bCtrl = null;
        }

    }
}