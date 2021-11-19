using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLogTester : MonoBehaviour
{
    public void Start()
    {
        Log firstLog = new Log(1);
        firstLog.Picked = Log.PickEvent.draw;
        firstLog.Played = Log.PlayEvent.peek;
        Logger.S.AddLog(firstLog);

        Log secondLog = new Log(2);
        secondLog.Picked = Log.PickEvent.discard;
        secondLog.Played = Log.PlayEvent.drawTwo;
        Logger.S.AddLog(secondLog);

        Log thirdLog = new Log(3);
        thirdLog.Picked = Log.PickEvent.draw;
        thirdLog.Played = Log.PlayEvent.swap;
        Logger.S.AddLog(thirdLog);

        Log fourthLog = new Log(4);
        fourthLog.Picked = Log.PickEvent.draw;
        fourthLog.Played = Log.PlayEvent.peek;
        Logger.S.AddLog(fourthLog);

        Log fifthLog = new Log(1);
        fifthLog.Picked = Log.PickEvent.discard;
        fifthLog.Played = Log.PlayEvent.swap;
        Logger.S.AddLog(fifthLog);

        Log sixthLog = new Log(2);
        sixthLog.Picked = Log.PickEvent.discard;
        sixthLog.Played = Log.PlayEvent.drawTwo;
        Logger.S.AddLog(sixthLog);
    }


    [ContextMenu("Print Logs")]
    public void PrintLogs() => Logger.S.WriteLogs();
}
