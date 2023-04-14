using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBClassLibrary.Model
{
    public enum Professions
    {
        actor, actress, animation_department, art_department, art_director, assistant, assistant_director, 
        camera_department, casting_department, casting_director, choreographer, cinematographer, composer, 
        costume_department, costume_designer, director, editor, editorial_department, electrical_department,
        executive, legal, location_management, make_up_department, manager, miscellaneous, music_artist,
        music_department, podcaster, producer, production_department, production_designer, production_manager,
        publicist, script_department, set_decorator, sound_department, soundtrack, special_effects, stunts,
        talent_agent, transportation_department, visual_effects, writer
    }
    public class Person
    {
        public Person() : this(0, "meh", 0, 0, new List<Professions>(), new List<string>())
        {
        }

        public Person(int id, string primaryName, int birthYear, int deathYear, 
            List<Professions> primaryProfessions, List<string> knownForTitles)
        {
            Id = id;
            PrimaryName = primaryName;
            BirthYear = birthYear;
            DeathYear = deathYear;
            PrimaryProfessions = primaryProfessions;
            KnownForTitles = knownForTitles;
        }

        public int Id { get; set; }
        public string PrimaryName { get; set; }
        public int BirthYear { get; set; }
        public int DeathYear { get; set; }
        public List<Professions> PrimaryProfessions { get; set; }
        public List<string> KnownForTitles { get; set; }
    }
}
