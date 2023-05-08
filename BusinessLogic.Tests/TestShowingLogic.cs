using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;

namespace BusinessLogic.Tests
{
    public class TestShowingLogic : IDisposable
    {

        private ShowingLogic _showingLogic;

        public TestShowingLogic()
        {
            _showingLogic = new ShowingLogic();
        }


        [Theory]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        public void TestIsSeatTaken(int showingId, int seatId)
        {
            //Arrange
            Seat mockSeat = new Seat();
            mockSeat.SeatNumber = seatId;
            
            //Act
            

            //Assert
            
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetBookedSeatsByShowingId(int showingId)
        {
            //Arrange

            //Act
            var result = _showingLogic.GetBookedSeatsByShowingId(showingId);

            if (result.Count > 0)
            {
                //Assert
                Assert.NotEmpty(result);
                Assert.IsType<Seat>(result[0]);
            }
            else if (result.Count < 1)
            {
                //Assert
                Assert.Empty(result);
            }
            else
            {
                //Assert
                Assert.Throws<InvalidOperationException>(() => result);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetShowingByShowingId(int showingId)
        {
            //Arrange

            //Act
            var result = _showingLogic.GetShowingByShowingId(showingId);

            if (result != null)
            {
                Assert.Equal(showingId, result.ShowingId);
            }
            else
            {
                Assert.Throws<InvalidOperationException>(() => result);
            }
        }

        public void Dispose()
        {
            _showingLogic = null;
        }

    }
}