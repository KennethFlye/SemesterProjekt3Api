using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using SemesterProjekt3Api.Controllers;

namespace ControllerTests
{
    public class TestMoviesController : IDisposable
    {

        private MoviesController _mCtrl;

        public TestMoviesController()
        {
            _mCtrl = new MoviesController();
        }

        [Fact]
        public void TestGetIndexPage()
        {
            //Arrange

            //Act
            var result = _mCtrl.Index();

            //Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetAllMovies(int movieId)
        {
            var result = _mCtrl.Get(movieId); //maybe put in if-else statement to prevent negative value from crashing

            Assert.IsType<ObjectResult>(result); //could save assert value and run all through a final assert, to also check for negatives
        }

        public void Dispose()
        {
            _mCtrl = null;
        }
    }
}