using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.BusinessLogic
{
    public class MovieLogic
    {

        private DbMovie _dbMovie = new DbMovie();


        public List<MovieInfo> GetMovieInfoList()
        {
            List<MovieInfo>? infoList = null; //could make method generic to cover both info and copies
            try
            {
                infoList = _dbMovie.GetMovieInfos();
            }
            catch (InvalidOperationException)
            {
                infoList = new List<MovieInfo>(); //return an empty list
            }
            return infoList;
        }

        public List<MovieCopy> GetMovieCopyList()
        {
            List<MovieCopy>? movieCopyList = null;
            try
            {
                movieCopyList = _dbMovie.GetMovieCopies();
            }
            catch (InvalidOperationException)
            {
                movieCopyList = new List<MovieCopy>();
            }
            return movieCopyList;
        }

        public List<Showing> GetShowingsByMovieInfoId(int movieInfoId)
        {
            List<Showing>? foundShowingList = null;
            try
            {
                foundShowingList = _dbMovie.GetShowingsByMovieInfoId(movieInfoId);
            }
            catch (InvalidOperationException)
            {
                foundShowingList = new List<Showing>();
            }
            return foundShowingList;
        }

        public int AddMovieInfoToDatabase(MovieInfo newMovieInfo)
        {
            int newInfoId = 0;

            try
            {
                newInfoId = _dbMovie.AddMovieInfoToDatabase(newMovieInfo);
            }
            catch (InvalidOperationException)
            {
                newInfoId = 0;
            }

            return newInfoId;
        }

        public bool GetMovieInfoById(int infoId, out MovieInfo? foundInfo)
        {
            foundInfo = null;
            bool isComplete;

            try
            {
                foundInfo = _dbMovie.GetMovieInfoById(infoId);
                isComplete = foundInfo != null;
            }
            catch (InvalidOperationException)
            {
                isComplete = false;   
            }

            return isComplete;
        }

        public bool GetMovieCopyById(int copyId, out MovieCopy? foundCopy)
        {
            foundCopy = null;
            bool isComplete;

            try
            {
                foundCopy = _dbMovie.GetMovieCopyById(copyId);
                isComplete = foundCopy != null;
            }
            catch (InvalidOperationException)
            {
                isComplete = false;
            }

            return isComplete;
        }

        public int AddMovieCopyToDatabase(MovieCopy newMovieCopy)
        {
            int newCopyId = 0;

            try
            {
                newCopyId = _dbMovie.AddMovieCopyToDatabase(newMovieCopy);
            }
            catch (InvalidOperationException)
            {
                newCopyId = 0;
            }

            return newCopyId;
        }
    }
}
