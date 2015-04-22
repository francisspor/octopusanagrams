using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OctopusAnagrams.Data
{
    public static class Extensions
    {
        public static string Hash<T>(this IEnumerable<T> l)
        {
            return l.ToString("+").GetHashCode().ToString( CultureInfo.CurrentCulture);
        }

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

        public Dictionary<string, List<string>> Search(string phrase)
        {
            var results = new Dictionary<string, List<string>>();

            char[] a = phrase.ToCharArray();
            Array.Sort(a);
            var sortedPhrase = new string(a);

            foreach (var c in sortedPhrase)
            {
                var resolved = Children.FirstOrDefault(r => r.IndexLetter == c);
                if (resolved != null)
                {
                    var remainder = sortedPhrase.Substring(0, sortedPhrase.IndexOf(c)) +
                                    sortedPhrase.Substring(sortedPhrase.IndexOf(c) + 1,
                                        sortedPhrase.Length - (sortedPhrase.IndexOf(c) + 1));
                    var result = resolved.SearchString(remainder, this);
                    if (result != null)
                    {
                        foreach (var x in result.Values)
                        {
                            var hash = x.Hash();
                            if (!results.ContainsKey(hash))
                            {
                                results.Add(hash, x);
                            }
                        }
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

        public Dictionary<string, List<string>> SearchString(string phrasePortion, WordListTree dictionary)
        {
            var results = new Dictionary<string, List<string>>();
            if (phrasePortion.Length == 0)
            {
                if (ResolvedWord != null)
                {
                    var item = new List<string> {ResolvedWord};
                    var hash = item.Hash();
                    if (!results.ContainsKey(hash))
                    {
                        results.Add(hash, item);
                    }
                }
            }
            foreach (var c in phrasePortion)
            {
                var resolved = Children.FirstOrDefault(r => r.IndexLetter == c);
                if (resolved != null)
                {
                    if (ResolvedWord != null)
                    {
                        //restart search at beginning of dictionary
                        var r = dictionary.Search(phrasePortion);
                        foreach (var x in r.Values)
                        {
                            // have to jam the resolved word into the generated results
                            x.Insert(0, ResolvedWord);

                            var hash = x.Hash();
                            if (!results.ContainsKey(hash))
                            {
                                results.Add(hash, x);
                            }
                        }
                    }

                    // Crop it down, removing that letter
                    var remainder = phrasePortion.Substring(0, phrasePortion.IndexOf(c)) +
                                    phrasePortion.Substring(phrasePortion.IndexOf(c) + 1,
                                        phrasePortion.Length - (phrasePortion.IndexOf(c) + 1));

                    var tempResults = resolved.SearchString(remainder, dictionary);

                    foreach (var x in tempResults.Values)
                    {
                        var hash = x.Hash();
                        if (!results.ContainsKey(hash))
                        {
                            results.Add(hash, x);
                        }
                    }
                }
            }
            return results;
        }
    }
}