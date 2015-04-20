using System;
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
            Console.Out.WriteLine(dictionary);
            Console.In.ReadLine();
        }
    }
}