﻿using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.BusinessLogic
{
    public class BookingLogic
    {

        private DbBooking _dbBooking = new DbBooking();

        public bool AddBooking(Booking booking)
        {
            bool success = false;

            try
            {
                success = _dbBooking.AddBooking(booking);
            }
            catch (InvalidOperationException)
            {
                success = false;
            }
            catch (SqlException)
            {
                success = false;
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

    }
}

