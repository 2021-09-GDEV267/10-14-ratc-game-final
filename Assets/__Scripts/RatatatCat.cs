using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TurnPhaseCat
{
    idle,
    pre,
    waiting,
    post,
    gameOver
}

public class RatatatCat : MonoBehaviour
{
    static public RatatatCat S;
    static public CatPlayer CURRENT_PLAYER;
    static public CatPlayer player2;

    [Header("Set in Inspector")]
    public TextAsset deckXML;
    public TextAsset layoutXML;
    public Vector3 layoutCenter = Vector3.zero;
    public int numStartingCards = 4;
    public int discardCount = 0;
    public float drawTimeStagger = 0.1f;
    public GameObject viewHand;
    public GameObject hand;
    public Text playerNum;
    public Text viewPlay;
    public GameObject transition;
    public GameObject startGame;
    public GameObject cardShow;
    //public bool pregame = true;
    public int discardCount = 0;

    [Header("Set Dynamically")]
    public CatDeck deck;
    public List<CardCat> drawpile;
    public List<CardCat> discardpile;
    public List<CatPlayer> players;
    public CardCat targetCard;
    public TurnPhaseCat phase = TurnPhaseCat.idle;
    public int index = 0;
    public CardCat swap;
    public CardCat swap2;
    public CardCat drawSelection;
    public CardCat discardSelection;
    public bool canClick = false;

    private CatLayout layout;
    private Transform layoutAnchor;

    void Awake()
    {
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        deck = GetComponent<CatDeck>();
        deck.InitDeck(deckXML.text);
        Deck.Shuffle(ref deck.cards);
        deck.cards = deck.cards as List<Card>;

        layout = GetComponent<CatLayout>();
        layout.ReadLayout(layoutXML.text);

        drawpile = UpgradeCardsList(deck.cards);
        LayoutGame();
    }

    public void ArrangeDrawPile()
    {
        CardCat tCC;

        for (int i = 0; i < drawpile.Count; i++)
        {
            tCC = drawpile[i];
            tCC.transform.SetParent(layoutAnchor);
            tCC.transform.localPosition = layout.drawPile.pos;
            tCC.faceUp = false;
            tCC.SetSortingLayerName(layout.drawPile.layerName);
            tCC.SetSortOrder(-i * 4);
            tCC.state = CCState.drawpile;
        }
    }

