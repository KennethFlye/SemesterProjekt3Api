using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowRoomsController : ControllerBase
    {
        private ShowRoomLogic _showRoomLogic = new ShowRoomLogic();

        [HttpGet]
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

        [HttpGet]
        [Route("{showRoomId}")]
        public IActionResult GetSpecificShowRoom(int showRoomId)
        {
            ShowRoom foundShowRoom = _showRoomLogic.getSpecificShowRoom(showRoomId);

            if(foundShowRoom != null)
            {
                return Ok(foundShowRoom);
            }
            else
            {
                return BadRequest();
            }
        }
        
    }
}
