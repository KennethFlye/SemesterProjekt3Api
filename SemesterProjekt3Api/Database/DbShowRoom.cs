using Dapper;
using SemesterProjekt3Api.Interfaces;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;
using System.Transactions;

namespace SemesterProjekt3Api.Database
{
    public class DbShowRoom : ICRUD<ShowRoom>
    {
        private string _getAllShowRooms = "select ShowRoom.roomNumber, ShowRoom.capacity FROM ShowRoom";

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

        public IEnumerable<ShowRoom> GetAll()
        {
            DBConnection dbCon = DBConnection.GetInstance();
            SqlConnection sqlCon = dbCon.GetConnection();

            List<ShowRoom> getAllShowRooms = sqlCon.Query<ShowRoom>(_getAllShowRooms).ToList();

            return getAllShowRooms;
        }

        public bool Update(ShowRoom entity)
        {
            throw new NotImplementedException();
        }
    }
}