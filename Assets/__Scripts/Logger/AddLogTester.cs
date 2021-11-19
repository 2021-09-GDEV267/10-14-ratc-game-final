using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLogTester : MonoBehaviour
{
    public void Start()
    {
        Log log0 = new Log(1);
        log0.Picked = Log.PickEvent.draw;
        log0.Played = Log.PlayEvent.peek;
        Logger.S.AddLog(log0);

        Log log1 = new Log(2);
        log1.Picked = Log.PickEvent.discard;
        log1.Played = Log.PlayEvent.drawTwo;
        Logger.S.AddLog(log1);

        Log log2 = new Log(3);
        log2.Picked = Log.PickEvent.draw;
        log2.Played = Log.PlayEvent.swap;
        log2.SetCardsSwapped(new GameObject("Crazy Cat"), new GameObject("Lazy Cat"));
        Logger.S.AddLog(log2);

        Log log3 = new Log(4);
        log3.Picked = Log.PickEvent.draw;
        log3.Played = Log.PlayEvent.peek;
        Logger.S.AddLog(log3);

        Log log4 = new Log(1);
        log4.Picked = Log.PickEvent.discard;
        log4.Played = Log.PlayEvent.swap;
        Logger.S.AddLog(log4);

        Log log5 = new Log(2);
        log5.Picked = Log.PickEvent.discard;
        log5.Played = Log.PlayEvent.drawTwo;
        Logger.S.AddLog(log5);

        PrintLogs();

        GameObject[] cardsSwapped = log2.GetCardsSwapped();

        if (cardsSwapped != null)
            print($"Log: {log2.LogNumber} || Swapped; player card: {cardsSwapped[0].name} || Swapped; opponents card: {cardsSwapped[1].name}");
    }


    [ContextMenu("Print Logs")]
    public void PrintLogs() => Logger.S.WriteLogs();
}
