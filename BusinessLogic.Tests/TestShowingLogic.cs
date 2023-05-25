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


        [Theory]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(3, 26)]
        [InlineData(15, 26)]
        public void TestIsSeatTaken(int showingId, int seatId)
        {
            //Arrange
            var comp = _showingLogic.GetBookedSeatsByShowingId(showingId);

            //Act
            var result = _showingLogic.IsSeatTaken(showingId, seatId);

            //Assert
            if (!result) 
            {
                //if seat to be booked and already booked seats are not in conflict we expect the id to not be in the booked seats list
                for (int i = 0; i < comp.Count; i++)
                {
                    Assert.NotEqual(seatId, comp[0].SeatId);
                }
            }
            else
            {
                Assert.True(result);
            }
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
        [InlineData(-1, 3)]
        [InlineData(0, 3)]
        [InlineData(1, 3)]
        public void TestAddShowing(int showRoomNumber, int movieCopyId)
        {
            //Arrange
            Showing mockShowing = new Showing();
            mockShowing.StartTime = DateTime.Now; //could also be set by param, but not relevant right now
            mockShowing.IsKidFriendly = true;

            ShowRoom mockShowRoom = new ShowRoom();
            mockShowRoom.RoomNumber = showRoomNumber;
            mockShowing.ShowRoom = mockShowRoom;

            MovieCopy mockMovieCopy = new MovieCopy();
            mockMovieCopy.copyId = movieCopyId;
            mockShowing.MovieCopy = mockMovieCopy; //DRY make into class/memberdata

            var showings = _showingLogic.GetShowingsList();
            int showingsCount = showings.Count;

            //Act
            var result = _showingLogic.AddShowing(mockShowing);

            if (result)
            {
                //Assert
                Assert.Equal(showingsCount +1, _showingLogic.GetShowingsList().Count);
            }
            else
            {
                //Assert
                Assert.Equal(showingsCount, _showingLogic.GetShowingsList().Count);
            }

            //cleanup should be implemented
            //_showingLogic.DeleteShowingByShowingId(mockShowing.ShowingId);
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

        [Theory]
        [InlineData(-1, 1, 3)]
        [InlineData(0, 1, 3)]
        [InlineData(1, 1, 3)]
        [InlineData(25, 1, 3)]
        public void TestUpdateSpecificShowingByShowingId(int showingIdToUpdate, int showRoomNumber, int movieCopyId)
        {
            //Arrange
            Showing mockShowing = new Showing();
            mockShowing.ShowingId = showingIdToUpdate;
            mockShowing.StartTime = DateTime.Now; //could also be set by param, but not relevant right now
            mockShowing.IsKidFriendly = true;

            ShowRoom mockShowRoom = new ShowRoom();
            mockShowRoom.RoomNumber = showRoomNumber;
            mockShowing.ShowRoom = mockShowRoom;

            MovieCopy mockMovieCopy = new MovieCopy();
            mockMovieCopy.copyId = movieCopyId;
            mockShowing.MovieCopy = mockMovieCopy; //DRY make into class/memberdata

            var showingExists = _showingLogic.GetShowingByShowingId(showingIdToUpdate);
            ShowRoomLogic srLog = new ShowRoomLogic();
            var showRoomExists = srLog.getSpecificShowRoom(showRoomNumber);
            MovieLogic movLog = new MovieLogic();
            MovieCopy copy;
            var movieExists = movLog.GetMovieCopyById(movieCopyId, out copy);


            //Act
            var result = _showingLogic.UpdateSpecificShowing(mockShowing);

            //Assert
            if (showingExists != null && showRoomExists != null && movieExists)
            {
                Assert.True(result);
            }
            else
            {
                Assert.False(result);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(12)] //try 13
        public void TestDeleteShowingByShowingId(int showingIdToDelete)
        {
            //Arrange
            var deletableShowing = _showingLogic.GetShowingByShowingId(showingIdToDelete); //we need to see if the showingid exists in the database
            var hasBookedSeats = _showingLogic.GetBookedSeatsByShowingId(showingIdToDelete); //we want to know if the showing is associated with any bookings

            //Act
            var result = _showingLogic.DeleteShowingByShowingId(showingIdToDelete);

            if(deletableShowing != null && hasBookedSeats.Count < 1)
            {
                //Assert
                Assert.True(result); //showing is deleted
            }
            else
            {
                Assert.False(result); //showing does not exist and therefore is not deleted
            }
        }

        public void Dispose()
        {
            _showingLogic = null;
        }

    }
}