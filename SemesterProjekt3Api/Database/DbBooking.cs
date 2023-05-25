using Dapper;
using SemesterProjekt3Api.Model;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace SemesterProjekt3Api.Database
{
    public class DbBooking
    {
        private string _selectBookingByIdQuery = "select * from Booking where bookingId = @bookingId";
        private string _getSeatTaken = "SELECT COUNT(Booking.bookingId) FROM BookingSeat, Booking WHERE BookingSeat.bookingId = Booking.bookingId and showingId = @sId and seatId = @seatId";

        private readonly string? _connectionString;

        public DbBooking()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("VestbjergBio");
        }

        public bool AddBooking(Booking newBooking)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.Serializable;
            using (var scopeTransaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using IDbConnection dbCon = new SqlConnection(_connectionString);

                for(int i = 0; i <newBooking.BookedSeats.Count; i++)
                {
                    if(dbCon.QuerySingle<int>(_getSeatTaken, new { sId = newBooking.Showing.ShowingId, seatId = newBooking.BookedSeats[i].SeatId }) != 0)
                    {
                        scopeTransaction.Dispose();
                        return false;
                    }
                }

                int idReturn = 0;
                string sql = "INSERT INTO [Booking] (timeOfPurchase, total, customerPhone, showingId) OUTPUT INSERTED.bookingId VALUES (@timeOfPurchase, @total, @phone, @sId)";
               
                idReturn = dbCon.QuerySingle<int>(
                sql,
                new
                {
                    timeOfPurchase = newBooking.TimeOfPurchase,
                    total = newBooking.Total,
                    phone = newBooking.CustomerPhone,
                    sId = newBooking.Showing.ShowingId
                });

                //Insert Seat
                foreach (Seat seat in newBooking.BookedSeats)
                {
                    dbCon.Query(
                        "INSERT INTO [BookingSeat] (bookingId, seatId) VALUES (@BookingId, @SeatId)",
                        new
                        {
                            BookingId = idReturn,
                            SeatId = seat.SeatId
                        });
                }
                scopeTransaction.Complete();
                
            }
            return true;
        }

        internal Booking GetBookingById(int _bookingId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            DbShowing _dbShowing = new();

            Booking booking = dbCon.QueryFirst<Booking>(_selectBookingByIdQuery, new { BookingId = _bookingId });
            int showingId = dbCon.QuerySingle<int>("select showingId from Booking where bookingId = @bookingId",new {bookingId = _bookingId});

            booking.Showing = _dbShowing.GetShowingByShowingId(showingId);

            string seatSql = "select * from Seat where seatId = ( select seatId from BookingSeat where bookingId = @id)";
            booking.BookedSeats = dbCon.Query<Seat>(seatSql, new {id = booking.BookingId}).ToList();

            return booking;
        }

        internal List<Seat> GetSeatsByShowingId(int showingId) //check if used, Gets all seats, also unbooked
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            List<Seat> seats = new List<Seat>();
            string sql = "select * from Seat where showRoomId = (select Showing.showRoomId from Showing where showingId = @id)";
            
            seats = dbCon.Query<Seat>(sql, new {id = showingId}).ToList();

            return seats;
        }
    }
}
