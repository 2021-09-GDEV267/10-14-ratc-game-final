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
        _pickEvent = pickEvent;
        _playEvent = playEvent;
        _playerNumber = playerNumber;
        _logNumber = NumberInLog;
        NumberInLog++;
    }

    static int NumberInLog;

    PickEvent _pickEvent;
    PlayEvent _playEvent;
    int _playerNumber;
    int _logNumber;

    public string Description => $"Log Number: {_logNumber} || Player {_playerNumber} picked from the {_pickEvent} pile and played {_playEvent}";

    public int LogNumber => _logNumber;
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
