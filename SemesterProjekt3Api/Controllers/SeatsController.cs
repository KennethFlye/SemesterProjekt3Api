using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : Controller
    {

        private DbSeat _dbSeat = new DbSeat();

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{seatId}")]
        public ActionResult Get(int seatId)
        {
            Seat seat = _dbSeat.getSeat(seatId);

            if (seat == null)
            {
                return NotFound();
            }

            return Ok(seat);
        }
    }
}
