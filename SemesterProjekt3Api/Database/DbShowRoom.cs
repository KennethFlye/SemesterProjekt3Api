using Dapper;
using SemesterProjekt3Api.Interfaces;
using SemesterProjekt3Api.Model;
using System.Data;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbShowRoom : ICRUD<ShowRoom>
    {
        //private string _getAllShowRooms = "SELECT roomNumber, capacity, seatId, rowNumber, seatNumber, showRoomId FROM ShowRoom, Seat WHERE roomNumber = showRoomId";
        private string _getAllShowRooms = "SELECT roomNumber, capacity from ShowRoom";

        private string _getShowRoomByIdQuery = "SELECT roomNumber, capacity FROM ShowRoom WHERE roomNumber = @roomNumber";
        private string _getSeatsByShowRoomId = "SELECT seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE showRoomId = @roomNumber";

        private readonly string? _connectionString;

        public DbShowRoom()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("VestbjergBio");
        }

        public bool Create(ShowRoom entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ShowRoom Get(int id)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            ShowRoom foundShowRoom = connection.Query<ShowRoom>(_getShowRoomByIdQuery, new { roomNumber = id }).First();

            List<Seat> foundSeats = connection.Query<Seat>(_getSeatsByShowRoomId, new { roomNumber = id }).ToList();

            foundShowRoom.Seats = foundSeats;

            return foundShowRoom;
        }

        public List<ShowRoom> GetAll()
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            List<ShowRoom> getAllShowRooms = connection.Query<ShowRoom>(_getAllShowRooms).ToList();

            for(int i = 0; i < getAllShowRooms.Count; i++)
            {
                List<Seat> foundSeats = connection.Query<Seat>(_getSeatsByShowRoomId, new { roomNumber = getAllShowRooms[i].RoomNumber }).ToList();
                getAllShowRooms[i].Seats = foundSeats;
            }

            return getAllShowRooms;
        }

        public bool Update(ShowRoom entity)
        {
            throw new NotImplementedException();
        }
    }
}