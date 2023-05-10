using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.BusinessLogic
{
    public class BookingLogic
    {

        private DbBooking _dbBooking = new DbBooking();

        public bool AddBooking(Booking booking)
        {
            bool success = false;
            if (IsSeatsTaken(booking.Showing.ShowingId, booking.BookedSeats))
            {

                try
                {
                    _dbBooking.AddBooking(booking);
                    success = true; //always returns true now? make dbbooking return a bool
                }
                catch (InvalidOperationException)
                {
                    success = false;
                }
            }
            else
            {
                success = false; //seats were already booked
            }
            return success; //could also write method with 3 returns, one for succes, one for null and one for exception,
                            //and then let the controller return actionresults based on that
        }


        public Booking GetBookingById(int bookingId)
        {
            Booking? foundBooking = null;
            try
            {
                foundBooking = _dbBooking.GetBookingById(bookingId);
            }
            catch (InvalidOperationException) //or NullReferenceException or higher exception
            {
                foundBooking = null;
            }
            return foundBooking;

        }

        public List<Seat> GetSeatsById(int showingId)
        {
            List<Seat>? seatList = null;
            try
            {
                seatList = _dbBooking.GetSeatsByShowingId(showingId);
            }
            catch (InvalidOperationException)
            {
                seatList = new List<Seat>();
            }
            return seatList;

        }
        public bool IsSeatsTaken(int showingID, List<Seat> seats)
        {
            bool conflict = true;
            ShowingLogic showLogic = new();
            for (int i = 0; i < seats.Count() && conflict == true; i++)
            {
                if (showLogic.IsSeatTaken(showingID, seats[i].SeatId))
                {
                    conflict = false;
                }
            }
            return conflict;




        }
    }
}

