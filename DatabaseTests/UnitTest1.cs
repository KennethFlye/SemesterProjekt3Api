using SemesterProjekt3Api.Database;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConnectionOpenTest()
        {
            DBConnection dbcon = DBConnection.GetInstance();
            SqlConnection con = dbcon.GetConnection();

            Assert.AreEqual(ConnectionState.Open, con.State);
        }

        [TestMethod]
        public void ConnectionHasAccesToDBTest()
        {
            DBConnection dbcon = DBConnection.GetInstance();
            SqlConnection con = dbcon.GetConnection();

            SqlCommand cmd = new SqlCommand("select seatNumber from seat where seatId = 10", con);

            int result = (int)cmd.ExecuteScalar();
            Assert.AreEqual(5, result);
        }
    }
}