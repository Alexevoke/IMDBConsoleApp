using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class Crew
    {

        public Crew() : this("meh", new List<string>(), new List<string>())
        { 
        }

        public Crew(string tconst, List<string> directors, List<string> writers)
        {
            Tconst = tconst;
            Directors = directors;
            Writers = writers;
        }

        public string Tconst { get; set; }
        public List<string> Directors { get; set; }
        public List<string> Writers { get; set; }
    }
}
