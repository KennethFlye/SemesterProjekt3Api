using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {

        private DbBooking _dbBooking = new DbBooking();

        [HttpGet]
        [Route("seats")]
        public ActionResult Get()
        {

            Request.Headers.TryGetValue("showingId", out var showingId);

            if(showingId == 0)
            {
                //Ved ikke om det er den her der bør bruges?
                return BadRequest();
            }
            else
            {
                List<Seat> seats = _dbBooking.GetSeatsByShowingId(showingId);

                if(seats.Count == 0)
                {
                    return NotFound();
                }
                return Ok(seats);
            }

        }

        [HttpPost]
        [Route("")]
        public ActionResult Post([FromBody] Booking newBooking)
        {
            // validate and save to database
            bool badThingsHappened = false;
            if (badThingsHappened)
            {
                return BadRequest();
            }
            else
            {
                /*
                //Stub
                Booking bookingTestJsonConvert = new Booking();
                bookingTestJsonConvert.BookingId = 3;
                bookingTestJsonConvert.Total = 240;
                bookingTestJsonConvert.TimeOfPurchase = DateTime.Now;
                bookingTestJsonConvert.CustomerPhone = "6412563";

                Seat seat1 = new Seat();
                seat1.SeatId = 1;
                seat1.RowNumber = 1;
                seat1.SeatNumber = 1;
                seat1.ShowroomId = 1;
                Seat seat2 = new Seat();
                seat2.SeatId = 2;
                seat2.RowNumber = 2;
                seat2.SeatNumber = 2;
                seat2.ShowroomId = 2;
                bookingTestJsonConvert.BookedSeats = new List<Seat>() {seat1,seat2};
                String stringLog = JsonSerializer.Serialize(bookingTestJsonConvert);

                return Created("Uuuh test " +stringLog,newBooking);
                //Stub

                */


                _dbBooking.AddBooking(newBooking);

                return Created("", newBooking);
            }

        }

    }
}
