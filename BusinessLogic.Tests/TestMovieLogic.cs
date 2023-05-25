using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;
using System.Diagnostics.Eventing.Reader;

namespace BusinessLogic.Tests
{
    [Collection("MovieTest")]
    public class TestMovieLogic : IDisposable
    {

        private MovieLogic _movieLogic;

        public TestMovieLogic()
        {
            _movieLogic = new MovieLogic();
        }


        [Fact]
        public void TestGetMovieInfoList()
        {
            //Arrange
            var result = _movieLogic.GetMovieInfoList();

            //Act
            if (result.Count > 0)
            {
                //Assert
                Assert.NotEmpty(result);
                Assert.IsType<MovieInfo>(result[0]);
            }
            else if (result.Count < 1)
            {
                Assert.Empty(result);
            }
            else
            {
                Assert.Throws<InvalidOperationException>(() => result);
            }
        }

        [Fact]
        public void TestGetMovieCopyList()
        {
            //Arrange
            var result = _movieLogic.GetMovieCopyList();

            //Act
            if (result.Count > 0)
            {
                //Assert
                Assert.NotEmpty(result);
                Assert.IsType<MovieCopy>(result[0]);
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
        [InlineData(4)]
        public void TestGetShowingsListByMovieInfoId(int movieInfoId)
        {
            //Arrange

            //Act
            var result = _movieLogic.GetShowingsByMovieInfoId(movieInfoId);

            if (result.Count > 0)
            {
                //Assert
                Assert.NotEmpty(result);
                Assert.Equal(movieInfoId, result[0].MovieCopy.MovieType.infoId);
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(30)]
        public void PositiveTestGetSpecificMovieInfo(int movieInfoId)
        {
            //Arrange


            //Act
            bool isComplete = _movieLogic.GetMovieInfoById(movieInfoId, out MovieInfo? foundInfo);

            //Assert
            if (foundInfo != null && isComplete) //add if else statement to all other tests to check for invalid values
            {
                Assert.True(foundInfo.infoId == movieInfoId);
                Assert.False(foundInfo.Title.Equals(""));
                Assert.True(foundInfo.Length != 0);
                Assert.False(foundInfo.Genre.Equals(""));
                Assert.False(foundInfo.PgRating.Equals(""));
                Assert.True(foundInfo.PremiereDate != DateTime.MinValue);
                Assert.False(foundInfo.MovieUrl.Equals(""));
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void NegativeTestGetSpecificMovieInfo(int movieInfoId)
        {
            //Arrange

            //Act
            bool isComplete = _movieLogic.GetMovieInfoById(movieInfoId, out MovieInfo? foundInfo);

            //Assert
            Assert.False(isComplete);
            Assert.Null(foundInfo);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void PositiveTestGetSpecificMovieCopy(int movieCopyId)
        {
            //Arrange

            //Act
            bool isComplete = _movieLogic.GetMovieCopyById(movieCopyId, out MovieCopy? foundCopy);

            //Assert
            Assert.True(isComplete);
            Assert.NotNull(foundCopy);

            Assert.True(foundCopy.copyId == movieCopyId);
            Assert.False(foundCopy.Language.Equals(""));
            Assert.True(foundCopy.Price > 0);
            Assert.NotNull(foundCopy.MovieType);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void NegativeTestGetSpecificMovieCopy(int movieCopyId)
        {
            //Arrange

            //Act
            bool isComplete = _movieLogic.GetMovieCopyById(movieCopyId, out MovieCopy? foundCopy);

            //Assert
            Assert.False(isComplete);
            Assert.Null(foundCopy);

        }

        [Fact]
        public void PositiveTestInsertNewMovieInfo()
        {
            //Arrange
            MovieInfo testMovieInfo = new MovieInfo() 
            { 
                Title = "Test",
                Length = 123,
                Genre = "Sjov",
                PgRating = "18",
                PremiereDate = DateTime.Now,
                MovieUrl = "test.jpg",
                CurrentlyShowing = false
            };

            //Act
            int newTestInfoId = _movieLogic.AddMovieInfoToDatabase(testMovieInfo);

            _movieLogic.GetMovieInfoById(newTestInfoId, out MovieInfo? foundInfo);

            //Assert
            Assert.NotNull(foundInfo);
            Assert.True(testMovieInfo.Title.Equals(foundInfo.Title));
            Assert.True(testMovieInfo.Length == foundInfo.Length);
            Assert.True(testMovieInfo.Genre.Equals(foundInfo.Genre));
            Assert.True(testMovieInfo.PgRating.Equals(foundInfo.PgRating));
            Assert.True(testMovieInfo.PremiereDate.ToString().Equals(foundInfo.PremiereDate.ToString()));
            Assert.True(testMovieInfo.MovieUrl.Equals(foundInfo.MovieUrl));
            Assert.True(testMovieInfo.CurrentlyShowing.Equals(foundInfo.CurrentlyShowing));

            //Clean Up
            _movieLogic.DeleteMovieInfoById(newTestInfoId);

        }

        [Fact]
        public void PositiveTestInsertNewMovieCopy()
        {
            //Arrange
            _movieLogic.GetMovieInfoById(1, out MovieInfo? foundInfo);

            MovieCopy testMovieCopy = new MovieCopy()
            {
                Language = "en",
                Is3D = true,
                Price = 1,
                MovieType = foundInfo
            };

            //Act
            int newTestCopyId = _movieLogic.AddMovieCopyToDatabase(testMovieCopy);

            _movieLogic.GetMovieCopyById(newTestCopyId, out MovieCopy? foundMovieCopy);

            //Assert
            Assert.NotNull(foundMovieCopy);
            Assert.True(foundMovieCopy.Language.Equals(testMovieCopy.Language));
            Assert.True(foundMovieCopy.Is3D.Equals(testMovieCopy.Is3D));
            Assert.True(foundMovieCopy.Price == testMovieCopy.Price);
            Assert.True(foundMovieCopy.MovieType.infoId == testMovieCopy.MovieType.infoId);

            //Clean Up
            _movieLogic.DeleteMovieCopyById(newTestCopyId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void PositiveTestUpdateMovieInfo(int movieInfoId)
        {
            //Arrange
            _movieLogic.GetMovieInfoById(movieInfoId, out MovieInfo? foundMovieInfo);
            _movieLogic.GetMovieInfoById(movieInfoId, out MovieInfo? testMovieInfo);

            testMovieInfo.Title = "Test";
            testMovieInfo.Length = 0;
            testMovieInfo.Genre = "Test";
            testMovieInfo.PgRating = "Test";
            testMovieInfo.PremiereDate = testMovieInfo.PremiereDate.AddYears(1);
            testMovieInfo.MovieUrl = "Test";
            testMovieInfo.CurrentlyShowing = true;

            //Act
            _movieLogic.UpdateMovieInfoInDatabase(testMovieInfo);
            _movieLogic.GetMovieInfoById(movieInfoId, out MovieInfo? returnMovieInfo);

            //Assert
            Assert.NotNull(returnMovieInfo);
            Assert.True(testMovieInfo.Title.Equals(returnMovieInfo.Title));
            Assert.True(testMovieInfo.Length == returnMovieInfo.Length);
            Assert.True(testMovieInfo.Genre.Equals(returnMovieInfo.Genre));
            Assert.True(testMovieInfo.PgRating.Equals(returnMovieInfo.PgRating));
            Assert.True(testMovieInfo.PremiereDate.ToString().Equals(returnMovieInfo.PremiereDate.ToString()));
            Assert.True(testMovieInfo.MovieUrl.Equals(returnMovieInfo.MovieUrl));
            Assert.True(testMovieInfo.CurrentlyShowing.Equals(returnMovieInfo.CurrentlyShowing));

            //Clean Up
            _movieLogic.UpdateMovieInfoInDatabase(foundMovieInfo);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void PositiveTestUpdateMovieCopy(int movieCopyId)
        {
            //Arrange
            _movieLogic.GetMovieCopyById(movieCopyId, out MovieCopy? foundMovieCopy);
            _movieLogic.GetMovieCopyById(movieCopyId, out MovieCopy? testMovieCopy);

            testMovieCopy.Language = "test";
            testMovieCopy.Is3D = !testMovieCopy.Is3D;
            testMovieCopy.Price = 0;

            //Act
            _movieLogic.UpdateMovieCopyInDatabase(testMovieCopy);
            _movieLogic.GetMovieCopyById(movieCopyId, out MovieCopy? returnMovieCopy);

            //Assert
            Assert.NotNull(returnMovieCopy);
            Assert.True(testMovieCopy.Language.Equals(returnMovieCopy.Language));
            Assert.True(testMovieCopy.Is3D.Equals(returnMovieCopy.Is3D));
            Assert.True(testMovieCopy.Price ==  returnMovieCopy.Price);

            //Clean Up
            _movieLogic.UpdateMovieCopyInDatabase(foundMovieCopy);
        }

        public void Dispose()
        {
            _movieLogic = null;
        }

    }
}