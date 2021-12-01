using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrentTurnPhase
{
    idle,
    pre,
    waiting,
    post,
    gameOver
}
public enum CurrentPlayerTurn
{
    Dealer,
    North,
    East,
    South,
    West,
    Scoring,
   
}

/// <summary>
/// struct to organize player's cards and know them at all times.
/// </summary>
public struct CARDSINHAND{ int CardOne, CardTwo, CardThree, CardFour; }

public class RatatatCat : MonoBehaviour
{
    static public RatatatCat S;
    public RATCPlayer CURRENT_PLAYER;

    /// <summary>
    /// These get updated whenever a player has a card.
    /// maybe this data will be used to score
    /// </summary>
    CARDSINHAND P1_Hand;
    CARDSINHAND P2_Hand;
    CARDSINHAND P3_Hand;
    CARDSINHAND P4_Hand;

    public TextAsset RatatatCatDeck;

    //will be specific layout
    public TextAsset layoutXML;

    public Vector3 layoutCenter = Vector3.zero;
 //   public float handFanDegrees = 10f;
 //   public int numStartingCards = 4;
    public float drawTimeStagger = 0.1f;

    [Header("Set Dynamically")]
    public RATCDeck deck;
    public List<CardRATC> drawPile;
    public List<CardRATC> discardPile;
    public List<Player> players;
    public CardRATC targetCard;
    public CurrentTurnPhase phase = CurrentTurnPhase.idle;
//    public TurnPhase phase = TurnPhase.idle;


    //didnt touch layout
    private RATCLayout layout;
    private Transform layoutAnchor;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        deck = GetComponent<RATCDeck>(); // Get the RATC Deck
        deck.InitDeck(RatatatCatDeck.text); // Pass DeckXML to it
        Deck.Shuffle(ref deck.cards); // This shuffles the deck

        layout = GetComponent<RATCLayout>(); // Get the Layout
        layout.ReadLayout(layoutXML.text); // Pass LayoutXML to it

        drawPile = UpgradeCardsList(deck.cards);
        LayoutGame();
    }

    List<CardRATC> UpgradeCardsList(List<Card> lCD)
    {
        List<CardRATC> lCB = new List<CardRATC>();
        foreach (Card tCD in lCD)
        {
            lCB.Add(tCD as CardRATC);
        }
        return (lCB);
    }
    void LayoutGame() {
        print("LayoutGame");
            }

    public void CardClicked(CardRATC tCB)
    {
     //   if (CURRENT_PLAYER.type != PlayerType.human) return;
        if (phase == CurrentTurnPhase.waiting) return;

        switch (tCB.state)
        {
            case CardState.drawPile:
                /**
                // Draw the top card, not necessarily the one clicked.
                CardRATC cb = CURRENT_PLAYER.AddCard(Draw());
                cb.callbackPlayer = CURRENT_PLAYER;
                Utils.tr("RATC:CardClicked()", "Draw", cb.name);
                phase = CurrentTurnPhase.waiting;
                */
                break;

            case CardState.hand:
   /**
    * // Check to see whether the card is valid
                if (ValidPlay(tCB))
                {
                    CURRENT_PLAYER.RemoveCard(tCB);
                    MoveToTarget(tCB);
                    tCB.callbackPlayer = CURRENT_PLAYER;
                    Utils.tr("RATC:CardClicked()", "Play", tCB.name, targetCard.name + " is target");
                    phase = TurnPhase.waiting;
                }
                else
                {
                    // Just ignore it but report what the player tried
                    Utils.tr("Bartok:CardClicked()", "Attempted to Play", tCB.name, targetCard.name + " is target");
                }
   */
                break;
        }
    }
}
