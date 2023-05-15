using Dapper;
using SemesterProjekt3Api.Interfaces;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;
using System.Transactions;

namespace SemesterProjekt3Api.Database
{
    public class DbShowRoom : ICRUD<ShowRoom>
    {
        private string _getAllShowRooms = "SELECT roomNumber, capacity, seatId, rowNumber, seatNumber, showRoomId FROM ShowRoom, Seat WHERE roomNumber = showRoomId";

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
            throw new NotImplementedException();
        }

        public List<ShowRoom> GetAll()
        {
            DBConnection dbCon = DBConnection.GetInstance();
            SqlConnection sqlCon = dbCon.GetConnection();

            List<ShowRoom> getAllShowRooms = sqlCon.Query<ShowRoom>(_getAllShowRooms).ToList();

            //List <ShowRoom> getAllShowRooms = sqlCon.Query<ShowRoom, Seat, ShowRoom>(_getAllShowRooms, (showRoom, seat) =>
            //{
            //    showRoom.Seats = new List<Seat>();
            //    return showRoom;
            //}, splitOn: "capacity").ToList();

            //for(int i = 0; i < getAllShowRooms.Count; i++)
            //{
            //    //getAllShowRooms[i].Seats.Add(...)
            //}

            return getAllShowRooms;
        }

        public bool Update(ShowRoom entity)
        {
            throw new NotImplementedException();
        }
    }
}