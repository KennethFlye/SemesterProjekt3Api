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
                foundList = _dbShowRoom.GetAll();
            }
            catch (InvalidOperationException)
            {
                foundList = new List<ShowRoom>();
            }

            return foundList;
        }

        public ShowRoom getSpecificShowRoom(int showRoomId)
        {
            ShowRoom foundShowRoom = null;

            try
            {
                foundShowRoom = _dbShowRoom.Get(showRoomId);
            }
            catch (InvalidOperationException)
            {
                foundShowRoom = new ShowRoom();
            }

            return foundShowRoom;
        }
    }
}
