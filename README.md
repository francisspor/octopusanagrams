# octopusanagrams

##Rough Description
This loads a list of words from a file (one per line) into an internal dictionary format and allows for the searching of valid anagrams inside of an entered string.

##Implementation Details
The first step is to load the words into the dictionary.  The dictionary I chose to create is essentially a tree of nodes, linking character to character, with each node also possibly having a valid word that it resolves to. 

https://raw.githubusercontent.com/francisspor/octopusanagrams/master/Images/OctopusDictionaryLayout.png

For each new word the parser starts at the top of the tree, popping characters off the front, stepping down a level in the tree, creating a child if need be, and when the string is empty, puts the entire word in the ResolvedWord of the final child.  Word by word the tree is built, each additional word walking through letter by letter, adding new letters as children to the previous if need be.  Building the dictionary isn't fast, but only needs to happen once.

Next, it asks for a word to generate anagrams from.  It takes that string, alphabetizes the letters, and starts stepping through the dictionary, letter by letter, passing the remainder of the string onto the next level.  If the node has a ResolvedWord, the remainder string is run through the dictionary again, to see what can be found, for the remainder.  If any are found, then that counts.  For example, given the image above, if the word boy is used, that string is alphabetized to 'boy' (bad example, I guess), and the b node is stepped to, and the b node's children is searched for o (the current head of the string).  Since there's an o, the remaining string of y is passed to that node for further searching.  Once in the o node, it's children is searched for y.  Ther is a y, and that is gone to, and the resolved string of BOY is returned.  If we wanted to search for BOD, it would go B->O, and then look for a D.  There is no D, so the result is empty, and null.

The biggest trick was if a resolved word was found early, the remainder needs to be put back into the dictionary to search for more words.  So, using the example from above, if I want to find anagrams using the phrase 'abyob', it would search through the tree, find that 'ab' is a word, and then would restart searching with 'yob'.  Through one of it's iterations of those remaining three characters, it's found that 'boy' is valid.  Meaning that the complete anagram for 'abyob' is 'ab boy'.  'bb' would have also matched, but there's no valid word in the dictionary using the letters 'ayo', so that wouldn't be returned.

The tree is just searched like this until all possible combinations of the string are searched against the dictionary, and then list of results is returned.  There had to be a small hack to make the results be a dictionary, and hashing the string values for the key to the results dictionary.  If I didn't do that, repeats would be inserted, especially if there were repeated letters in the string, and the number of anagrams exploded.  The hash removed that possiblity.

##Other options
I briefly considered a few other designs, like jamming the words into a hashtable and trying randomly to break up the word to see if they resolved, but that felt easy to miss stuff, and I wasn't sure how I'd build the more complex multi-word anagrams.  I also thought about using sql server and making it happen using a similar query to what I descibed, but once I figured out the tree I was happy with it, at least enough to move forward with.

##Things I would have done differently in retrospect
In fiddling, I learned about (Trie)[http://en.wikipedia.org/wiki/Trie], and I think that might be a neat way of making my tree smaller, and make searches faster.  I'm also not entirely sure as I sit here that this works either for one letter words in the dictionary and that's probably a bug.
