using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowingsController : ControllerBase
    {

        private DbShowing _dbShowing = new DbShowing();

        [HttpGet]
        [Route("{showingId}")]
        public ActionResult GetShowingByShowingId(int showingId)
        {

            Showing foundShowing = _dbShowing.GetShowingByShowingId(showingId);

            if(foundShowing == null)
            {
                return NotFound();
            }

            return Ok(foundShowing);
        }

        [HttpGet]
        [Route("booked/{showingId}")]
        public ActionResult GetBookedSeats(int showingId)
        {
           List<Seat> foundSeats = _dbShowing.GetBookedSeats(showingId);
            return Ok(foundSeats);
        }

        [HttpGet]
        [Route("seatTaken/{showingId}/{seatId}")]
        public ActionResult GetIfSeatBooked(int showingId, int seatId)
        {
            return Ok(_dbShowing.IsSeatTaken(showingId, seatId));
        }


    }
}
