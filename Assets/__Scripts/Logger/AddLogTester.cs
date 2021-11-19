using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLogTester : MonoBehaviour
{
    public void Start()
    {
        Logger.S.AddLog(1, Log.PickEvent.draw, Log.PlayEvent.peek);
        Logger.S.AddLog(2, Log.PickEvent.discard, Log.PlayEvent.swap);
        Logger.S.AddLog(3, Log.PickEvent.draw, Log.PlayEvent.drawTwo);
        Logger.S.AddLog(4, Log.PickEvent.draw, Log.PlayEvent.peek);
        Logger.S.AddLog(1, Log.PickEvent.discard, Log.PlayEvent.swap);
        Logger.S.AddLog(2, Log.PickEvent.draw, Log.PlayEvent.drawTwo);
        Logger.S.AddLog(3, Log.PickEvent.discard, Log.PlayEvent.swap);
        Logger.S.AddLog(4, Log.PickEvent.draw, Log.PlayEvent.peek);
    }


    [ContextMenu("Print Logs")]
    public void PrintLogs()
    {
        Logger.S.WriteLogs();
    }
}
