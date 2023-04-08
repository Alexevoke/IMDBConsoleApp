using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class Title
    {

        public Title() : this("meh", "meh", "meh", "meh", false, 0, 0, 0, new List<string>())
        {
            
        }
                   

        public Title(string tconst, string titleType, string primaryTitle, string originalTitle,
            bool isAdult, int startYear, int endYear,int runtimeMinutes, List<string> genres)
        {
            TitleType = titleType;
            PrimaryTitle = primaryTitle;
            OriginalTitle = originalTitle;
            IsAdult = isAdult;
            StartYear = startYear;
            EndYear = endYear;
            RuntimeMinutes = runtimeMinutes;
            Genres = genres;
        }

        public string Tconst { get; set; }
        public string? TitleType { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? OriginalTitle { get; set;}
        public bool IsAdult { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? RuntimeMinutes { get; set; }
        public List<string> Genres { get; set; }
    }
}
