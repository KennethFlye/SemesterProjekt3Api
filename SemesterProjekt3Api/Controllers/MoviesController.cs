using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private MovieLogic _movieLogic = new MovieLogic();

        [HttpGet]
        public ActionResult Index()
        {
            List<MovieCopy> movieCopiesList = _movieLogic.GetMovieCopyList();
            
            //Could also be made into a method for itself, takinga generic list as param, to keep code short and DRY
            if(movieCopiesList.Count > 0)
            {
                return Ok(movieCopiesList);
            }
            else if(movieCopiesList.Count < 1)
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("infos")]
        public ActionResult GetInfos()
        {
            List<MovieInfo> movieInfoList = _movieLogic.GetMovieInfoList();
            if(movieInfoList.Count > 0)
            {
                return Ok(movieInfoList);
            }
            else if(movieInfoList.Count < 1)
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("{movieId}/showings")]
        public ActionResult Get(int movieId)
        {
            List<Showing> showingsList = _movieLogic.GetShowingsByMovieInfoId(movieId);
            if(showingsList.Count > 0)
            {
                return Ok(showingsList);
            }
            else if(showingsList.Count < 1)
            {
                return NotFound(showingsList); //or movieinfoID
            }
            else
            {
                return BadRequest(); //maybe also take param to show exception?
            }
        }
    }
}
