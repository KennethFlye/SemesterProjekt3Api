using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SemesterProjekt3Api.Controllers;

namespace ControllerTests
{
    public class TestTokensController
    {

        //takes all valid inputs
        [Theory]
        [InlineData("3SemClient", "D]&2D3k!VXpfch+f-39~.X*3JD|p_KzBe!HAb'#+^fnU8ocI}m/dTE7*BpCAdvGKP", "User")]
        public void TestGenerateTokenValid(string username, string password, string grantType)
        {
            //Arrange - set up the configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            //Act - instantiate the constructor and call the method for creating tokens
            var tc = new TokensController(config);

            var result = tc.Create(username, password, grantType);

            //Assert - if result is of type ObjectResult, thereby implying that IActionResult = 200 Ok

            Assert.IsType<ObjectResult>(result);
        }

        //takes invalid inputs/testdata in different order
        [Theory]
        [InlineData("3SemClient", "bobbytables1", "User")]
        public void TestGenerateTokenInvalid(string username, string password, string grantType)
        {
            //Arrange - set up the configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build(); //could be refactored to a method in itself

            var mockResult = new BadRequestResult();

            //Act - instantiate the constructor and call the method for creating tokens
            var tc = new TokensController(config);

            var result = tc.Create(username, password, grantType);

            //Assert - whether or not the results are of the same instance

            //Assert.Same(mockResult, result); //breaks although results are the same, may be a object location thing
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData("3SemClient", "D]&2D3k!VXpfch+f-39~.X*3JD|p_KzBe!HAb'#+^fnU8ocI}m/dTE7*BpCAdvGKP", "User")]
        public void TestTokenExpired(string username, string password, string grantType)
        {
            //Arrange - set up the configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            //var mockResult = new OkObjectResult();

            //Act - instantiate the constructor and call the method for creating tokens
            var tc = new TokensController(config);
            var result = tc.Create(username, password, grantType);

            //Thread t = Thread.CurrentThread;
            //t.

            //TODO make an await and test if expired?
        }
    }
}