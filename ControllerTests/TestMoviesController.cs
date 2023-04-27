using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using SemesterProjekt3Api.Controllers;

namespace ControllerTests
{
    public class TestMoviesController
    {

        [Fact]
        public void TestGetIndexPage()
        {
            //Arrange
            var mc = new MoviesController();

            //Act
            var result = mc.Index();

            //Assert
            Assert.IsType<OkObjectResult>(result);

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetAllMovies(int movieId)
        {
            var mc = new MoviesController();

            var result = mc.Get(movieId); //maybe put in if-else statement to prevent negative value from crashing

            Assert.IsType<ObjectResult>(result); //could save assert value and run all through a final assert, to also check for negatives
        }
    }
}