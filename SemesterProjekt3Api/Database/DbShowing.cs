using Dapper;
using SemesterProjekt3Api.Model;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace SemesterProjekt3Api.Database
{
    public class DbShowing
    {
        private string _getSeatsByShowRoomId = "SELECT seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE showRoomId = @roomNumber";
        private string _getSeatTaken = "select * from BookingSeat, Booking where BookingSeat.bookingId = Booking.bookingId and showingId = @sId and seatId = @seatId";
        
        private string _getShowingByShowingIdQuery = "SELECT showingId, startTime, isKidFriendly, copyId, language, is3D, price, infoId, title, length, genre, pgRating, premiereDate, movieUrl, currentlyShowing, roomNumber, capacity FROM Showing, MovieInfo, MovieCopy, ShowRoom WHERE Showing.showingId = @insertedShowingId AND Showing.movieCopyId = MovieCopy.copyId AND MovieCopy.movieinfoId = MovieInfo.infoId AND Showing.showRoomId = ShowRoom.roomNumber";
        private string _getAllShowings = "SELECT showingId, startTime, isKidFriendly, copyId, language, is3D, price, infoId, title, length, genre, pgRating, premiereDate, roomNumber, capacity FROM Showing, MovieInfo, MovieCopy, ShowRoom WHERE Showing.movieCopyId = MovieCopy.copyId AND MovieCopy.movieinfoId = MovieInfo.infoId AND Showing.showRoomId = ShowRoom.roomNumber";

        private string _getSeatsByShowingId = "select BookingSeat.bookingId, seatId from BookingSeat, Booking where BookingSeat.bookingId = Booking.bookingId and Booking.showingId = @sId";
        private string _addNewShowing = "INSERT INTO [Showing] (startTime, isKidFriendly, showRoomId, movieCopyId) VALUES (@newStartTime, @newIsKidFriendly, @newShowRoomId, @newMovieCopyId)";
        private string _updateShowing = "UPDATE [Showing] SET startTime = @updatedStartTime, isKidFriendly = @updatedIsKidFriendly, showRoomId = @updatedShowRoomId, movieCopyId = @updatedMovieCopyId WHERE showingId = @showingIdToUpdate";
        private string _deleteShowingByShowingId = "DELETE FROM [Showing] WHERE showingId = @showingIdToDelete";

        private readonly string? _connectionString;

        public DbShowing()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("VestbjergBio");
        }

        internal bool IsSeatTaken(int showingId, int seatId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();
            return connection.QuerySingleOrDefault<bool>(_getSeatTaken, new { sId = showingId, seatId = seatId });
        }

        internal List<Seat> GetBookedSeats(int showingId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            List<Seat> seats = dbCon.Query<Seat>(_getSeatsByShowingId, new { sId = showingId }).ToList();

            return seats;
        }

        internal bool CreateShowing(Showing newShowing)
        {
            bool success = false;
            using(var scope = new TransactionScope())
            {
                using IDbConnection dbCon = new SqlConnection(_connectionString);

                int rowsAffected = dbCon.Execute(_addNewShowing, new
                {
                    newStartTime = newShowing.StartTime,
                    newIsKidFriendly = newShowing.IsKidFriendly,
                    newShowRoomId = newShowing.ShowRoom.RoomNumber,
                    newMovieCopyId = newShowing.MovieCopy.copyId
                });
                if(rowsAffected > 0)
                {
                    success = true;
                }
                scope.Complete();
            }
            return success;
        }

        internal Showing GetShowingByShowingId(int showingId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            var showingResult = dbCon.Query<Showing, MovieCopy, MovieInfo, ShowRoom, Showing>(_getShowingByShowingIdQuery, (showing, movieCopy, movieInfo, showRoom) =>
            {
                movieCopy.MovieType = movieInfo;
                showing.MovieCopy = movieCopy;
                showing.ShowRoom = showRoom;
                return showing;
            }, new { insertedShowingId = showingId }, splitOn: "copyId, infoId, roomNumber");

            Showing? foundShowing = showingResult.FirstOrDefault();
            if(foundShowing != null)
            {
                foundShowing.ShowRoom.Seats = dbCon.Query<Seat>(_getSeatsByShowRoomId, new { roomNumber = foundShowing.ShowRoom.RoomNumber }).ToList();
            }

            return foundShowing;
        }

        internal List<Showing> GetAllShowings()
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            List<Showing> foundAllShowings = dbCon.Query<Showing, MovieCopy, MovieInfo, ShowRoom, Showing>(_getAllShowings, (showing, movieCopy, movieInfo, showRoom) =>
            {
                movieCopy.MovieType = movieInfo;
                showing.MovieCopy = movieCopy;
                showing.ShowRoom = showRoom;
                return showing;
            }, splitOn: "copyId, infoId, roomNumber").ToList(); 

            for(int i = 0; i < foundAllShowings.Count; i++)
            {
                foundAllShowings[i].ShowRoom.Seats = dbCon.Query<Seat>(_getSeatsByShowRoomId, new
                {
                    roomNumber = foundAllShowings[i].ShowRoom.RoomNumber
                }).ToList();
            }

            return foundAllShowings;
        }

        internal bool UpdateShowing(Showing updatedShowing)
        {
            bool success = false;
            using(var scope = new TransactionScope())
            {
                using IDbConnection dbCon = new SqlConnection(_connectionString);

                int rowsAffected = dbCon.Execute(_updateShowing, new
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
                using IDbConnection dbCon = new SqlConnection(_connectionString);

                int rowsAffected = dbCon.Execute(_deleteShowingByShowingId, new { showingIdToDelete = showingId });
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
