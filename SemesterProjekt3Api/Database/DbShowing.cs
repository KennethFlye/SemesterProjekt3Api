using Dapper;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbShowing
    {
        private string _getSeatsByShowRoomId = "SELECT seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE showRoomId = @roomNumber";
        private string _getSeatTaken = "select * from BookingSeat, Booking where BookingSeat.bookingId = Booking.bookingId and showingId = @sId and seatId = @seatId";
        private string _getShowingByShowingIdQuery = "SELECT showingId, startTime, isKidFriendly, copyId, language, is3D, price, infoId, title, length, genre, pgRating, premiereDate, roomNumber, capacity FROM Showing, MovieInfo, MovieCopy, ShowRoom WHERE Showing.showingId = @insertedShowingId AND Showing.movieCopyId = MovieCopy.copyId AND MovieCopy.movieinfoId = MovieInfo.infoId AND Showing.showRoomId = ShowRoom.roomNumber";
        private string _getSeatsByShowingId = "select BookingSeat.bookingId, seatId from BookingSeat, Booking where BookingSeat.bookingId = Booking.bookingId and Booking.showingId = @sId";

        internal bool IsSeatTaken(int showingId, int seatId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();
            return connection.QuerySingleOrDefault<bool>(_getSeatTaken, new { sId = showingId, seatId = seatId });
        }

        public List<Seat> GetBookedSeats(int showingId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();
            List<Seat> seats = connection.Query<Seat>(_getSeatsByShowingId, new { sId = showingId }).ToList();

            return seats;
        }

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
