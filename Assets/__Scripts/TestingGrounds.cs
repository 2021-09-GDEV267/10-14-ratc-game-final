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

    public void SwapDiscard(CardCat discard, CardCat hand, int handIndex)
    {
        player = RatatatCat.CURRENT_PLAYER;
        swap = discard;
        swap2 = hand;
        swapNum = handIndex;
        player.hand[swapNum] = swap;
        swap.state = CCState.toHand;
        RatatatCat.S.MoveToDiscard(swap2);
        swap2.state = CCState.discard;
    }

    public void SwapDraw(CardCat draw, CardCat hand, int handIndex)
    {
        player = RatatatCat.CURRENT_PLAYER;
        swap = draw;
        swap2 = hand;
        swapNum = handIndex;
        player.hand[swapNum] = swap;
        swap.state = CCState.toHand;
        RatatatCat.S.MoveToDiscard(swap2);
        swap2.state = CCState.discard;
    }

    public void SwapPlayer(CardCat handChoice, CardCat opponentChoice, CatPlayer opponent, int handIndex, int opponentIndex)
    {
        player = RatatatCat.CURRENT_PLAYER;
        swap = handChoice;
        player.hand[handIndex] = null;
        swapNum = handIndex;
        player2 = opponent;
        swap2 = opponentChoice;
        player.hand[opponentIndex] = null;
        swapNum2 = opponentIndex;
        player.hand[swapNum] = swap2;
        player2.hand[swapNum2] = swap;
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
            SwapDiscard(RatatatCat.S.discardpile[0], RatatatCat.S.players[0].hand[0], 0);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            RatatatCat.CURRENT_PLAYER = RatatatCat.S.players[0];
            SwapDraw(RatatatCat.S.Draw(), RatatatCat.S.players[0].hand[0], 0);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            RatatatCat.CURRENT_PLAYER = RatatatCat.S.players[0];

            SwapPlayer(RatatatCat.CURRENT_PLAYER.hand[1], RatatatCat.S.players[1].hand[0], RatatatCat.S.players[1], 1,0);
        }
    }
}
