using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;

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

        public bool AddShowing(Showing newShowing)
        {
            bool success = false;
            try
            {
                success = _dbShowing.CreateShowing(newShowing);
            }
            catch(InvalidOperationException)
            {
                success = false;
                //throw the exception istg
            }
            return success;
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

        public List<Showing> GetShowingsList()
        {
            List<Showing>? foundShowingsList = null;
            try
            {
                foundShowingsList = _dbShowing.GetAllShowings();
            }
            catch (InvalidOperationException)
            {
                //maybe nullreferenceexception
                foundShowingsList = new List<Showing>(); //return an empty list
            }
            return foundShowingsList;
        }

        public bool UpdateSpecificShowing(Showing showingToUpdate)
        {
            bool success = false;
            try
            {
                success = _dbShowing.UpdateShowing(showingToUpdate);
            }
            catch (InvalidOperationException)
            {
                success = false;
            }
            catch (SqlException)
            {
                success = false;
            }
            return success;
        }

        public bool DeleteShowingByShowingId(int showingIdToDelete)
        {
            bool success = false;
            try
            {
                success = _dbShowing.DeleteShowingByShowingId(showingIdToDelete);
            }
            catch (InvalidOperationException)
            {
                success = false;
            }
            catch (SqlException)
            {
                success = false;
            }
            return success;
        }
      
    }
}
