﻿namespace SemesterProjekt3Api.Model
{
    public class MovieInfo
    {

        public int infoId { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }
        public string Genre { get; set; }
        public string PgRating { get; set; }
        public DateTime PremiereDate { get; set; }
        public string MovieUrl { get; set; }
        public bool CurrentlyShowing { get; set; }
        

    }
}
