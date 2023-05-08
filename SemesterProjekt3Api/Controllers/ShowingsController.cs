using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowingsController : ControllerBase
    {

        private ShowingLogic _showingLogic = new ShowingLogic();

        [HttpGet]
        [Route("{showingId}")]
        public ActionResult GetShowingByShowingId(int showingId)
        {
            Showing foundShowing = _showingLogic.GetShowingByShowingId(showingId);
            if(foundShowing != null)
            {
                return Ok(foundShowing);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("booked/{showingId}")]
        public ActionResult GetBookedSeats(int showingId)
        {
            List<Seat> bookedSeatsList = _showingLogic.GetBookedSeatsByShowingId(showingId);
            if(bookedSeatsList.Count > 0)
            {
                return Ok(bookedSeatsList);
            }
            else if(bookedSeatsList.Count < 1)
            {
                return NotFound(showingId); //there are no booked seats for showingId
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("seatTaken/{showingId}/{seatId}")]
        public ActionResult GetIfSeatBooked(int showingId, int seatId)
        {
            //We are interested in both true and false values, so:
            return Ok(_showingLogic.isSeatTaken(showingId, seatId));
        }


    }
}
