using Dapper;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace SemesterProjekt3Api.Database
{
    public class DbSeat
    {

        private string _getSeatBySeatIdQuery = "Select seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE seatId = @seatId";

        internal Seat getSeat(int seatId)
        {

            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            Seat foundSeat = connection.Query<Seat>(_getSeatBySeatIdQuery, new { seatId = seatId }).First();

            return foundSeat;
        }
    }
}
