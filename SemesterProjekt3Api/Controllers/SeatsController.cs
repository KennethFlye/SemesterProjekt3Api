using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : Controller
    {

        private SeatLogic _seatLogic = new SeatLogic();

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{seatId}")]
        public ActionResult Get(int seatId)
        {
            Seat foundSeat = _seatLogic.GetSeatBySeatId(seatId);
            if(foundSeat != null)
            {
                return Ok(foundSeat);
            }
            else
            {
                return NotFound(foundSeat);
            }
        }
    }
}
