# GraphStuff
This repository contains a project I made in my Algorithms and Data Structures course. 

The primary goal is to find the most connected movie stars among 500,000+ actors from the Internet Movie Database. 

The file is massive so it can be obtained on their webpage.

Detailed information can be found in the code, but essentially a very connected actor, Kevin Bacon, is taken as a starting node. His connected component is traversed through DFS to establish a list of other potentially well connected actors. From here, BFS is performed on a random sample of this list in hopes to find anyone better than Kevin Bacon. I ran a test overnight on about 10,000 actors and found nobody better!
