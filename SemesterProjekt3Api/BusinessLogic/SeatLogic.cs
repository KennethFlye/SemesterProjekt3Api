using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.BusinessLogic
{
    public class SeatLogic
    {
        private DbSeat _dbSeat = new DbSeat();

        internal Seat GetSeatBySeatId(int seatId)
        {
            Seat? foundSeat = null;
            try
            {
                foundSeat = _dbSeat.getSeat(seatId);
            }
            catch (InvalidOperationException)
            {
                foundSeat = null;
            }
            return foundSeat;
        }
    }
}
