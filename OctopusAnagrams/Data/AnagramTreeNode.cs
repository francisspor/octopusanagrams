using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusAnagrams.Data
{
    public class WordListTree
    {
        public WordListTree()
        {
            Children = new List<WordListTreeNode>();
        }

        public IList<WordListTreeNode> Children { get; private set; }
        public int NumberOfEntries { get; private set; }

        public void Parse(string word)
        {
            NumberOfEntries += 1;

            var wordCharArray = word.ToCharArray();

            var result = Children.FirstOrDefault(f => f.IndexLetter == wordCharArray[0]);
            if (result == null)
            {
                result = new WordListTreeNode {IndexLetter = wordCharArray[0]};
                Children.Add(result);
            }
            result.Parse(wordCharArray, word);
        }

        public List<List<string>> Search(string phrase)
        {
            var results = new List<List<string>>();
            foreach (var c in phrase.ToCharArray().OrderBy(x => x))
            {
                var resolved = Children.FirstOrDefault(r => r.IndexLetter == c);
                if (resolved != null)
                {
                    var remainder = phrase.Remove(phrase.IndexOf(c), 1);
                    results.AddRange(resolved.SearchString(remainder, new List<string>(), results, this));
                }
            }
            return results;
        }
    }

    public class WordListTreeNode
    {
        public void Parse(char[] wordArray, string rootWord)
        {
            if (wordArray.Length == 1)
            {
                ResolvedWord = rootWord;
            }
            else
            {
                var result = Children.FirstOrDefault(f => f.IndexLetter == wordArray[1]);
                if (result == null)
                {
                    result = new WordListTreeNode {IndexLetter = wordArray[1]};
                    Children.Add(result);
                }
                result.Parse(wordArray.Skip(1).Take(wordArray.Length - 1).ToArray(), rootWord);
            }
        }

        public WordListTreeNode()
        {
            Children = new List<WordListTreeNode>();
        }

        public char IndexLetter { get; set; }

        public string ResolvedWord { get; set; }

        public List<WordListTreeNode> Children { get; private set; }

        public List<List<string>> SearchString(string phrasePortion, List<string> soFar, List<List<string>> allResults, WordListTree dictionary )
        {
            if (phrasePortion.Length == 0)
            {
                if (ResolvedWord != null)
                {
                    soFar.Add(ResolvedWord);
                    allResults.Add(soFar);
                }
            }
            foreach (var c in phrasePortion.ToCharArray())
            {
                Console.Out.WriteLine(phrasePortion);
                var resolved = Children.FirstOrDefault(r => r.IndexLetter == c);
                if (resolved != null)
                {
                    Console.Out.WriteLine("In: {0}", phrasePortion);
                    var remainder = phrasePortion.Remove(phrasePortion.IndexOf(c), 1);
                    Console.Out.WriteLine("Out: {0}", remainder);

                    if (ResolvedWord != null)
                    {
                        soFar.Add(ResolvedWord);
                        //restart search at beginning of dictionary
                        allResults.AddRange(dictionary.Search(remainder));
                    }
                    else
                    {
                        allResults = resolved.SearchString(remainder, soFar, allResults, dictionary);
                    }
                }
            }
            return allResults;
        }

        //public List<List<string>> Search(string phrasePortion, List<List<string>> results)
        //{
        //    if (phrasePortion.Length == 0)
        //    {
        //        if (ResolvedWord != null)
        //        {
        //            results.Add(ResolvedWord);
        //            return results;
        //        }
        //        return null;
        //    }

        //    foreach (var c in phrasePortion.ToCharArray())
        //    {
        //        var resolved = Children.FirstOrDefault(r => r.IndexLetter == c);
        //        if (resolved != null)
        //        {
        //            if (ResolvedWord != null)
        //            {
        //                var remainder = phrasePortion.Remove(phrasePortion.IndexOf(c), 1);
        //                resolved.Search(remainder, results);
        //                results.Add(ResolvedWord);
        //            }
        //        }
        //    }
        //    return results;
        //}
    }
}