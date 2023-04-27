using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SemesterProjekt3Api.Controllers;
using SemesterProjekt3Api.Model;

namespace ControllerTests
{
    
    public class TestBookingsController
    {

        [Fact]
        public void TestGetSeats()
        {
            //Arrange
            var bctrl = new BookingsController();

            //Act
            var result = bctrl.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result); //evt check for all returns

        }

        [Fact]
        //[ClassData(Booking b = new())] obs change param and fact/theory if implemented
        public void TestPostNewBooking()
        {
            //Arrange
            var newBooking = new Booking();
            var bctrl = new BookingsController();

            //Act
            var result = bctrl.Post(newBooking);

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
    }