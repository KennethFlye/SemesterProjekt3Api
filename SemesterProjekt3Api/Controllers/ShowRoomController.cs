using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.Controllers
{
    public class ShowRoomController : Controller
    {
        private ShowRoomLogic _showRoomLogic;

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<ShowRoom>? foundList = _showRoomLogic.GetShowRoomsList();

            if (foundList.Count > 0)
            {
                return Ok(foundList);
            }
            else if (foundList.Count < 1)
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
