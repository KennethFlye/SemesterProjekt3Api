using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private DbMovie _dbMovie = new DbMovie();

        [HttpGet]
        public ActionResult Index()
        {

            List<MovieCopy> movieCopies = _dbMovie.getMovies();

            if(movieCopies.Count == 0)
            {
                return NotFound();
            }

            return Ok(movieCopies);

        }

        [HttpGet]
        [Route("{movieId}/showings")]
        public ActionResult Get(int movieId)
        {

            List<Showing> showings = _dbMovie.getShowingsByMovieId(movieId);

            if(showings.Count == 0)
            {
                return NotFound();
            }

            return Ok(showings);

        }

    }
}
