using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.BusinessLogic
{
    public class MovieLogic
    {

        private DbMovie _dbMovie = new DbMovie();


        internal List<MovieInfo> GetMovieInfoList()
        {
            List<MovieInfo>? infoList = null; //could make method generic to cover both info and copies
            try
            {
                infoList = _dbMovie.GetMovieInfos();
            }
            catch (InvalidOperationException)
            {
                infoList = null;
            }
            return infoList;
        }

        internal List<MovieCopy> GetMovieCopyList()
        {
            List<MovieCopy>? movieCopyList = null;
            try
            {
                movieCopyList = _dbMovie.GetMovieCopies();
            }
            catch (InvalidOperationException)
            {
                movieCopyList = null;
            }
            return movieCopyList;
        }

        internal List<Showing> GetShowingsByMovieInfoId(int movieInfoId)
        {
            List<Showing>? foundShowingList = null;
            try
            {
                foundShowingList = _dbMovie.GetShowingsByMovieInfoId(movieInfoId);
            }
            catch (InvalidOperationException)
            {
                foundShowingList = null;
            }
            return foundShowingList;
        }
    }
}
