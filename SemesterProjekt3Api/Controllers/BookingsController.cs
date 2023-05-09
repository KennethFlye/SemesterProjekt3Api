using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {

        private BookingLogic _bookingLogic = new BookingLogic();


        [HttpPost]
        [Route("")]
        public ActionResult Post([FromBody] Booking newBooking)
        {
           
            bool success = _bookingLogic.AddBooking(newBooking);
            if (success)
            {
                return Created("", newBooking); //explain URI string
            }
            else
            {
                return BadRequest(); //badrequests should probably be 500 internal server error
            }

        }

        [HttpGet]
        [Route("{bookingId}")]
        public ActionResult Get(int bookingId)
        {
            Booking foundBooking = _bookingLogic.GetBookingById(bookingId);
            
            
            if (foundBooking != null)
            {
                
                return Ok(foundBooking);
            }
            else
            {
                return NotFound(foundBooking); //or bookingId?
                                               //could also be BadRequest depending on whether or not it's an error or null value
            }
        }

        [HttpGet]
        [Route("allSeats/{showingId}")]
        public ActionResult GetSeats(int showingId)
        {
            List<Seat> seatList = _bookingLogic.GetSeatsById(showingId);
            if(seatList.Count > 0)
            {
                return Ok(seatList);
            }
            else if(seatList.Count < 1)
            {
                return NotFound(seatList); //or showingId?
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