    public void LayoutGame()
    {
        if (layoutAnchor == null)
        {
            GameObject tGO = new GameObject("LayoutAnchor");
            layoutAnchor = tGO.transform;
            layoutAnchor.transform.position = layoutCenter;
        }

        ArrangeDrawPile();

        CatPlayer pl;
        players = new List<CatPlayer>();
        foreach (SlotDefCat tSD in layout.slotDefs)
        {
            pl = new CatPlayer();
            pl.handSlotDef = tSD;
            pl.type = PlayerTypeCat.human;
            players.Add(pl);
            pl.playerNum = tSD.player;
        }

        CardCat tCC;

        for (int i = 0; i < numStartingCards; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                tCC = Draw();

                tCC.timeStart = Time.time + drawTimeStagger * (i * 4 + j);

                tCC.handIndex = j;

                players[(j + 1) % 4].AddCard(tCC);
            }
        }
        Invoke("DrawFirstTarget", drawTimeStagger * (numStartingCards * 4 + 4));
        
    }

    public void DrawFirstTarget()
    {
        CardCat tCC = MoveToDiscard(Draw());
        tCC.reportFinishTo = this.gameObject;
    }

    public void CCCallback(CardCat cc)
    {
        Utils.tr("Rat-a-tat-cat:CCCallback", cc.name);
        StartCoroutine(CanvasSet(viewHand, hand));
        CURRENT_PLAYER = players[index];
    }

    //switch canvas screens from one to another
    private IEnumerator CanvasSet(GameObject obj, GameObject deactivate)
    {

        deactivate.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        obj.SetActive(true);
        yield return null;
    }

    //switch canvas to the canvas screen that is for passing to the next player
    public void ViewStart()
    {
        if (CURRENT_PLAYER.playerNum < 4)
        {
            index++;
            phase = TurnPhaseCat.idle;
            CURRENT_PLAYER = players[index];
            StartCoroutine(CanvasSet(viewHand, hand));
            playerNum.text = "Player " + CURRENT_PLAYER.playerNum;
            if (!canClick) {
                viewPlay.text = "View Cards";
            }
            else
            {
                viewPlay.text = "Take Turn";
            }
        }
        else if (CURRENT_PLAYER.playerNum == 4)
        {
            StartCoroutine(CanvasSet(viewHand, hand));
            playerNum.text = "Player 1";
            if (!canClick)
            {
                cardShow.SetActive(false);
                startGame.SetActive(true);
            }
            else
            {
                StartGame();
            }
        }
    }

    public void StartGame()
    {
        index = 0;
        CURRENT_PLAYER = players[0];
        viewPlay.text = "Take Turn";
        startGame.SetActive(false);
        cardShow.SetActive(true);
        canClick = true;
    }

    //switch canvas to the canvas screen that is for viewing your hand
    public void View()
    {
        StartCoroutine(CanvasSet(hand, viewHand));
        phase = TurnPhaseCat.idle;
    }

    public CardCat MoveToTarget(CardCat tCC)
    {
        tCC.timeStart = 0;
        tCC.MoveTo(layout.discardPile.pos + Vector3.back);
        tCC.state = CCState.toTarget;
        tCC.faceUp = true;

        tCC.SetSortingLayerName("10");
        tCC.eventualSortLayer = layout.target.layerName;
        if (targetCard != null)
        {
            MoveToDiscard(targetCard);
        }

        targetCard = tCC;

        return (tCC);
    }

    public CardCat MoveToDiscard(CardCat tCC)
    {
        float ydex = 0;
        tCC.state = CCState.discard;
        tCC.handIndex = -1;
        discardpile.Add(tCC);
        tCC.SetSortingLayerName(layout.discardPile.layerName);
        if (discardCount == 0)
        {
            tCC.SetSortOrder(-100 + (discardpile.Count * 3));
        }
        else
        {
            tCC.eventualSortOrder = -100 + (discardCount * 3);
        }
        tCC.eventualSortLayer = layout.discardPile.layerName;
        tCC.eventualSortOrder = -100 + (discardCount * 3);

        if (canClick)
        {
            ydex = -98;
            layout.discardPile.pos.y = ydex;
        }

        tCC.MoveTo((layout.discardPile.pos + Vector3.back / 2), Quaternion.Euler(0, 0, 0));
        tCC.faceUp = true;
        discardCount++;

        return (tCC);
    }

    public CardCat Draw()
    {
        CardCat cc = drawpile[0];
        if (drawpile.Count == 0)
        {
            int ndx;
            while (discardpile.Count > 0)
            {
                ndx = Random.Range(0, discardpile.Count);
                drawpile.Add(discardpile[ndx]);
                discardpile.RemoveAt(ndx);
            }
            ArrangeDrawPile();

            float t = Time.time;
            foreach (CardCat tCC in drawpile)
            {
                tCC.transform.localPosition = layout.discardPile.pos;
                tCC.callbackPlayer = null;
                tCC.MoveTo(layout.drawPile.pos);
                tCC.timeStart = t;
                t += 0.02f;
                tCC.state = CCState.toDrawpile;
                tCC.eventualSortLayer = "0";
            }
        }
        drawpile.RemoveAt(0);
        return (cc);
    }

    public void CardClicked(CardCat tCC)
    {
        if (CURRENT_PLAYER.type != PlayerTypeCat.human) return;
        if (phase == TurnPhaseCat.waiting) return;
        if (!canClick) return;
        Debug.Log("not skipped");
        if (tCC.state == CCState.discard)
        {
            Debug.Log("You clicked on the discard pile!");

            if (drawSelection != null)
            {
                DrawToDiscard(drawSelection);
                drawSelection = null;
            }
            else if (discardSelection != null)
            {
                discardSelection = null;
            }
            else{
                discardSelection = tCC;
            }
        }
        else if (tCC.state == CCState.drawpile)
        {
            Debug.Log("You clicked on the draw pile!");
            drawSelection = tCC;
            tCC.faceUp = true;
            tCC.SetSortOrder(100);
        }
        else if(tCC.state == CCState.hand)
        {
            Debug.Log("You clicked on the player 1's hand!");
            if (discardSelection != null)
            {
                SwapDiscard(discardSelection, tCC, tCC.handIndex);
                discardSelection = null;
                tCC.callbackPlayer = CURRENT_PLAYER;
                phase = TurnPhaseCat.waiting;
            }
            else if (drawSelection != null)
            {
                SwapDraw(drawSelection, tCC, tCC.handIndex);
                drawSelection = null;
                tCC.callbackPlayer = CURRENT_PLAYER;
                phase = TurnPhaseCat.waiting;
            }
        }
    }

    public void SwapDiscard(CardCat discard, CardCat hand, int handIndex)
    {
        swap = discard;
        swap2 = hand;
        RatatatCat.CURRENT_PLAYER.hand[handIndex] = swap;
        swap.MoveTo(swap2.transform.position, swap2.transform.rotation);
        swap.state = CCState.toHand;
        swap.handIndex = handIndex;
        swap.faceUp = false;
        RatatatCat.S.MoveToDiscard(swap2);
        RatatatCat.S.discardpile.Remove(discard);
        swap2.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (CURRENT_PLAYER.type != PlayerTypeCat.human) return;
        if (phase == TurnPhaseCat.waiting) return;
    }

    public void SwapDraw(CardCat draw, CardCat hand, int handIndex)
    {
        swap = draw;
        swap2 = hand;
        RatatatCat.CURRENT_PLAYER.hand[handIndex] = swap;
        swap.MoveTo(swap2.transform.position, swap2.transform.rotation);
        swap.state = CCState.toHand;
        swap.handIndex = handIndex;
        swap.faceUp = false;
        RatatatCat.S.MoveToDiscard(swap2);
        RatatatCat.S.drawpile.Remove(draw);
        swap2.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (CURRENT_PLAYER.type != PlayerTypeCat.human) return;
        if (phase == TurnPhaseCat.waiting) return;
    }

    public void DrawToDiscard(CardCat drawCard)
    {
        MoveToDiscard(drawCard);
    }

    public bool ValidPlay(CardCat cc)
    {
        if (cc.rank == targetCard.rank) return (true);
        if (cc.suit == targetCard.suit)
        {
            return (true);
        }
        return (false);
    }

    List<CardCat> UpgradeCardsList(List<Card> lCD)
    {
        List<CardCat> lCC = new List<CardCat>();
        foreach (Card tCD in lCD)
        {
            lCC.Add(tCD as CardCat);
        }
        return (lCC);
    }
}
