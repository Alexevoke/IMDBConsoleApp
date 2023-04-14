using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class KnownForTitle
    {
        public KnownForTitle() : this(0, "meh")
        {

        }

        public KnownForTitle(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }
        public string? Title { get; set; }
    }
}
