using Dapper;
using SemesterProjekt3Api.Model;
using System.Data;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbMovie
    {
        private string _getMovieInfoQuery = "SELECT infoId, title, length, genre, pgRating, premiereDate, movieUrl, currentlyShowing FROM MovieInfo";
        private string _getMovieCopyQuery = "SELECT copyId, language, is3D, price FROM MovieCopy";
        private string _getMovieInfoByCopyIdQuery = "SELECT movieinfoId FROM MovieCopy WHERE copyId = @copyId";

        private string _getMovieCopiesQuery = "SELECT copyId, language, is3D, price, infoId, title, length, genre, pgRating, premiereDate, movieUrl, currentlyShowing FROM MovieCopy, MovieInfo WHERE MovieCopy.movieInfoId = MovieInfo.infoId";

        private string _getMovieInfoByInfoIdQuery = "SELECT infoId, title, length, genre, pgRating, premiereDate, movieUrl, currentlyShowing FROM MovieInfo WHERE MovieInfo.infoId = @infoId";
        private string _getSeatsByShowRoomId = "SELECT seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE showRoomId = @showRoomId";

        private string _getShowingsByInfoId = "SELECT * FROM Showing, ShowRoom, MovieCopy, MovieInfo WHERE Showing.showRoomId = ShowRoom.roomNumber AND Showing.movieCopyId = MovieCopy.copyId AND MovieCopy.movieinfoId = MovieInfo.infoId AND MovieInfo.infoId = @infoId";

        private string _getMovieCopyByCopyIdQuery = "SELECT copyId, language, is3D, price, infoId, title, length, genre, pgRating, premiereDate, movieUrl, currentlyShowing FROM MovieCopy, MovieInfo WHERE copyId = @copyId AND MovieCopy.movieinfoId = MovieInfo.infoId";

        private string _insertMovieInfoQuery = @"INSERT INTO MovieInfo (title, length, genre, pgRating, premiereDate, movieUrl, currentlyShowing) VALUES (@Title, @Length, @Genre, @PgRating, @PremiereDate, @MovieUrl, @CurrentlyShowing) SELECT SCOPE_IDENTITY()";
        private string _insertMovieCopyQuery = @"INSERT INTO MovieCopy (language, is3D, price, movieinfoId) VALUES (@Language, @Is3D, @Price, @movieinfoId) SELECT SCOPE_IDENTITY()";

        private string _updateMovieInfoQuery = "UPDATE MovieInfo SET title = @Title, length = @Length, genre = @Genre, pgRating = @PgRating, premiereDate = @PremiereDate, movieUrl = @MovieUrl, currentlyShowing = @CurrentlyShowing WHERE infoId = @InfoId";
        private string _updateMovieCopyQuery = "UPDATE MovieCopy SET language = @Language, is3D = @Is3D, price = @Price, movieInfoId = @MovieInfoId WHERE copyId = @CopyId";

        private string _deleteMovieInfoByIdQuery = "DELETE FROM MovieInfo WHERE infoId = @infoId";
        private string _deleteMovieCopyByIdQuery = "DELETE FROM MovieCopy WHERE copyId = @copyId";

        private readonly string? _connectionString;

        public DbMovie()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("VestbjergBio");
        }

        public List<MovieInfo> GetMovieInfos()
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            List<MovieInfo> foundInfos = dbCon.Query<MovieInfo>(_getMovieInfoQuery).ToList();

            return foundInfos;
        }

        public List<MovieCopy> GetMovieCopies()
        {

            using IDbConnection dbCon = new SqlConnection(_connectionString);

            List<MovieCopy> foundCopies = dbCon.Query<MovieCopy, MovieInfo, MovieCopy>(_getMovieCopiesQuery, (movieCopy, movieInfo) =>
            {
                movieCopy.MovieType = movieInfo;
                return movieCopy;
            }, splitOn:"infoId").ToList();
            
            return foundCopies;
        }

        public List<Showing> GetShowingsByMovieInfoId(int movieId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            List<Showing> foundShowings = dbCon.Query<Showing, ShowRoom, MovieCopy, MovieInfo, Showing>(_getShowingsByInfoId, (showing, showRoom, movieCopy, movieInfo) =>
            {
                movieCopy.MovieType = movieInfo;
                showing.MovieCopy = movieCopy;

                using IDbConnection dbCon2 = new SqlConnection(_connectionString);
                showRoom.Seats = dbCon2.Query<Seat>(_getSeatsByShowRoomId, new { showRoomId = showRoom.RoomNumber }).ToList();

                showing.ShowRoom = showRoom;

                return showing;
            }, new { infoId = movieId }, splitOn: "roomNumber, copyId, infoId").ToList();

            return foundShowings;
        }

        public MovieInfo? GetMovieInfoById(int infoId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            return dbCon.Query<MovieInfo>(_getMovieInfoByInfoIdQuery, new {infoId = infoId}).First();
        }

        public MovieCopy? GetMovieCopyById(int copyId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            var movieResult = dbCon.Query<MovieCopy, MovieInfo, MovieCopy>(_getMovieCopyByCopyIdQuery, (movieCopy, movieInfo) =>
            {
                movieCopy.MovieType = movieInfo;
                return movieCopy;
            }, new { copyId = copyId }, splitOn: "infoId");

            MovieCopy foundCopy = movieResult.FirstOrDefault();

            return foundCopy;
        }

        public int AddMovieInfoToDatabase(MovieInfo newMovieInfo)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            int newInfoId = 0;

            newInfoId = dbCon.ExecuteScalar<int>(_insertMovieInfoQuery, newMovieInfo);

            return newInfoId;
        }

        public int AddMovieCopyToDatabase(MovieCopy newMovieCopy)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            int newCopyId = 0;

            newCopyId = dbCon.QuerySingle<int>(_insertMovieCopyQuery, new
            {
                Language = newMovieCopy.Language,
                Is3D = newMovieCopy.Is3D,
                Price = newMovieCopy.Price,
                movieinfoId = newMovieCopy.MovieType.infoId
            });

            return newCopyId;
        }

        public bool UpdateMovieInfoInDatabase(MovieInfo updatedMovieInfo)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            int rowsChanged = dbCon.Execute(_updateMovieInfoQuery, updatedMovieInfo);

            return rowsChanged > 0;
        }

        public bool UpdateMovieCopyInDatabase(MovieCopy updatedMovieCopy)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            int rowsChanged = dbCon.Execute(_updateMovieCopyQuery, new
            {
                Language = updatedMovieCopy.Language,
                Is3D = updatedMovieCopy.Is3D,
                Price = updatedMovieCopy.Price,
                MovieInfoId = updatedMovieCopy.MovieType.infoId,
                CopyId = updatedMovieCopy.copyId,
            });

            return rowsChanged > 0;
        }

        public bool DeleteMovieInfoById(int movieInfoId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            int rowsChanged = dbCon.Execute(_deleteMovieInfoByIdQuery, new { infoId = movieInfoId });

            return rowsChanged > 0;
        }

        public bool DeleteMovieCopyById(int movieCopyId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            int rowsChanged = dbCon.Execute(_deleteMovieCopyByIdQuery, new { copyId = movieCopyId });

            return rowsChanged > 0;
        }
    }
}