using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;
using System.Net.Http.Headers;

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
                _dbBooking.AddBooking(newBooking);

                return Created("", newBooking);
            }

        }

    }
}
