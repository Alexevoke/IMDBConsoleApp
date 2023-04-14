using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public class Profession
    {
        
        public Profession() : this(0, 0) 
        {
            
        }
        
        public Profession(int id, Professions professionName)
        {
            Id = id;
            ProfessionName = professionName;
        }

        public int Id { get; set; }
        public Professions ProfessionName { get; set; }
    }
}
