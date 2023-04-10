using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class Genre
    {

        public Genre() : this("meh", "meh") 
        {
                 
        }
        public Genre(string tconst, string? genretype)
        {
            Tconst = tconst;
            Genretype = genretype;
        }

        public string Tconst { get; set; }
        public string? Genretype { get; set; }
    }
}
