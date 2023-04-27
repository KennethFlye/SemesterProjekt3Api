using SemesterProjekt3Api.Database;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseConnectionTest
{
    public class TestDbConnection
    {
        [Fact]
        public void ConnectionOpenTest()
        {
            //Arrange
            DBConnection dbcon = DBConnection.GetInstance();
            SqlConnection con = dbcon.GetConnection();

            //Act

            //Assert
            Assert.Equal(ConnectionState.Open, con.State);
        }

        [Fact]
        public void ConnectionHasAccesToDBTest()
        {
            //Arrange
            DBConnection dbcon = DBConnection.GetInstance();
            SqlConnection con = dbcon.GetConnection();

            //Act
            SqlCommand cmd = new SqlCommand("select seatNumber from seat where seatId = 10", con);
            int result = (int)cmd.ExecuteScalar();

            //Assert
            Assert.Equal(5, result);
        }
    }
}