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

    [Header("Set in Inspector")]
    public TextAsset deckXML;
    public TextAsset layoutXML;
    public Vector3 layoutCenter = Vector3.zero;
    public int numStartingCards = 4;
    public float drawTimeStagger = 0.1f;
    public GameObject viewHand;
    public GameObject hand;
    public Text playerNum;
    public GameObject transition;
    public GameObject startGame;
    public GameObject cardShow;
    public bool pregame = true;

    [Header("Set Dynamically")]
    public Deck deck;
    public List<CardCat> drawpile;
    public List<CardCat> discardpile;
    public List<CatPlayer> players;
    public CardCat targetCard;
    public TurnPhaseCat phase = TurnPhaseCat.idle;
    public int index = 0;

    private CatLayout layout;
    private Transform layoutAnchor;

    void Awake()
    {
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        deck = GetComponent<Deck>();
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
            players.Add(pl);
            pl.playerNum = tSD.player;
        }
        players[0].type = PlayerTypeCat.human;

        CardCat tCC;

        for (int i = 0; i < numStartingCards; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                tCC = Draw();

                tCC.timeStart = Time.time + drawTimeStagger * (i * 4 + j);

                players[(j + 1) % 4].AddCard(tCC);
            }
        }
        if (pregame == true)
        {
            Invoke("DrawFirstTarget", drawTimeStagger * (numStartingCards * 4 + 4));
        }
    }

    public void DrawFirstTarget()
    {
        CardCat tCC = MoveToTarget(Draw());
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
            CURRENT_PLAYER = players[index];
            StartCoroutine(CanvasSet(viewHand, hand));
            playerNum.text = "Player " + CURRENT_PLAYER.playerNum;
        }
        else if(CURRENT_PLAYER.playerNum == 4)
        {
            StartCoroutine(CanvasSet(viewHand, hand));
            playerNum.text = "Player 1";
            cardShow.SetActive(false);
            startGame.SetActive(true);
        }
    }

    public void StartGame()
    {
        //This is assuming that the pre-game loop will transition to the game loop via scene transition
        SceneManager.LoadScene("Begin_Game");
    }

    //switch canvas to the canvas screen that is for viewing your hand
    public void View()
    {
        StartCoroutine(CanvasSet(hand, viewHand));
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
        tCC.state = CCState.discard;
        discardpile.Add(tCC);
        tCC.SetSortingLayerName(layout.discardPile.layerName);
        tCC.SetSortOrder(discardpile.Count * 4);
        tCC.transform.localPosition = layout.discardPile.pos + Vector3.back / 2;

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
