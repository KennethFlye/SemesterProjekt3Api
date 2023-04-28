using Microsoft.AspNetCore.Mvc;
using SemesterProjekt3Api.Controllers;

namespace ControllerTests
{

    public class TestShowingsController : IDisposable
    {

        private ShowingsController _sCtrl;

        public TestShowingsController()
        {
            _sCtrl = new ShowingsController();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetShowingsById(int showingId)
        {
            //Arrange
            
            //Act
            var result = _sCtrl.Get(showingId);

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
        
        public void Dispose()
        {
            _sCtrl = null;
        }
    }
}