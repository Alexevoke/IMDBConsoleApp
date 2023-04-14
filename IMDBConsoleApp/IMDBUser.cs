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
            bool run = true;
            while(run)
            {
                Console.WriteLine("Hvis du gerne vil søge på en titel tryk 1");
                Console.WriteLine("Hvis du gerne vil søge på en person tryk 2");
                Console.WriteLine("Hvis du gerne vil tilføje en titel tryk 3");
                Console.WriteLine("Hvis du her vil tilføje en person tryk 4");
                Console.WriteLine("Hvis du gerne vil slette en film fra databasen tryk 5");
                Console.WriteLine("For at afslutte programmet tryk 6");
                char c = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (c)
                {
                    case '1':
                        {
                            Console.WriteLine("Indtast titlen på den film du vil finde: ");
                            string? search = Console.ReadLine();

                            Console.WriteLine("Search:");
                            List<Title> titles = TitleQueries.TitleSearch(search);

                            Console.WriteLine("Du har fundet " + titles.Count + " titler");
                            Console.WriteLine("Tryk Enter for at fortsætte");
                            Console.ReadKey();

                            foreach (Title title in titles)
                            {
                                Console.WriteLine("Primarytitle: " + title.PrimaryTitle);
                                Console.WriteLine("Originaltitle: " + title.OriginalTitle);
                                Console.WriteLine("Titletype: " + title.TitleType);
                                Console.WriteLine("IsAdult: " + title.IsAdult);
                                Console.WriteLine("Startyear: " + title.StartYear);
                                Console.WriteLine("Endyear: " + title.EndYear);
                                Console.WriteLine("Runtime(min): " + title.RuntimeMinutes);
                                Console.Write("Genres: ");
                                foreach (GenreType genre in title.Genres)
                                {
                                    Console.Write(genre + " | ");
                                }
                                Console.WriteLine();
                                Console.WriteLine();
                            }
                            break;
                        }
                    case '2':
                        {
                            Console.WriteLine("Indtast navnet på den person du vil finde: ");
                            string? search = Console.ReadLine();

                            Console.WriteLine("Search:");
                            List<Person> persons = NameQueries.PersonSearch(search);

                            Console.WriteLine("Du har fundet " + persons.Count + " personer");
                            Console.WriteLine("Tryk Enter for at fortsætte");
                            Console.ReadKey();

                            foreach (Person person in persons)
                            {
                                Console.WriteLine("Name: " + person.PrimaryName);
                                Console.WriteLine("Birthyear: " + person.BirthYear);
                                Console.WriteLine("Deathyear: " + person.DeathYear);
                                Console.Write("Professions: ");
                                foreach (Professions profession in person.PrimaryProfessions)
                                {
                                    Console.Write(profession + " | ");
                                }
                                Console.WriteLine();
                                Console.Write("Known for titles: ");
                                foreach (string title in person.KnownForTitles)
                                {
                                    Console.Write(title + " | ");
                                }
                                Console.WriteLine();
                                Console.WriteLine();

                            }
                            break;
                        }
                    case '3':
                        {
                            Title title = new Title();                          

                            Console.WriteLine("Indtast filmens titel: ");
                            title.PrimaryTitle = Console.ReadLine();

                            Console.WriteLine("Indtast filmens original titel: ");
                            title.OriginalTitle = Console.ReadLine();
                            Console.WriteLine("Er det porno? Indtast true hvis Ja/indtast false hvis Nej: ");
                            title.IsAdult = Convert.ToBoolean(Console.ReadLine());
                            Console.WriteLine("Indtast udgivelsesår: ");
                            title.StartYear = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Indtast slutår: ");
                            title.EndYear = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Indtast filmens varighed i minutter: ");
                            title.RuntimeMinutes = Convert.ToInt32(Console.ReadLine());
                            foreach (TitleType type in (TitleType[]) Enum.GetValues(typeof(TitleType)))
                            {
                                Console.WriteLine("Indtast " + (int)type + " for " + type);
                            }
                            title.TitleType = (TitleType)Convert.ToInt32(Console.ReadLine());
                            foreach (GenreType genre in (GenreType[])Enum.GetValues(typeof(GenreType)))
                            {
                                Console.WriteLine("Indtast " + (int)genre + " for " + genre);
                            }
                            title.Genres.Add((GenreType)Convert.ToInt32(Console.ReadLine()));
                            TitleQueries.AddTitle(title);
                            break;
                        }
                    case '4':
                        {
                            Person person = new Person();

                            Console.WriteLine("Indtast personens fulde navn: ");
                            string? navn = Console.ReadLine();
                            if (navn is null)
                            {
                                break;
                            }
                            person.PrimaryName = navn;
                            Console.WriteLine("Indtast fødselsår: ");
                            person.BirthYear = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Indtast dødsår");
                            person.DeathYear = Convert.ToInt32(Console.ReadLine());
                            foreach (Professions profession in (Professions[])Enum.GetValues(typeof(Professions)))
                            {
                                Console.WriteLine("Indtast " + (int)profession + " for " + profession);
                            }
                            person.PrimaryProfessions.Add((Professions)Convert.ToInt32(Console.ReadLine()));

                            NameQueries.AddPerson(person);
                            break;
                        }
                    case '5':
                        {
                            Console.WriteLine("Indtast titlen på den film du vil finde: ");
                            string? search = Console.ReadLine();

                            Console.WriteLine("Search:");
                            List<Title> titles = TitleQueries.TitleSearch(search);

                            Console.WriteLine("Du har fundet " + titles.Count + " titler");
                            Console.WriteLine("Tryk Enter for at fortsætte");
                            Console.ReadKey();

                            foreach (Title title in titles)
                            {
                                Console.WriteLine("Id: " + title.Id);
                                Console.WriteLine("Primarytitle: " + title.PrimaryTitle);
                                Console.WriteLine();
                            }

                            Console.WriteLine("Indtast Id'et på den film du vil slette");
                            int id = Convert.ToInt32(Console.ReadLine());

                            if (id == 0) 
                            {
                                break;
                            }

                            TitleQueries.DeleteTitle(id);

                            break;
                        }
                    case '6':
                        {
                            run = false;
                            break;
                        }
                }
            }

        }

    }
}
