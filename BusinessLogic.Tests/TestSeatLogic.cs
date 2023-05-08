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


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetSeatBySeatId(int seatId)
        {
            //Arrange

            //Act
            var result = _seatLogic.GetSeatBySeatId(seatId);

            //Assert
            Assert.Equal(seatId, result.SeatId);
        }
        
        public void Dispose()
        {
            _seatLogic = null;
        }

    }
}