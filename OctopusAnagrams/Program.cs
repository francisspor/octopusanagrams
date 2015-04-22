using System;
using System.Linq;
using CommandLine;
using OctopusAnagrams.Data;

namespace OctopusAnagrams
{
    public class Options
    {
        [Option('f', DefaultValue = "testWordlist.txt", Required = true)]
        public string FileName { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //           var options = new Options();
            //           if (Parser.Default.ParseArguments(args, options))
            //           {
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
                    foreach (var r in results)
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