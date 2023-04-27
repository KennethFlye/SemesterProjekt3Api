using Dapper;
using Microsoft.Win32.SafeHandles;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbMovie
    {
        private string _getMovieInfoQuery = "SELECT infoId, title, length, genre, pgRating, premiereDate FROM MovieInfo";
        private string _getMovieCopyQuery = "SELECT copyId, language, is3D, price FROM MovieCopy";
        private string _getMovieInfoByCopyId = "SELECT movieinfoId FROM MovieCopy WHERE copyId = @copyId";

        internal List<MovieCopy> getMovies()
        {
            List<MovieCopy> foundCopies = new List<MovieCopy>();
            List<MovieInfo> foundInfos = new List<MovieInfo>();

            foundInfos.Count();

            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            foundInfos = connection.Query<MovieInfo>(_getMovieInfoQuery).ToList();
            foundCopies = connection.Query<MovieCopy>(_getMovieCopyQuery).ToList();

            //https://www.learndapper.com/parameters
            foundCopies.ForEach(movieCopy =>
            {
                var parameters = new { copyId = movieCopy.copyId };
                int movieInfoId = connection.Query<int>(_getMovieInfoByCopyId, parameters).First();
                bool found = false;

                for (int i = 0; i < foundInfos.Count() && found == false; i++)
                {
                    if (movieInfoId == foundInfos[i].infoId)
                    {
                        movieCopy.MovieType = foundInfos[i];
                        found = true;
                    }
                }
            });

            return foundCopies;
        }

        internal List<Showing> getShowingsByMovieId(int movieId)
        {
            throw new NotImplementedException();
        }
    }
}
