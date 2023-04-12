using IMDBClassLibrary.Model;
using IMDBConsoleApp.Queries;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Threading.Channels;
using IMDBConsoleApp.IDGenerators;

namespace IMDBConsoleApp
{
    public class IMDBUser
    {

        

        public void Start()
        {
            //Console.WriteLine("Indtast titlen på den film du vil finde: ");
            //string? search = Console.ReadLine();

            //Console.WriteLine("Search:");
            //List<Title> titles = TitleQueries.TitleSearch(search);


            //foreach (Title title in titles)
            //{
            //    Console.WriteLine("Primarytitle: " + title.PrimaryTitle);
            //    Console.WriteLine("Originaltitle: " + title.OriginalTitle);
            //    Console.WriteLine("Titletype: " + title.TitleType);
            //    Console.WriteLine("IsAdult: " + title.IsAdult);
            //    Console.WriteLine("Startyear: " + title.StartYear);
            //    Console.WriteLine("Endyear: " + title.EndYear);
            //    Console.WriteLine("Runtime(min): " + title.RuntimeMinutes);
            //    Console.Write("Genres: ");
            //    foreach (string genre in title.Genres)
            //    {
            //        Console.Write(genre + " | ");
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}
            //Console.WriteLine(titles.Count);

            //Console.WriteLine("Indtast navnet på den person du vil finde: ");
            //string? search = Console.ReadLine();

            //Console.WriteLine("Search:");
            //List<Person> persons = NameQueries.PersonSearch(search);

            //foreach (Person person in persons)
            //{
            //    Console.WriteLine("Name: " + person.PrimaryName);
            //    Console.WriteLine("Birthyear: " + person.BirthYear);
            //    Console.WriteLine("Deathyear: " + person.DeathYear);
            //    Console.Write("Professions: ");
            //    foreach (string profession in person.PrimaryProffesions)
            //    {
            //        Console.Write(profession + " | ");
            //    }
            //    Console.WriteLine();
            //    Console.Write("Known for titles: ");
            //    foreach (string title in person.KnownForTitles)
            //    {
            //        Console.Write(title + " | ");
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine();

            //}
            //Console.WriteLine(persons.Count);

            //TconstGenerator generator = new TconstGenerator();

            //string tconst = generator.GenerateTconst();
            //Console.WriteLine(tconst);

            

        }

    }
}
