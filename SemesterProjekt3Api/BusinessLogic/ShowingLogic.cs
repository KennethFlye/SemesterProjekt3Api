using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.BusinessLogic
{
    public class ShowingLogic
    {
        private DbShowing _dbShowing = new DbShowing();

        public bool IsSeatTaken(int showingId, int seatId)
        {
            bool isTaken = true; //default value set to true to make absolutely sure nothing is double booked
            try
            {
                isTaken = _dbShowing.IsSeatTaken(showingId, seatId);
            }
            catch (InvalidOperationException)
            {
                isTaken = true;
            }
            return isTaken;
        }

        public List<Seat> GetBookedSeatsByShowingId(int showingId)
        {
            List<Seat>? bookedSeatsList = null;
            try
            {
                bookedSeatsList = _dbShowing.GetBookedSeats(showingId);
            }
            catch (InvalidOperationException)
            {
                bookedSeatsList = new List<Seat>();
            }
            return bookedSeatsList;
        }

        public Showing GetShowingByShowingId(int showingId)
        {
            Showing? foundShowing = null;
            try
            {
                foundShowing = _dbShowing.GetShowingByShowingId(showingId);
            }
            catch (InvalidOperationException)
            {
                foundShowing = null;
            }
            return foundShowing;
        }

      
    }
}
