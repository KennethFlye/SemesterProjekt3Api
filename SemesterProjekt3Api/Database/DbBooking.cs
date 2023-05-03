using Dapper;
using Microsoft.Extensions.Primitives;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;
using System.Transactions;

namespace SemesterProjekt3Api.Database
{
    public class DbBooking
    {
        private string _selectBookingByIdQuery = "select * from Booking where bookingId = @bookingId";


        public void AddBooking(Booking newBooking)
        {
            using (var scopeTransaction = new TransactionScope())
            {
                Console.WriteLine("Before try");
                try
                {
                    Console.WriteLine("Start of try");
                    DBConnection dbc = DBConnection.GetInstance();
                    SqlConnection sqlConnection = dbc.GetConnection();

                    int idReturn = 0;
                    string sql = "INSERT INTO [Booking] (timeOfPurchase, total, customerPhone, showingId) OUTPUT INSERTED.bookingId VALUES (@timeOfPurchase, @total, @phone, @sId)";
                    Console.WriteLine("ShowingID:" + newBooking.Showing.showingId);
                    idReturn = sqlConnection.QuerySingle<int>(
                    sql,
                    new
                    {
                        timeOfPurchase = newBooking.TimeOfPurchase,
                        total = newBooking.Total,
                        phone = newBooking.CustomerPhone,
                        sId = newBooking.Showing.showingId
                    });
                    Console.WriteLine("Query single completed");
                    //Insert Seat
                    foreach (Seat seat in newBooking.BookedSeats)
                    {
                        sqlConnection.Query(
                            "INSERT INTO [BookingSeat] (bookingId, seatId) VALUES (@BookingId, @SeatId)",
                            new
                            {
                                BookingId = idReturn,
                                SeatId = seat.SeatId
                            });
                    }
                    Console.WriteLine("For-each completed");
                    scopeTransaction.Complete();

                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCEPTION HAPPENED!");
                    Console.WriteLine(e.Message);
                }
            }
        }

        internal Booking GetBookingById(int _bookingId)
        {
            DBConnection dbc = DBConnection.GetInstance();
            SqlConnection sqlConnection = dbc.GetConnection();

            DbShowing _dbShowing = new();

            Booking booking = sqlConnection.QueryFirst<Booking>(_selectBookingByIdQuery, new { BookingId = _bookingId });
            int showingId = sqlConnection.QuerySingle<int>("select showingId from Booking where bookingId = @bookingId",new {bookingId = _bookingId});

            booking.Showing = _dbShowing.GetShowingByShowingId(showingId);

            string seatSql = "select * from Seat where seatId = ( select seatId from BookingSeat where bookingId = @id)";
            booking.BookedSeats = sqlConnection.Query<Seat>(seatSql, new {id = booking.BookingId}).ToList();

            return booking;
        }

        internal List<Seat> GetSeatsByShowingId(int showingId) //check if used, Gets all seats, also unbooked
        {
            List<Seat> seats = new List<Seat>();
            DBConnection dbc = DBConnection.GetInstance();
            SqlConnection sqlConnection = dbc.GetConnection();
            string sql = "select * from Seat where showRoomId = (select Showing.showRoomId from Showing where showingId = @id)";
            seats = sqlConnection.Query<Seat>(sql, new {id = showingId}).ToList();
            return seats;
        }
    }
}
