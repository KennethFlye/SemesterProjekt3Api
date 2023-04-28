using Dapper;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbShowing
    {

        //private string _getShowingByShowingIdQuery = "SELECT showingId, startTime, isKidFriendly FROM Showing WHERE showingId = @insertedShowingId";
        //private string _getCopyIdAndShowRoomIdByShowingIdQuery = "SELECT movieCopyId, showRoomId FROM Showing WHERE showingId = @insertedShowingId";
        //Kigge på at kunne gøre de to nedenunder i en i stedet for to
        //private string _getMovieCopyByCopyIdQuery = "SELECT copyId, language, is3D, price FROM MovieCopy WHERE copyId = @copyId";
        //private string _getMovieInfoByCopyIdQuery = "SELECT infoId, title, length, genre, pgRating, premiereDate FROM MovieInfo, MovieCopy WHERE copyId = @copyId AND infoId = movieInfoId";
        //private string _getShowRoomByShowRoomIdQuery = "SELECT roomNumber, capacity FROM ShowRoom WHERE roomNumber = @roomNumber";
        private string _getSeatsByShowRoomId = "SELECT seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE showRoomId = @roomNumber";

        //private string _getMovieCopyAndMovieInfoByCopyIdQuery = "SELECT copyId, language, is3D, price, infoId, title, length, genre, pgRating, premiereDate FROM MovieCopy, MovieInfo WHERE copyId = @copyId AND infoId = movieInfoId";

        private string _getShowingByShowingIdQuery = "SELECT showingId, startTime, isKidFriendly, copyId, language, is3D, price, infoId, title, length, genre, pgRating, premiereDate, roomNumber, capacity FROM Showing, MovieInfo, MovieCopy, ShowRoom WHERE Showing.showingId = @insertedShowingId AND Showing.movieCopyId = MovieCopy.copyId AND MovieCopy.movieinfoId = MovieInfo.infoId AND Showing.showRoomId = ShowRoom.roomNumber";

/*        internal Showing GetShowingByShowingId(int showingId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            //1. Få fat i Showing
            Showing foundShowing = connection.Query<Showing>(_getShowingByShowingIdQuery, new { insertedShowingId = showingId }).Single();

            var result = connection.Query(_getCopyIdAndShowRoomIdByShowingIdQuery, new {insertedShowingId = showingId}).Single();

            int movieCopyId = result.movieCopyId;
            int showRoomId = result.showRoomId;

            //Test 2 i en
            
            var movieCopyResult = connection.Query<MovieCopy, MovieInfo, MovieCopy>(_getMovieCopyAndMovieInfoByCopyIdQuery, (movieCopy, movieInfo) =>
            {
                movieCopy.MovieType = movieInfo;
                return movieCopy;
            } , new { copyId = movieCopyId }, splitOn: "infoId");

            MovieCopy foundMovieCopy = movieCopyResult.First();

            
            //2. Få fat i MovieCopy
            //MovieCopy foundMovieCopy = connection.Query<MovieCopy>(_getMovieCopyByCopyIdQuery, new {copyId = movieCopyId }).Single();

            //3. Få fat i MovieInfo
            //MovieInfo foundMovieInfo = connection.Query<MovieInfo>(_getMovieInfoByCopyIdQuery, new {copyId = movieCopyId}).Single();
            
            //4. Tilføj MovieInfo Til MovieCopye og MovieCopy til Showing
            //foundMovieCopy.MovieType = foundMovieInfo;
            foundShowing.MovieCopy = foundMovieCopy;

            //5. Få fat i Showroom
            ShowRoom foundShowRoom = connection.Query<ShowRoom>(_getShowRoomByShowRoomIdQuery, new {roomNumber = showRoomId}).Single();

            //6. Få fat i Sæder til det Showroom
            List<Seat> foundSeats = connection.Query<Seat>(_getSeatsByShowRoomId, new {roomNumber = showRoomId}).ToList();

            //7. Tilføj Sæder til Showroom og Showroom til showing
            foundShowRoom.Seats = foundSeats;
            foundShowing.ShowRoom = foundShowRoom;

            //8. Returner Showing
            return foundShowing;
        }
    */
        internal Showing GetShowingByShowingId(int showingId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            var showingResult = connection.Query<Showing, MovieCopy, MovieInfo, ShowRoom, Showing>(_getShowingByShowingIdQuery, (showing, movieCopy, movieInfo, showRoom) =>
            {
                movieCopy.MovieType = movieInfo;
                showing.MovieCopy = movieCopy;
                showing.ShowRoom = showRoom;
                return showing;
            }, new { insertedShowingId = showingId }, splitOn: "copyId, infoId, roomNumber");

            Showing foundShowing = showingResult.First();

            foundShowing.ShowRoom.Seats = connection.Query<Seat>(_getSeatsByShowRoomId, new { roomNumber = foundShowing.ShowRoom.RoomNumber }).ToList();

            return foundShowing;

        }
    }
}
