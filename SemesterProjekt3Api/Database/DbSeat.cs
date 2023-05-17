using Dapper;
using SemesterProjekt3Api.Model;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace SemesterProjekt3Api.Database
{
    public class DbSeat
    {
        private string _getSeatBySeatIdQuery = "Select seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE seatId = @seatId";

        private readonly string? _connectionString;

        public DbSeat()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("VestbjergBio");
        }

        internal Seat GetSeat(int seatId)
        {
            using IDbConnection dbCon = new SqlConnection(_connectionString);

            Seat foundSeat = dbCon.Query<Seat>(_getSeatBySeatIdQuery, new { seatId = seatId }).First();

            return foundSeat;
        }
    }
}
