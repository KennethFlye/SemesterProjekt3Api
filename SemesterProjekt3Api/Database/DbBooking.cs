using Dapper;
using Microsoft.Extensions.Primitives;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;
using System.Transactions;

namespace SemesterProjekt3Api.Database
{
    public class DbBooking
    {
        public void AddBooking(Booking newBooking)
        {
            using (var scopeTransaction = new TransactionScope())
            {
                try
                {
                    DBConnection dbc = DBConnection.GetInstance();
                    SqlConnection sqlConnection = dbc.GetConnection();

                    int idReturn = 0;
                    string sql = "INSERT INTO [Booking] (timeOfPurchase, total, customerPhone, showingId) OUTPUT INSERTED.bookingId VALUES (@timeOfPurchase, @total, @phone, @sId)";
                    idReturn = sqlConnection.QuerySingle<int>(
                    sql,
                    new
                    {
                        timeOfPurchase = newBooking.TimeOfPurchase,
                        total = newBooking.Total,
                        phone = newBooking.CustomerPhone,
                        sId = newBooking.Showing.showingId
                    });

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
                    scopeTransaction.Complete();

                }
                catch (Exception)
                {

                }

            }
        }

        internal List<Seat> GetSeatsByShowingId(StringValues showingId)
        {
            throw new NotImplementedException();
        }
    }
}
