# octopusanagrams

##Rough Description
This loads a list of words from a file (one per line) into an internal dictionary format and allows for the searching of valid anagrams inside of an entered string.

##Implementation Details
The first step is to load the words into the dictionary.  The dictionary I chose to create is essentially a tree of nodes, linking character to character, with each node also possilbly having a valid word that it resolves to. 
