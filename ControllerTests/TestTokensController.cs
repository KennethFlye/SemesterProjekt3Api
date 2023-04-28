using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SemesterProjekt3Api.Controllers;

namespace ControllerTests
{
    public class TestTokensController : IDisposable
    {

        private TokensController _tCtrl;
        private IConfigurationRoot _config;

        public TestTokensController()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            _tCtrl = new TokensController(_config);
        }

        //takes all valid inputs
        [Theory]
        [InlineData("3SemClient", "D]&2D3k!VXpfch+f-39~.X*3JD|p_KzBe!HAb'#+^fnU8ocI}m/dTE7*BpCAdvGKP", "User")]
        public void TestGenerateTokenValid(string username, string password, string grantType)
        {
            //Arrange
            
            //Act
            var result = _tCtrl.Create(username, password, grantType);

            //Assert - if result is of type ObjectResult, thereby implying that IActionResult = 200 Ok

            Assert.IsType<ObjectResult>(result);
        }

        //takes invalid inputs/testdata in different order
        [Theory]
        [InlineData("3SemClient", "bobbytables1", "User")]
        public void TestGenerateTokenInvalid(string username, string password, string grantType)
        {
            //Arrange
            var mockResult = new BadRequestResult();

            //Act
            var result = _tCtrl.Create(username, password, grantType);

            //Assert

            //Assert.Same(mockResult, result); //breaks although results are the same, may be a object location thing
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData("3SemClient", "D]&2D3k!VXpfch+f-39~.X*3JD|p_KzBe!HAb'#+^fnU8ocI}m/dTE7*BpCAdvGKP", "User")]
        public void TestTokenExpired(string username, string password, string grantType)
        {
            //Arrange            
            //var mockResult = new OkObjectResult();

            //Act
            var result = _tCtrl.Create(username, password, grantType);

            //Thread t = Thread.CurrentThread;
            //t.

            //TODO make an await and test if expired?
        }

        public void Dispose()
        {
            _tCtrl = null;
            _config = null;
        }
    }
}