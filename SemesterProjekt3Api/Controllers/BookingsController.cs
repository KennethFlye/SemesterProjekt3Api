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
        [Route("allSeats/{showingId}")]
        public ActionResult GetSeats(int showingId)
        {
            return Ok(_dbBooking.GetSeatsByShowingId(showingId));   
        }

        [HttpGet]
        [Route("{bookingId}")]
        public ActionResult Get(int bookingId)
        {
            //booking foundbooking = _bookinglogic.get....(id)
            //if foundbooking!= null
            //return ok(foundbooking)
            //else return other error(bookingid?)
            
            Booking returnBooking = _dbBooking.GetBookingById(bookingId);
            
            return Ok(returnBooking);
        }

        [HttpPost]
        [Route("")]
        public ActionResult Post([FromBody] Booking newBooking)
        {
            Console.WriteLine("Post method called");
            // validate and save to database
            bool badThingsHappened = false;
            if (badThingsHappened)
            {
                return BadRequest();
            }
            else
            {
                
                _dbBooking.AddBooking(newBooking);

                return Created("", newBooking);
            }

        }

    }
}
