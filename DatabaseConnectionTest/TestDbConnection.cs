using SemesterProjekt3Api.Database;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseConnectionTest
{
    public class TestDbConnection : IDisposable
    {

        private DBConnection _dbcon;
        private SqlConnection _con;

        public TestDbConnection()
        {
            _dbcon = DBConnection.GetInstance();
            _con = _dbcon.GetConnection();
        }

        [Fact]
        public void ConnectionOpenTest()
        {
            //Arrange
            //Act

            //Assert
            Assert.Equal(ConnectionState.Open, _con.State);
        }

        [Fact]
        public void ConnectionHasAccesToDBTest()
        {
            //Arrange

            //Act
            SqlCommand cmd = new SqlCommand("select seatNumber from seat where seatId = 10", _con);
            int result = (int)cmd.ExecuteScalar();

            //Assert
            Assert.Equal(5, result);
        }

        public void Dispose()
        {
            _con = null;
            _dbcon = null;
        }
    }
}