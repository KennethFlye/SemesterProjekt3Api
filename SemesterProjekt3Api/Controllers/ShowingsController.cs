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

        [HttpPost]
        [Route("")] //route api/showings
        public ActionResult PostNewShowing(Showing newShowing)
        {
            bool success = _showingLogic.AddShowing(newShowing);
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(); //internal server error
            }

        }

        [HttpGet]
        [Route("{showingId}")]
        public ActionResult GetShowingByShowingId(int showingId)
        {
            Showing foundShowing = _showingLogic.GetShowingByShowingId(showingId);
            if (foundShowing != null)
            {
                return Ok(foundShowing);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("")]
        public ActionResult GetAllShowings()
        {
            List<Showing> foundShowingsList = _showingLogic.GetShowingsList();
            if (foundShowingsList.Count > 0)
            {
                return Ok(foundShowingsList);
            }
            else if (foundShowingsList.Count < 1)
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{showingId}")]
        public ActionResult UpdateShowing(Showing showingToUpdate)
        {
            bool success = _showingLogic.UpdateSpecificShowing(showingToUpdate);
            if (success)
            {
                return Ok(showingToUpdate);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{showingId}")]
        public ActionResult DeleteShowing(int showingId)
        {
            bool success = _showingLogic.DeleteShowingByShowingId(showingId);
            if (success)
            {
                return NoContent(); //we dont receive anything when deleting
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("booked/{showingId}")]
        public ActionResult GetBookedSeats(int showingId)
        {
            List<Seat> bookedSeatsList = _showingLogic.GetBookedSeatsByShowingId(showingId);
            if (bookedSeatsList.Count > 0)
            {
                return Ok(bookedSeatsList);
            }
            else if (bookedSeatsList.Count < 1)
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
            return Ok(_showingLogic.IsSeatTaken(showingId, seatId));
        }


    }
}
