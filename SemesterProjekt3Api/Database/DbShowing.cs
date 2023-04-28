using Dapper;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbShowing
    {

        private string _getShowingByShowingIdQuery = "SELECT showingId, startTime, isKidFriendly FROM Showing WHERE showingId = @insertedShowingId";
        private string _getCopyIdAndShowRoomIdByShowingIdQuery = "SELECT movieCopyId, showRoomId FROM Showing WHERE showingId = @insertedShowingId";
        //Kigge på at kunne gøre de to nedenunder i en i stedet for to
        private string _getMovieCopyByCopyIdQuery = "SELECT copyId, language, is3D, price FROM MovieCopy WHERE copyId = @copyId";
        private string _getMovieInfoByCopyIdQuery = "SELECT infoId, title, length, genre, pgRating, premiereDate FROM MovieInfo, MovieCopy WHERE copyId = @copyId AND infoId = movieInfoId";
        private string _getShowRoomByShowRoomIdQuery = "SELECT roomNumber, capacity FROM ShowRoom WHERE roomNumber = @roomNumber";
        private string _getSeatsByShowRoomId = "SELECT seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE showRoomId = @roomNumber";
        internal Showing GetShowingByShowingId(int showingId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            //1. Få fat i Showing
            Showing foundShowing = connection.Query<Showing>(_getShowingByShowingIdQuery, new { insertedShowingId = showingId }).Single();

            var result = connection.Query(_getCopyIdAndShowRoomIdByShowingIdQuery, new {insertedShowingId = showingId}).Single();

            int movieCopyId = result.movieCopyId;
            int showRoomId = result.showRoomId;

            //2. Få fat i MovieCopy
            MovieCopy foundMovieCopy = connection.Query<MovieCopy>(_getMovieCopyByCopyIdQuery, new {copyId = movieCopyId }).Single();

            //3. Få fat i MovieInfo
            MovieInfo foundMovieInfo = connection.Query<MovieInfo>(_getMovieInfoByCopyIdQuery, new {copyId = movieCopyId}).Single();

            //4. Tilføj MovieInfo Til MovieCopye og MovieCopy til Showing
            foundMovieCopy.MovieType = foundMovieInfo;
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
    }
}
