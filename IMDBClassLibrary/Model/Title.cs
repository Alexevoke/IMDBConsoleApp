using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public enum TitleType
    {
        movie, Short, tvEpisode, tvMiniSeries, tvMovie, tvPilot, tvSeries, tvShort, tvSpecial, video, videoGame
    }
    public enum GenreType
    {
        Action, Adult, Adventure, Animation, Biography, Comedy, Crime, Documentary, Drama, Family, Fantasy, 
        FilmNoir, GameShow, History, Horror, Music, Musical, Mystery, News, RealityTV, Romance, SciFi, Short, 
        Sport, TalkShow, Thriller, War, Western
    }
    public class Title
    {

        public Title() : this(0, 0 , "meh", "meh", false, 0, 0, 0, new List<GenreType>())
        {
            
        }
                   

        public Title(int id, TitleType titleType, string primaryTitle, string originalTitle,
            bool isAdult, int startYear, int endYear,int runtimeMinutes, List<GenreType> genres)
        {
            Id = id;
            TitleType = titleType;
            PrimaryTitle = primaryTitle;
            OriginalTitle = originalTitle;
            IsAdult = isAdult;
            StartYear = startYear;
            EndYear = endYear;
            RuntimeMinutes = runtimeMinutes;
            Genres = genres;
        }

        public int Id { get; set; }
        public TitleType TitleType { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? OriginalTitle { get; set;}
        public bool IsAdult { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? RuntimeMinutes { get; set; }
        public List<GenreType> Genres { get; set; }
    }
}
