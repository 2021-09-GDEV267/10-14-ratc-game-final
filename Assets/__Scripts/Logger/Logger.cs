using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//event to set which pile player picked from
public enum PickEvent
{
    draw,
    discard
}

//event to set which action the player does
public enum PlayEvent
{
    drawTwo,
    peek,
    swap,
    replace,
    discard
}

public class Log
{
    //log constructor used for drawTwo and discard 
    public Log(int playerNumber, PickEvent pickEvent, PlayEvent playEvent)
    {
        _pickEvent = pickEvent;
        _playEvent = playEvent;
        _playerNumber = playerNumber;
    }

    //log constructor for replace and peek
    public Log(int playerNumber, PickEvent pickEvent, PlayEvent playEvent, GameObject card)
    {
        _pickEvent = pickEvent;
        _playerNumber = playerNumber;
        _playEvent = playEvent;
       
        switch (playEvent)
        {
            case PlayEvent.peek:
                _cardPeeked = card;
                break;
            case PlayEvent.replace:
                _cardReplaced = card;
                break;
        }
    }

    //log constuctor used for swap
    public Log(int playerNumber, PickEvent pickEvent, GameObject playerCard, GameObject opponentCard)
    {
        _pickEvent = pickEvent;
        _playerNumber = playerNumber;
        _playEvent = PlayEvent.swap;
        _cardsSwapped = new GameObject[2];
        _cardsSwapped[0] = playerCard;
        _cardsSwapped[1] = opponentCard;
    }
   
    PickEvent _pickEvent;
    PlayEvent _playEvent;

    GameObject _cardPeeked;
    GameObject _cardReplaced;
    GameObject[] _cardsSwapped;

    int _playerNumber;
    int _logNumber;

    public PickEvent Picked => _pickEvent;

    public PlayEvent Played => _playEvent;

    public int LogNumber
    {
        get => _logNumber;
        set => _logNumber = value;
    }

    public GameObject GetCardPeeked() => _cardPeeked;

    public GameObject GetCardSwapped(int indx)
    {
        if (indx >= 0 && indx < _cardsSwapped.Length)
            return _cardsSwapped[indx];
        return null;
    }

    public GameObject GetReplaced() => _cardReplaced;

    public bool HasReplaced() => _cardReplaced != null;

    public bool HasPeeked() => _cardPeeked != null;

    public bool HasSwapped() => _cardsSwapped != null;

    public string Description => $"Log Number: {_logNumber} || Player {_playerNumber} picked from the {_pickEvent} pile and played {_playEvent}";

    public int PlayerNumber => _playerNumber;
}

public class Logger : MonoBehaviour
{
    public static Logger S;
    [SerializeField] GameObject logCanvas;
    [SerializeField] GameObject logButton;
    List<Log> gameLog = new List<Log>();

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

    public void PrintLogs()
    {
        foreach (var l in gameLog)
        {
            if (l.HasSwapped())
                print($"Log Number: {l.LogNumber} || player {l.PlayerNumber} picked from the {l.Picked} pile and swapped: {l.GetCardSwapped(0).name} with opponents card: {l.GetCardSwapped(1).name}");
            else if (l.HasPeeked())
                print($"Log Number: {l.LogNumber} || player {l.PlayerNumber} picked from the {l.Picked} pile and peeked at: {l.GetCardPeeked().name}");
            else if (l.HasReplaced())
                print($"Log Number: {l.LogNumber} || player {l.PlayerNumber} picked from the {l.Picked} pile and replaced card: {l.GetReplaced().name}");
            else
                print(l.Description);
        }
    }
}
