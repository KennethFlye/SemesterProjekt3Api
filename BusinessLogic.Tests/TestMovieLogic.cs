using SemesterProjekt3Api.BusinessLogic;
using SemesterProjekt3Api.Model;
using System.Diagnostics.Eventing.Reader;

namespace BusinessLogic.Tests
{
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
        [InlineData(4)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(30)]
        public void TestGetSpecificMovieInfo(int movieInfoId)
        {
            //Arrange


            //Act
            bool isComplete = _movieLogic.GetMovieInfoById(movieInfoId, out MovieInfo? foundInfo);

            //Assert
            Assert.True(isComplete);
            Assert.NotNull(foundInfo);

            Assert.True(foundInfo.infoId == movieInfoId);
            Assert.False(foundInfo.Title.Equals(""));
            Assert.True(foundInfo.Length != 0);
            Assert.False(foundInfo.Genre.Equals(""));
            Assert.False(foundInfo.PgRating.Equals(""));
            Assert.True(foundInfo.PremiereDate != DateTime.MinValue);
            Assert.False(foundInfo.MovieUrl.Equals(""));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void TestGetSpecificMovieCopy(int movieCopyId)
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

        public void Dispose()
        {
            _movieLogic = null;
        }

    }
}