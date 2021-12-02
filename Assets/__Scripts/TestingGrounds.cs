using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingGrounds : MonoBehaviour
{
    private CatPlayer player;
    private CatPlayer player2;
    public CardCat swap;
    public CardCat swap2;
    public int swapNum;
    public int swapNum2;
    void Start()
    {
        RatatatCat.S.MoveToDiscard(RatatatCat.S.Draw());
    }

    public void SwapDraw(CardCat discard, CardCat hand, int handIndex)
    {
        player = RatatatCat.CURRENT_PLAYER;
        swap = discard;
        swap2 = hand;
        swapNum = handIndex;
        player.hand[swapNum] = swap;
        RatatatCat.S.MoveToDiscard(swap2);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            player = RatatatCat.S.players[0];
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            player = RatatatCat.S.players[1];
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            player = RatatatCat.S.players[2];
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            player = RatatatCat.S.players[3];
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            swap = player.hand[0];
            player.hand[0] = null;
            swapNum = 0;
            Debug.Log("Detected R");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            swap = player.hand[1];
            player.hand[1] = null;
            swapNum = 1;
            Debug.Log("Detected R");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            swap = player.hand[2];
            player.hand[2] = null;
            swapNum = 2;
            Debug.Log("Detected R");
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            swap = player.hand[3];
            player.hand[3] = null;
            swapNum = 3;
            Debug.Log("Detected R");
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            player2 = RatatatCat.S.players[0];
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            player2 = RatatatCat.S.players[1];
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            player2 = RatatatCat.S.players[2];
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            player2 = RatatatCat.S.players[3];
        }

        if (Input.GetKeyUp(KeyCode.U))
        {
            swap2 = player2.hand[0];
            player2.hand[0] = null;
            swapNum2 = 0;
            player.hand[swapNum] = swap2;
            player2.hand[swapNum2] = swap;
            Debug.Log("Detected D");
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            swap2 = player2.hand[1];
            player2.hand[1] = null;
            swapNum2 = 1;
            player.hand[swapNum] = swap2;
            player2.hand[swapNum2] = swap;
            Debug.Log("Detected D");
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            swap2 = player2.hand[2];
            player2.hand[2] = null;
            swapNum2 = 2;
            player.hand[swapNum] = swap2;
            player2.hand[swapNum2] = swap;
            Debug.Log("Detected D");
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            swap2 = player2.hand[3];
            player2.hand[3] = null;
            swapNum2 = 3;
            player.hand[swapNum] = swap2;
            player2.hand[swapNum2] = swap;
            Debug.Log("Detected D");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            RatatatCat.CURRENT_PLAYER = RatatatCat.S.players[0];
            SwapDraw(RatatatCat.S.discardpile[0], RatatatCat.S.players[0].hand[0], 0);
        }
    }
}
