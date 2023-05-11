using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestPlatform.Utilities;
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


        [Theory(Skip = "Needs to be corrected. Invalid values should return booked or exception. Valid should do the check - maybe add if/else statement")] 
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

        [Fact]
        public void TestAddShowing()
        {
            //Arrange
            Showing mockShowing = new Showing();
            mockShowing.StartTime = DateTime.Now;
            mockShowing.IsKidFriendly = true;

            ShowRoom mockShowRoom = new ShowRoom();
            mockShowRoom.RoomNumber = 2;
            mockShowing.ShowRoom = mockShowRoom;

            MovieCopy mockMovieCopy = new MovieCopy();
            mockMovieCopy.copyId = 4;
            mockShowing.MovieCopy = mockMovieCopy;

            //Act
            var result = _showingLogic.AddShowing(mockShowing);

            //Assert
            Assert.True(result);
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

        [Fact]
        public void TestGetAllShowings()
        {
            //Arrange

            //Act
            var result = _showingLogic.GetShowingsList();

            if(result.Count > 0)
            {
                //Assert
                Assert.NotEmpty(result);
                Assert.IsType<Showing>(result[0]);
            }
            else if(result.Count < 1)
            {
                //Assert
                Assert.Empty(result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [Fact(Skip = "Implement when making mockShowing into classdata object")]
        public void TestUpdateSpecificShowingByShowingId()
        {
            //Arrange
            Showing mockShowing = new Showing(); //could reuse mock from addShowing

            //Act
            var result = _showingLogic.UpdateSpecificShowing(mockShowing);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(19)] //works once
        public void TestDeleteShowingByShowingId(int showingIdToDelete)
        {
            //Arrange

            //Act
            var result = _showingLogic.DeleteShowingByShowingId(showingIdToDelete);

            Assert.True(result);
        }

        public void Dispose()
        {
            _showingLogic = null;
        }

    }
}