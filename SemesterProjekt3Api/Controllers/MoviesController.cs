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

        [HttpGet]
        [Route("infos/{infoId}")]
        public ActionResult GetInfo(int infoId)
        {
            if(!_movieLogic.GetMovieInfoById(infoId, out MovieInfo? foundInfo))
            {
                return NotFound();
            }

            return Ok(foundInfo);
        }

        [HttpGet]
        [Route("copies/{copyId}")]
        public ActionResult GetCopy(int copyId)
        {
            if (!_movieLogic.GetMovieCopyById(copyId, out MovieCopy? foundCopy))
            {
                return NotFound();
            }

            return Ok(foundCopy);
        }



        [HttpPost]
        [Route("infos")]
        public ActionResult PostInfo([FromBody]MovieInfo newMovieInfo)
        {
            int newInfoId = _movieLogic.AddMovieInfoToDatabase(newMovieInfo);

            if (newInfoId != 0)
            {
                newMovieInfo.infoId = newInfoId;
                return CreatedAtAction(nameof(GetInfo), new { infoId = newInfoId }, newMovieInfo);

            }
            else { return BadRequest(); }
        }

        [HttpPost]
        [Route("copies")]
        public ActionResult PostCopy([FromBody]MovieCopy newMovieCopy)
        {
            int newCopyId = _movieLogic.AddMovieCopyToDatabase(newMovieCopy);

            if(newCopyId != 0)
            {
                newMovieCopy.copyId = newCopyId;
                return CreatedAtAction(nameof(GetCopy), new {copyId = newCopyId}, newMovieCopy);

            }
            else { return BadRequest(); }
        }
    }
}
