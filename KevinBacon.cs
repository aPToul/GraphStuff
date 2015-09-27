/*  
Input: Text file with number of actors on first line, 
       followed by an adjacency list (tab seperated)
       of incrementing IDs starting from 0.
       Kevin Bacon's ID (below).
       Kevin Bacon's average distance (below).

Example text file: 
5
0   2
1   2
2   0   1
3   4
4   3
...

Goal: Compare Kevin Bacon to other actors via DFS/BFS.
      Choose a random collection of 1/2 of his connected component.

Output: The number of sampled users with better connectivity than the well-connected Kevin Bacon. 
 
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Collections;

class Program
{
    static void Main()
    {
        // Read adj.txt file, which will be used to produce the graph.
        StreamReader reader = new StreamReader("adj.txt");

        try
        {
            /*  Create a new array containing lists.
                Set capacity to the number of unique users on first line. */

            int numberOfActors = int.Parse(reader.ReadLine());
            List<int>[] array = new List<int>[numberOfActors];

            /* Read each line and split it by tabs (default).
               The first element of the created array is the user ID. 
               Following elements are adjacent IDs.
            */

            string currLine = reader.ReadLine();
            while (currLine != null)
            {
                string[] lineSplit = currLine.Split();

                int idNumber = int.Parse(lineSplit[0]);

                /* Due to ordering, the index of a given ID is its ID number.
                   Create a new list to store adjacent IDs. */               
                array[idNumber] = new List<int>();
                
                for (int i = 1; i < lineSplit.Length; ++i)
                {
                    // Append each adjacent ID number to the user's list
                    array[idNumber].Add(int.Parse(lineSplit[i]));
                }

                // Next ID in text file
                currLine = reader.ReadLine();
            }

            reader.Close();

            // Kevin Bacon's ID
            int kevBac = 359910;

            // Find everyone in Kevin Bacon's connected component.
            bool[] baconTracker = new bool[array.Length];
            zDFS(array, baconTracker, kevBac);

            // List of Kevin Bacon's connected component to be sampled from.
            List<int> randomIDs = new List<int>();
            for (int i = 0; i < baconTracker.Length; ++i)
            {
                if (baconTracker[i] == true && i != kevBac) 
                {
                    // Add everyone to a list who is connected to Kevin Bacon.
                    randomIDs.Add(i);
                }
            }

            Random randomGen = new Random();

            /* Randomized swapping of all positions in a set creates a random set.
	           [Cormen et al., Introduction to Algorithms, 3rd edition, MIT Press] */

            for (int i = 0; i < randomIDs.Count - 1; ++i)
            {
                int temporary = randomIDs[i];
                int randomValue = randomGen.Next(0, randomIDs.Count);
                randomIDs[i] = randomIDs[randomValue];
                randomIDs[randomValue] = temporary;
            }

            /* Average distance for Kevin Bacon */
            /* Established from previous analysis */
            double baconDist = ((double)(1319167)) / ((double)(503944));

            /* Keep track of the total users judged, and the number
               which are actually better than Kevin. */

            int betterThanKevin = 0;
            int total = 0;

            // Call BFS for floored half of the connected IDs
            for (int i = 0; i < (randomIDs.Count/2); ++i)
            {
                total = total + 1;
		
		        /* Create new array that represent
                   whether each ID has been visited */
		
                bool[] tracker = new bool[array.Length];

                betterThanKevin = betterThanKevin + zBFS(array, tracker, randomIDs[i], baconDist);
            }

            // Write the stats into a text file.

            StreamWriter sw = new StreamWriter("KevinSpecialOrNot.txt");
            sw.WriteLine(betterThanKevin);
            sw.WriteLine(total);
            sw.WriteLine(randomIDs.Count);
            sw.Close();
        }

        catch (Exception e)
        {
            Console.Error.WriteLine("Caught unhandled exception: " + e);
        }

    }

    /*  
       Perform depth first search on a given ID to find connected users
       Use a stack to avoid stack overflow
    */

    static void zDFS(List<int>[] array, bool[] baconTracker, int i)
    {
        /* Since a node exists, the size of the current
           connected component is one. */

        int size = 1;

        Stack<int> vertToVis = new Stack<int>();

        // Push the starting node to the stack
        vertToVis.Push(i);
        // Starting node is visited
        baconTracker[i] = true;

        while (vertToVis.Count != 0)
        {           
            i = vertToVis.Pop();

            // For each node connected to the current node
            for (int k = 0; k < array[i].Count; ++k)
            {
                // For each node not visited
                if (baconTracker[array[i][k]] == false)
                {
                    vertToVis.Push(i);
                    // Add to the unvisited node to the stack
                    // Traverse as far as possible, possibly eliminating re-added nodes
                    vertToVis.Push(array[i][k]);
                    baconTracker[array[i][k]] = true;
                    size = size + 1;
                }
            }
        }
    }

    /* 
       Perform BFS for a given user.
       Use a queue to avoid stack overflow.
       If the average distance exceeds the Kevin Bacon's, return 1. 
       Otherwise, return 0.
    */

    static int zBFS(List<int>[] array, bool[] tracker, int i, double baconDist)
    {
        // Set the distance of unvisited nodes to -1
        int[] dist = new int[array.Length];
        for (int k = 0; k < array.Length; ++k)
        {
            dist[k] = -1;
        }

        tracker[i] = true;
        dist[i] = 0;

        // Add the starting node to the queue
        Queue<int> workWith = new Queue<int>();
        workWith.Enqueue(i);

        while (workWith.Count > 0)
        {
            int eval = workWith.Dequeue();

            for (int j = 0; j < array[eval].Count; ++j)
            {
                if (tracker[array[eval][j]] == false)
                {
                    tracker[array[eval][j]] = true;
                    // Increment the distance of the node from the start node
                    dist[array[eval][j]] = dist[eval] + 1;
                    // Add the adjacent node to the queue
                    workWith.Enqueue(array[eval][j]);
                }
            }
        }

        /* Distances have been determined.
           Compute statistics for user.
           Compare to Kevin Bacon. */

        int distanceSum = 0;
        int distanceDenom = 0;

        for (int k = 0; k < array.Length; ++k)
        {
            if (dist[k] > 0)
            {
                distanceSum = distanceSum + dist[k];
                distanceDenom = distanceDenom + 1;
            }
        }

        if (distanceDenom == 0)
        {
            return 0;
        }
        else
        {
            double average = ((double)distanceSum) / ((double)distanceDenom);

            if ((average <= baconDist))
            {
                Console.WriteLine("Impressive.");
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}