﻿using SemesterProjekt3Api.Database;
using SemesterProjekt3Api.Model;

namespace SemesterProjekt3Api.BusinessLogic
{
    public class SeatLogic
    {
        private DbSeat _dbSeat = new DbSeat();

        public Seat GetSeatBySeatId(int seatId)
        {
            Seat? foundSeat = null;
            try
            {
                foundSeat = _dbSeat.GetSeat(seatId);
            }
            catch (InvalidOperationException)
            {
                foundSeat = null;
            }
            return foundSeat;
        }
    }
}
