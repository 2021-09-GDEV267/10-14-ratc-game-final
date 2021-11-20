using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLogTester : MonoBehaviour
{
    public void Start()
    {
        int currentPlayer = 0;  
        currentPlayer = 1;
       
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.draw, PlayEvent.peek, new GameObject("Fat Cat Test Object")));
        
        currentPlayer = 2; 
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.discard, new GameObject("Messy Cat Test Object"), new GameObject("Clean Cat Test Object")));
        
        currentPlayer = 3;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.draw, PlayEvent.replace, new GameObject("Lazy Cat Test Object")));
        
        currentPlayer = 4;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.draw, PlayEvent.drawTwo));
        
        currentPlayer = 4;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.discard, PlayEvent.replace, new GameObject("Fat Cat Test Object")));

        PrintLogs();
    }


    [ContextMenu("Print Logs")]
    public void PrintLogs() => Logger.S.PrintLogs();
}
