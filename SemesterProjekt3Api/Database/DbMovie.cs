using Dapper;
using Microsoft.Win32.SafeHandles;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbMovie
    {
        private string _getMovieInfoQuery = "SELECT infoId, title, length, genre, pgRating, premiereDate FROM MovieInfo";
        private string _getMovieCopyQuery = "SELECT copyId, language, is3D, price, movieinfoId FROM MovieCopy";
        private string _getMovieCopiesQuery = "SELECT * FROM MovieCopy mc INNER JOIN MovieInfo mi ON mc.movieinfoId = mi.infoId";

        internal List<MovieCopy> getMovies()
        {
            List<MovieCopy> foundCopies = new List<MovieCopy>();
            List<MovieInfo> foundInfos = new List<MovieInfo>();

            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            foundInfos = connection.Query<MovieInfo>(_getMovieInfoQuery).ToList();

            foundCopies = connection.Query<MovieCopy>(_getMovieCopyQuery, (MovieCopy, foundInfos) =>
            {
                for(int i = 0, i < foundInfos.Count; i++)
                {

                }
            }).ToList();

            throw new NotImplementedException();
        }

        internal List<Showing> getShowingsByMovieId(int movieId)
        {
            throw new NotImplementedException();
        }
    }
}
