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

        public void Parse(string word)
        {
            var wordCharArray = word.ToCharArray();

            var result = Children.FirstOrDefault(f => f.IndexLetter == wordCharArray[0]);
            if (result == null)
            {
                result = new WordListTreeNode {IndexLetter = wordCharArray[0]};
                Children.Add(result);
            }
            result.Parse(wordCharArray, word);
        }

        public override string ToString()
        {
            string result = "";
            foreach (var c in Children)
            {
                result += string.Format("{0}:\r\n\t{1}" + Environment.NewLine, c.IndexLetter, c);
            }
            return result;
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

        public override string ToString()
        {
            string result = "";
            foreach (var c in Children)
            {
                result += string.Format("{0}: {1} \r\n\t{2}", c.IndexLetter, c.ResolvedWord, c);
            }
            return result;
        }
    }
}