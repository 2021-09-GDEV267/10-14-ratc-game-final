using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLogTester : MonoBehaviour
{
    public void Start()
    {
        int currentPlayer = 0;  
        
        GameObject messyCat = new GameObject("Messy Cat Test Object");
        GameObject cleanCat = new GameObject("Clean Cat Test Object");
        GameObject fatCat = new GameObject("Fat Cat Test Object");
        GameObject lazyCat = new GameObject("Lazy Cat Test Object");

        currentPlayer = 1;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.draw, PlayEvent.peek, fatCat));
        
        currentPlayer = 2;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.discard, messyCat, cleanCat));
        
        currentPlayer = 3;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.draw, PlayEvent.replace, lazyCat));
        
        currentPlayer = 4;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.draw, PlayEvent.drawTwo));
        
        currentPlayer = 4;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.discard, PlayEvent.replace, fatCat));

        currentPlayer = 5;
        Logger.S.AddLog(new Log(currentPlayer, PickEvent.draw, PlayEvent.discard));

        PrintLogs();
    }


    [ContextMenu("Print Logs")]
    public void PrintLogs() => Logger.S.PrintLogs();
}
