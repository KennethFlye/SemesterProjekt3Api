using Dapper;
using Microsoft.Win32.SafeHandles;
using SemesterProjekt3Api.Model;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DbMovie
    {
        private string _getMovieInfoQuery = "SELECT infoId, title, length, genre, pgRating, premiereDate FROM MovieInfo";
        private string _getMovieCopyQuery = "SELECT copyId, language, is3D, price FROM MovieCopy";
        private string _getMovieInfoByCopyIdQuery = "SELECT movieinfoId FROM MovieCopy WHERE copyId = @copyId";

        private string _getMovieInfoByInfoIdQuery = "SELECT infoId, title, length, genre, pgRating, premiereDate FROM MovieInfo WHERE MovieInfo.infoId = @infoId";
        private string _getMovieCopiesByInfoIdQuery = "SELECT copyId, language, is3D, price FROM MovieCopy WHERE MovieCopy.movieInfoId = @infoId";
        private string _getShowRoomsQuery = "SELECT roomNumber, capacity FROM ShowRoom";
        private string _getSeatsByShowRoomId = "SELECT seatId, rowNumber, seatNumber, showRoomId FROM Seat WHERE showRoomId = @showRoomId";
        private string _getShowingsByMovieCopyIdsQuery = "SELECT showingId, startTime, isKidFriendly, showRoomId FROM Showing WHERE Showing.movieCopyId IN @Ids";
        private string _getCopyIdAndShowRoomIdByShowingIdQuery = "SELECT movieCopyId, showRoomId FROM Showing WHERE showingId = @showingId";

        internal List<MovieInfo> GetMovieInfos()
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            List<MovieInfo> foundInfos = connection.Query<MovieInfo>(_getMovieInfoQuery).ToList();

            return foundInfos;
        }

        internal List<MovieCopy> getMovies()
        {

            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            List<MovieInfo> foundInfos = connection.Query<MovieInfo>(_getMovieInfoQuery).ToList();

            if(foundInfos.Count == 0 )
            {
                //Exception?
            }

            

            List<MovieCopy> foundCopies = connection.Query<MovieCopy>(_getMovieCopyQuery).ToList();

            //https://www.learndapper.com/parameters
            foundCopies.ForEach(movieCopy =>
            {
                var parameters = new { copyId = movieCopy.copyId };
                int movieInfoId = connection.Query<int>(_getMovieInfoByCopyIdQuery, parameters).First();
                bool found = false;

                for (int i = 0; i < foundInfos.Count() && found == false; i++)
                {
                    if (movieInfoId == foundInfos[i].infoId)
                    {
                        movieCopy.MovieType = foundInfos[i];
                        found = true;
                    }
                }
            });

            return foundCopies;
        }

        internal List<Showing> getShowingsByMovieInfoId(int movieId)
        {
            DBConnection dbConnection = DBConnection.GetInstance();
            SqlConnection connection = dbConnection.GetConnection();

            //Gem infoId som parameter
            var parameterInfoId = new { infoId = movieId };

            //Få den specifikke MovieInfo
            MovieInfo foundInfo = connection.Query<MovieInfo>(_getMovieInfoByInfoIdQuery, parameterInfoId).First();
            //Lav liste med alle MovieCopies som er tilsluttet til denne MovieInfo
            List<MovieCopy> foundCopies = connection.Query<MovieCopy>(_getMovieCopiesByInfoIdQuery, parameterInfoId).ToList();

            //Tilføj MovieInfo instansen til alle MovieCopies
            foundCopies.ForEach(movieCopy =>
            {
                movieCopy.MovieType = foundInfo;
            });

            //Få fat i alle ShowRooms og gem dem i en liste
            List<ShowRoom> foundShowRooms = connection.Query<ShowRoom>(_getShowRoomsQuery).ToList();

            //Gem alle sæder til det specifikke ShowRoom og tilføj det til instansen
            foreach (ShowRoom element in foundShowRooms)
            {
                List<Seat> foundSeats = connection.Query<Seat>(_getSeatsByShowRoomId, new { showRoomId = element.RoomNumber }).ToList();
                element.Seats = foundSeats;
            }

            //Kan laves meget smartere. Men Laver en liste med alle CopyIds i de MovieCopies som blev fundet tidligere
            List<int> copyIds = new List<int>();
            foundCopies.ForEach(copy =>
            {
                copyIds.Add(copy.copyId);
            });
            //Konverterer listen til et IntArray
            var ids = copyIds.ToArray<int>();

            //Gem alle showings der har de CopyIDs som blev tilføjet til listen tidligere i en liste
            List<Showing> foundShowings = connection.Query<Showing>(_getShowingsByMovieCopyIdsQuery, new {Ids = ids}).ToList();

            //For hver showing gøres dette
            foundShowings.ForEach(showing =>
            {
                //Gemmer showingId'et for showing'en i en variabel
                var parameterShowingId = new { showingId = showing.showingId };
                //Gemmer movieCopyId'et og showRoomId'et vha. showingId'et
                var result = connection.Query(_getCopyIdAndShowRoomIdByShowingIdQuery, parameterShowingId).Single();

                int movieCopyId = result.movieCopyId;
                int showRoomId = result.showRoomId;

                bool copyFound = false;
                bool showRoomFound = false;

                for(int i = 0; i < foundCopies.Count && !copyFound; i++)
                {
                    //Find rigtig copy og tilføj til showing
                    if (foundCopies[i].copyId == movieCopyId)
                    {
                        showing.MovieCopy = foundCopies[i];
                        copyFound = true;
                    }
                }

                for(int i = 0; i < foundShowRooms.Count && !showRoomFound; i++)
                {
                    //Find rigtig showRoom og tilføj til showing
                    if (foundShowRooms[i].RoomNumber == showRoomId)
                    {
                        showing.ShowRoom = foundShowRooms[i];
                        showRoomFound = true;
                    }
                    
                }
            });
            //Returner de fuldendte showings
            return foundShowings;
        }
    }
}
