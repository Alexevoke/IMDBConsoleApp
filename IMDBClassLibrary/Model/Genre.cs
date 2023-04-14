using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class Genre
    {

        public Genre() : this(0, 0) 
        {
                 
        }
        public Genre(int id, GenreType genretype)
        {
            Id = id;
            Genretype = genretype;
        }

        public int Id { get; set; }
        public GenreType Genretype { get; set; }
    }
}
