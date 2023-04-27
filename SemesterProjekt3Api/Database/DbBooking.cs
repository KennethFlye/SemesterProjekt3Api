using Dapper;
using Microsoft.Extensions.Primitives;
using SemesterProjekt3Api.Model;
using System.Data;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbBooking
    {
        public void AddBooking(Booking newBooking)
        {
            DBConnection dbc = DBConnection.GetInstance();
            SqlConnection sqlConnection = dbc.GetConnection();

            int idReturn = 0;
            try
            {
                string sql = "INSERT INTO [Booking] (timeOfPurchase, total, customerPhone, showingId) OUTPUT INSERTED.bookingId VALUES (@timeOfPurchase, @total, @phone, @showingId)";
                idReturn = sqlConnection.QuerySingle<int>(
                sql,
                new
                {
                    timeOfPurchase = newBooking.TimeOfPurchase,
                    total = newBooking.Total,
                    phone = newBooking.CustomerPhone,
                    showingId = newBooking.Showing.showingId
                });
            }
            catch (Exception)
            {

            }
            //Todo, seat stuff
            
            Console.WriteLine(idReturn);


            //sqlConnection.Open();
            /*try
            {
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "INSERT INTO Booking (timeOfPurchase, total, customerPhone, showingId) VALUES (@purchaseTime, @total @phone @showingId)";
                SqlParameter time = new SqlParameter("@timeOfPurchase", SqlDbType.DateTime);
                SqlParameter total = new SqlParameter("@total",SqlDbType.Float);
                SqlParameter phone = new SqlParameter("@customerPhone",SqlDbType.Text, 50);
                SqlParameter showingId = new SqlParameter("@showingId",SqlDbType.Int);

                time.Value = newBooking.TimeOfPurchase;
                total.Value = newBooking.Total;
                phone.Value = newBooking.CustomerPhone;
                showingId.Value = newBooking.Showing.Id;
                
                command.Parameters.Add(time);
                command.Parameters.Add(total);
                command.Parameters.Add(phone);
                command.Parameters.Add(showingId);

                //Execute
                command.Prepare();
                command.ExecuteNonQuery();



            }
            catch (Exception)
            {
                throw;
            }*/
        }

        internal List<Seat> GetSeatsByShowingId(StringValues showingId)
        {
            throw new NotImplementedException();
        }
    }
}
