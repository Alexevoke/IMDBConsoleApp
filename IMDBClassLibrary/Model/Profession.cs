using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class Profession
    {
        
        public Profession() : this("meh", "meh") 
        {
            
        }
        
        public Profession(string nconst, string professionName)
        {
            Nconst = nconst;
            ProfessionName = professionName;
        }

        public string Nconst { get; set; }
        public string ProfessionName { get; set; }
    }
}
