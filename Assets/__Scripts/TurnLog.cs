using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class TurnLog
{
    private static List<Turn> allTurns;
    private static Queue<Turn> recentTurns;
    
    static TurnLog()
    {
        allTurns = new List<Turn>();
        recentTurns = new Queue<Turn>(3);
    }

    public static void addToLog(Turn toAdd)
    {
        allTurns.Add(toAdd);
        if(recentTurns.Count == 4)
        {
            recentTurns.Dequeue();
        }
        recentTurns.Enqueue(toAdd);
    }
    public static void roundReport()
    {
        Turn[] lastRound = new Turn[3];
        recentTurns.CopyTo(lastRound, 0);
        foreach(Turn t in lastRound){
            System.Console.WriteLine(t.toString());
        }
    }

    public static void matchReport()
    {
        foreach(Turn t in allTurns)
        {
            System.Console.WriteLine(t.toString());
        }
    }

    public static void matchReportToFile()
    {
        List<string> matchReportString = new List<string>();
        foreach(Turn t in allTurns)
        {
            matchReportString.Add(t.toString());
        }
        System.IO.File.WriteAllLines("matchReport.txt", matchReportString);
    }


}
