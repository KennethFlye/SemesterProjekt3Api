using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.Database;

namespace SemesterProjekt3Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{tst}")] //Hurtig test
        public ActionResult<string> Get(string tst)
        {
            DBConnection dbc = DBConnection.GetInstance();
            dbc.OpenConnection();
            string returnString = dbc.GetConnection().State.ToString() + " " + dbc.GetConnection().Database.ToString();
            dbc.CloseConnection();

            return Ok($"recieved: {tst} {returnString}");
        }


    }



}
