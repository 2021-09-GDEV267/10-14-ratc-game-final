using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingGrounds : MonoBehaviour
{
    private CatPlayer player2;
    public CardCat swap;
    public CardCat swap2;
    void Start()
    {
        RatatatCat.S.MoveToDiscard(RatatatCat.S.Draw());
    }
    //discard to hand
    public void SwapDiscard(CardCat discard, CardCat hand, int handIndex)
    {
        swap = discard;
        swap2 = hand;
        RatatatCat.CURRENT_PLAYER.hand[handIndex] = swap;
        swap.MoveTo(swap2.transform.position, swap2.transform.rotation);
        swap.state = CCState.toHand;
        swap.faceUp = false;
        RatatatCat.S.MoveToDiscard(swap2);
        RatatatCat.S.discardpile.Remove(discard);
        swap2.transform.rotation = Quaternion.Euler(0, 0, 0);
        TurnLog.addToLog(new Turn(RatatatCat.CURRENT_PLAYER.playerNum, discard, Location.Discard, (Location)handIndex, hand));
    }
    //draw to hand
    public void SwapDraw(CardCat draw, CardCat hand, int handIndex)
    {
        swap = draw;
        swap2 = hand;
        RatatatCat.CURRENT_PLAYER.hand[handIndex] = swap;
        swap.MoveTo(swap2.transform.position, swap2.transform.rotation);
        swap.state = CCState.toHand;
        RatatatCat.S.MoveToDiscard(swap2);
        swap2.transform.rotation = Quaternion.Euler(0, 0, 0);
        TurnLog.addToLog(new Turn(RatatatCat.CURRENT_PLAYER.playerNum, draw, Location.Deck, (Location)handIndex, hand));
    }
    // swap between players
    public void SwapPlayer(CardCat handChoice, CardCat opponentChoice, CatPlayer opponent, int handIndex, int opponentIndex)
    {
        swap = handChoice;
        player2 = opponent;
        swap2 = opponentChoice;
        RatatatCat.CURRENT_PLAYER.hand[handIndex] = swap2;
        swap.MoveTo(swap2.transform.position, swap2.transform.rotation);
        swap2.MoveTo(swap.transform.position, swap.transform.rotation);
        player2.hand[opponentIndex] = swap;
    }
    // drawn to discard
    public void DrawToDiscard(CardCat drawCard)
    {
        RatatatCat.S.MoveToDiscard(drawCard);
        TurnLog.addToLog(new Turn(RatatatCat.CURRENT_PLAYER.playerNum, drawCard, Location.Deck, Location.Discard, drawCard));
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.S))
        {
            RatatatCat.CURRENT_PLAYER = RatatatCat.S.players[0];
            SwapDiscard(RatatatCat.S.discardpile[0], RatatatCat.CURRENT_PLAYER.hand[0], 0);
            TurnLog.matchReport();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            RatatatCat.CURRENT_PLAYER = RatatatCat.S.players[1];
            SwapDraw(RatatatCat.S.Draw(), RatatatCat.CURRENT_PLAYER.hand[0], 0);
            TurnLog.matchReport();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            RatatatCat.CURRENT_PLAYER = RatatatCat.S.players[0];

            SwapPlayer(RatatatCat.CURRENT_PLAYER.hand[1], RatatatCat.S.players[1].hand[0], RatatatCat.S.players[1], 1, 0);
            TurnLog.matchReport();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            DrawToDiscard(RatatatCat.S.Draw());
            TurnLog.matchReport();
        }
    }
}

