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

    public Log(int playerNumber) => _playerNumber = playerNumber;

    PickEvent _pickEvent;
    PlayEvent _playEvent;

    GameObject[] _cardsPeeked;
    GameObject[] _cardsSwapped;

    int _playerNumber;
    int _logNumber;

    public PickEvent Picked
    {
        get => _pickEvent;
        set => _pickEvent = value;
    }

    public PlayEvent Played
    {
        get => _playEvent;
        set => _playEvent = value;
    }

    public int LogNumber
    {
        get => _logNumber;
        set => _logNumber = value;
    }

    public void SetCardsPeeked(GameObject playerCard, GameObject opponentsCard) => _cardsPeeked = new GameObject[] { playerCard, opponentsCard };

    public void SetCardsSwapped(GameObject playerCard, GameObject opponentsCard) => _cardsSwapped = new GameObject[] { playerCard, opponentsCard };

    public GameObject[] GetCardsPeeked() => _cardsPeeked;

    public GameObject[] GetCardsSwapped() => _cardsSwapped;

    public string Description => $"Log Number: {_logNumber} || Player {_playerNumber} picked from the {_pickEvent} pile and played {_playEvent}";

    public int PlayerNumber => _playerNumber;
}

public class Logger : MonoBehaviour
{
    public static Logger S;
    [SerializeField] GameObject logCanvas;
    [SerializeField] GameObject logButton;
    [SerializeField] List<Log> gameLog = new List<Log>();

    void Awake() => S = this;

    public void AddLog(Log log)
    {
        gameLog.Add(log);
        log.LogNumber = gameLog.Count - 1;
    }

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
            print($"{l.Description}");
    }
}
