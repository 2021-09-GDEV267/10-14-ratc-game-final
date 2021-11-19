using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Log
{ 
    public enum PickEvent
    {
        draw,
        discard
    }

    public enum PlayEvent
    {
        peek,
        swap,
        drawTwo
    }

    public Log(int playerNumber, PickEvent pickEvent, PlayEvent playEvent)
    {
        this.pickEvent = pickEvent;
        this.playEvent = playEvent;
        this.playerNumber = playerNumber;
        logNumber = NumberInLog;
        NumberInLog++;
    }
    
    [SerializeField] PickEvent pickEvent;
    [SerializeField] PlayEvent playEvent;
    [SerializeField] int playerNumber;
    static int NumberInLog;
    [SerializeField] int logNumber;
    [SerializeField] string logDescription;

    public string Description => $"Log Number: {logNumber} || Player {playerNumber} picked from the {pickEvent} and played {playEvent}";

    public int LogNumber => logNumber;
}

public class Logger : MonoBehaviour
{
    public static Logger S;
    [SerializeField] GameObject logCanvas;
    [SerializeField] GameObject logButton;
    [SerializeField] List<Log> gameLog = new List<Log>();

    void Awake() => S = this;

    public void AddLog(int playerNumber, Log.PickEvent pickEvent, Log.PlayEvent playEvent) => gameLog.Add(new Log(playerNumber, pickEvent, playEvent));

    public void DisplayLog()
    {
        logButton.SetActive(false);
        logCanvas.SetActive(true);
    }

    public void HideLog()
    {
        logCanvas.SetActive(false);
        logButton.SetActive(true);
    }

    public void WriteLogs()
    {
        foreach (var l in gameLog)
        {
            print($"{l.Description}");
        }
    }
}
