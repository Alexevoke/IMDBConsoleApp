using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class KnownForTitle
    {
        public KnownForTitle() : this("meh", "meh")
        {

        }

        public KnownForTitle(string nconst, string title)
        {
            Nconst = nconst;
            Title = title;
        }

        public string Nconst { get; set; }
        public string? Title { get; set; }
    }
}
