using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class Person
    {
        public Person() : this("meh", "meh", 0, 0, new List<string>(), new List<string>())
        {
        }

        public Person(string nconst, string primaryName, int birthYear, int deathYear, 
            List<string> primaryProffesions, List<string> knownForTitles)
        {
            Nconst = nconst;
            PrimaryName = primaryName;
            BirthYear = birthYear;
            DeathYear = deathYear;
            PrimaryProffesions = primaryProffesions;
            KnownForTitles = knownForTitles;
        }

        public string Nconst { get; set; }
        public string PrimaryName { get; set; }
        public int BirthYear { get; set; }
        public int DeathYear { get; set; }
        public List<string> PrimaryProffesions { get; set; }
        public List<string> KnownForTitles { get; set; }
    }
}
