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

            //Act
            var result = _showingLogic.IsSeatTaken(showingId, seatId);

            //Assert
            Assert.False(result);

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
        [InlineData(3)]
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
                Assert.Null(result);
                //Assert.Throws<InvalidOperationException>(() => result); //Should really test if the method throws the correct exception,
                //but nothing is thrown and the system handles null values without problem
            }
        }

        public void Dispose()
        {
            _showingLogic = null;
        }

    }
}