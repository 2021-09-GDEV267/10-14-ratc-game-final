using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TurnLog : MonoBehaviour
{
    private List<Turn> allTurns;
    private Queue<Turn> recentTurns;
    
    public TurnLog()
    {
        allTurns = new List<Turn>();
        recentTurns = new Queue<Turn>(3);
    }

    public void addToLog(Turn toAdd)
    {
        allTurns.Add(toAdd);
        if(recentTurns.Count == 4)
        {
            recentTurns.Dequeue();
        }
        recentTurns.Enqueue(toAdd);
    }
    public void roundReport()
    {
        Turn[] lastRound = new Turn[3];
        recentTurns.CopyTo(lastRound, 0);
        foreach(Turn t in lastRound){
            print(t.toString());
        }
    }

    public void matchReport()
    {
        foreach(Turn t in allTurns)
        {
            print(t.toString());
        }
    }

    public void matchReportToFile()
    {
        List<string> matchReportString = new List<string>();
        foreach(Turn t in allTurns)
        {
            matchReportString.Add(t.toString());
        }
        System.IO.File.WriteAllLines("matchReport.txt", matchReportString);
    }


}
