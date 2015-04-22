using System;
using System.IO;

namespace OctopusAnagrams.Data
{
    public class WordListParser
    {
        private readonly string fileName;

        public WordListParser(string fileName)
        {
            this.fileName = fileName;
        }

        public WordListTree ParseWordList()
        {
            var list = new WordListTree ();
            using (var fileReader = new StreamReader(string.Format("WordLists/{0}", fileName)))
            {
                string word;
                while ((word = fileReader.ReadLine()) != null)
                {
                    list.Parse(word);
                }
            }
            return list;
        }
    }
}