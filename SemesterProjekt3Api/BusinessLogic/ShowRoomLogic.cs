using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.BusinessLogic
{
    public class ShowRoomLogic
    {
        private DbShowRoom _dbShowRoom = new DbShowRoom();

        public List<ShowRoom>? GetShowRoomsList()
        {
            List<ShowRoom>? foundList = null;
            
            try
            {
                foundList = (List<ShowRoom>?)_dbShowRoom.GetAll();
            }
            catch (InvalidOperationException)
            {
                foundList = new List<ShowRoom>();
            }

            return foundList;
        }
    }
}
