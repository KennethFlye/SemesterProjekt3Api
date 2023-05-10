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

        public void Dispose()
        {
            _movieLogic = null;
        }

    }
}