using Dapper;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;
using System.Transactions;

namespace SemesterProjekt3Api.Database
{
    public class DbShowing
    {
        private string _getSeatsByShowRoomId = "SELECT seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE showRoomId = @roomNumber";
        private string _getSeatTaken = "select * from BookingSeat, Booking where BookingSeat.bookingId = Booking.bookingId and showingId = @sId and seatId = @seatId";
        private string _getShowingByShowingIdQuery = "SELECT showingId, startTime, isKidFriendly, copyId, language, is3D, price, infoId, title, length, genre, pgRating, premiereDate, roomNumber, capacity FROM Showing, MovieInfo, MovieCopy, ShowRoom WHERE Showing.showingId = @insertedShowingId AND Showing.movieCopyId = MovieCopy.copyId AND MovieCopy.movieinfoId = MovieInfo.infoId AND Showing.showRoomId = ShowRoom.roomNumber";
        private string _getAllShowings = "SELECT showingId, startTime, isKidFriendly, showRoomId, movieCopyId FROM Showing";
        private string _getSeatsByShowingId = "select BookingSeat.bookingId, seatId from BookingSeat, Booking where BookingSeat.bookingId = Booking.bookingId and Booking.showingId = @sId";
        private string _addNewShowing = "INSERT INTO [Showing] (startTime, isKidFriendly, showRoomId, movieCopyId) VALUES (@newStartTime, @newIsKidFriendly, @newShowRoomId, @newMovieCopyId)";
        private string _updateShowing = "UPDATE [Showing] SET startTime = @updatedStartTime, isKidFriendly = @updatedIsKidFriendly, showRoomId = @updatedShowRoomId, movieCopyId = @updatedMovieCopyId WHERE showingId = @showingIdToUpdate";
        private string _deleteShowingByShowingId = "DELETE FROM [Showing] WHERE showingId = @showingIdToDelete";

        internal bool IsSeatTaken(int showingId, int seatId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();
            return connection.QuerySingleOrDefault<bool>(_getSeatTaken, new { sId = showingId, seatId = seatId });
        }

        internal List<Seat> GetBookedSeats(int showingId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();
            List<Seat> seats = connection.Query<Seat>(_getSeatsByShowingId, new { sId = showingId }).ToList();

            return seats;
        }

        internal bool CreateShowing(Showing newShowing)
        {
            bool success = false;
            using(var scope = new TransactionScope())
            {
                DBConnection dbConnection = DBConnection.GetInstance();
                SqlConnection sqlConnection = dbConnection.GetConnection();

                success = sqlConnection.QuerySingle<bool>(_addNewShowing, new
                {
                    newStartTime = newShowing.StartTime,
                    newIsKidFriendly = newShowing.IsKidFriendly,
                    newShowRoomId = newShowing.ShowRoom.RoomNumber,
                    newMovieCopyId = newShowing.MovieCopy.copyId
                });

                scope.Complete();
            }
            return success;
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

            Showing? foundShowing = showingResult.FirstOrDefault();
            if(foundShowing != null)
            {
                foundShowing.ShowRoom.Seats = connection.Query<Seat>(_getSeatsByShowRoomId, new { roomNumber = foundShowing.ShowRoom.RoomNumber }).ToList();
            }

            return foundShowing;
        }

        internal List<Showing> GetAllShowings()
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection sqlConnection = dbConnection.GetConnection();

            List<Showing> foundShowingsList = sqlConnection.Query<Showing>(_getAllShowings).ToList();
            return foundShowingsList;
        }

        internal bool UpdateShowing(Showing updatedShowing)
        {
            bool success = false;
            using(var scope = new TransactionScope())
            {
                DBConnection dbConnection = DBConnection.GetInstance();
                SqlConnection sqlConnection = dbConnection.GetConnection();

                int rowsAffected = sqlConnection.Execute(_updateShowing, new
                {
                    updatedStartTime = updatedShowing.StartTime,
                    updatedIsKidFriendly = updatedShowing.IsKidFriendly,
                    updatedShowRoomId = updatedShowing.ShowRoom.RoomNumber,
                    updatedMovieCopyId = updatedShowing.MovieCopy.copyId,

                    showingIdToUpdate = updatedShowing.ShowingId
                });
                
                if(rowsAffected > 0)
                {
                    success = true;
                }
                scope.Complete();
            }
            return success;
        }

        internal bool DeleteShowingByShowingId(int showingId)
        {
            bool success = false;
            using(var scope = new TransactionScope())
            {
                DBConnection dbConnection = DBConnection.GetInstance();
                SqlConnection sqlConnection = dbConnection.GetConnection();

                int rowsAffected = sqlConnection.Execute(_deleteShowingByShowingId, new { showingIdToDelete = showingId });
                if(rowsAffected > 0)
                {
                    success = true;
                }
                scope.Complete();
            }
            return success;
        }
    }
}
