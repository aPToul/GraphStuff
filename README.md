# GraphStuff

This repository contains a project I made in my Algorithms and Data Structures course. 

The primary goal is to find the most connected actor among 500,000+ from the Internet Movie Database.  
A connection is undirected and defined as working in the same movie.

The file is massive so it can be obtained on their webpage.

CODE SUMMARY:

A very connected actor, Kevin Bacon, is taken as a starting node. His connected component is traversed through DFS to establish a list of other potentially well connected actors. BFS is performed on a random sample of this list in hopes to find someone better than Kevin Bacon, using average degree of separation as a metric. 


RESULTS:

I ran a test overnight on about 10% of Kevin's connections and found nobody more connected!
Detailed comments can be found in the code :)

