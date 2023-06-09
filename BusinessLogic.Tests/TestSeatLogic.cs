﻿using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;

namespace BusinessLogic.Tests
{
    public class TestSeatLogic : IDisposable
    {

        private SeatLogic _seatLogic;

        public TestSeatLogic()
        {
            _seatLogic = new SeatLogic();
        }


        [Theory]//(Skip = "Invalid values run fine. Valid should be corrected (returns nullreferenceexception)/look at return values in method calls")]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(16)]
        public void TestGetSeatBySeatId(int seatId)
        {
            //Arrange

            //Act
            var result = _seatLogic.GetSeatBySeatId(seatId);

            if (seatId > 0)
            {
                //Assert
                Assert.Equal(seatId, result.SeatId);
            }
            else
            {
                Assert.Null(result);
            }
        }

        public void Dispose()
        {
            _seatLogic = null;
        }

    }
}