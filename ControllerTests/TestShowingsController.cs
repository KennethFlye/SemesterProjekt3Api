using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.Controllers;

namespace ControllerTests
{

    public class TestShowingsController
    {

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetShowingsById(int showingId)
        {
            //Arrange
            var showingsController = new ShowingsController();

            //Act
            var result = showingsController.Get(showingId);

            //Assert
            if(result == null || showingId < 0 /*maybe redundent*/)
            {
                Assert.IsType<NotFoundResult>(result);
            }
            else
            {
                Assert.IsType<OkObjectResult>(result);
            }
        }
    }
}