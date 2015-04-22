using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OctopusAnagrams.Data
{
    public static class Extensions
    {
        public static string ToString<T>(this IEnumerable<T> l, string separator)
        {
            return "[" + String.Join(separator, l.Select(i => i.ToString()).ToArray()) + "]";
        }
    }

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
            Console.Out.WriteLine("Searching for: {0}", phrase);
            var results = new List<List<string>>();

            foreach (var c in phrase)
            {
                var resolved = Children.FirstOrDefault(r => r.IndexLetter == c);
                if (resolved != null)
                {
                    var remainder = phrase.Substring(0, phrase.IndexOf(c)) +
                                    phrase.Substring(phrase.IndexOf(c) + 1, phrase.Length - (phrase.IndexOf(c) + 1));
                    Console.Out.WriteLine(remainder);
                    var result = resolved.SearchString(remainder, this);
                    if (result != null)
                    {
                        results.AddRange(result);
                    }
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

        public List<List<string>> SearchString(string phrasePortion, WordListTree dictionary)
        {
            var results = new List<List<string>>();
            Console.Out.WriteLine("Looking at {0}", phrasePortion);
            if (phrasePortion.Length == 0)
            {
                if (ResolvedWord != null)
                {
                    Console.Out.WriteLine(ResolvedWord);
                    return new List<List<string>> {new List<string> {ResolvedWord}};
                }
            }
            foreach (var c in phrasePortion)
            {
                var resolved = Children.FirstOrDefault(r => r.IndexLetter == c);
                if (resolved != null)
                {
                    Console.Out.WriteLine(resolved.IndexLetter);
                    //if (ResolvedWord != null)
                    //{
                    //    Console.Out.WriteLine(ResolvedWord);
                    //    //restart search at beginning of dictionary
                    //    var r = dictionary.Search(phrasePortion);
                    //    foreach (var x  in r)
                    //    {
                    //        Console.Out.WriteLine("x: {0}", x.ToString(","));
                    //        x.Insert(0, ResolvedWord);
                    //        //                          Console.Out.WriteLine("postx: {0}", x.ToString(","));
                    //    }
                    //    return r;
                    //}

                    var remainder = phrasePortion.Substring(0, phrasePortion.IndexOf(c)) +
                                    phrasePortion.Substring(phrasePortion.IndexOf(c) + 1, phrasePortion.Length - (phrasePortion.IndexOf(c) + 1));
                    results.AddRange(resolved.SearchString(remainder, dictionary));
                }
            }
            return results;
//            return new List<List<string>>();
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