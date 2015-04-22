using System;
using System.Linq;
using OctopusAnagrams.Data;

namespace OctopusAnagrams
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Console.Out.WriteLine("Loading dictionary...");
            var parser = new WordListParser("masterWordlist.txt");
            var dictionary = parser.ParseWordList();

            Console.Out.WriteLine("Dictionary loaded with {0} entries", dictionary.NumberOfEntries);
            Console.Out.WriteLine("****");
            while (true)
            {
                Console.Out.WriteLine("Please enter a word to search for anagrams in (Ctrl-C to quit):");
                var phrase = Console.In.ReadLine();

                var results = dictionary.Search(phrase);

                if (results.Any())
                {
                    Console.Out.WriteLine("The following {0} words were found inside of {1}", results.Count, phrase);
                    foreach (var r in results.Values)
                    {
                        Console.Out.WriteLine("\t{0}", string.Join(" ", r));
                    }
                }
                else
                {
                    Console.Out.WriteLine("No anagrams found for {0}", phrase);
                }
            }
        }
    }
}