using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    /// <summary>
    /// States for the Card position and animation
    /// note: this will not be used in minimap/log. there will be seperate sprites for that.
    /// Draw Pile where cards can be drawn,
    /// discard pile
    /// 
    /// </summary>
    // Start is called before the first frame update
public enum CardState
        {
          toDrawPile,
        drawPile,
          toDiscardPile,
        discardPile,
          toViewCard,
        ViewCardPosition,
          toOpponent,    //not sure if this will be used when showing opponents hand
        OpponentsHand,   //not sure if this will be used when using peek.
          toHand,
        hand,
          toTarget,
        target,
        discard,
          to,
        idle,
    }
/// <summary>
/// The Special Card properties of a Ratatatcat
/// </summary>
public class CardRATC : Card {

    static public float MOVE_DURATION = 0.5f;
    static public string MOVE_EASING = Easing.InOut;
    static public float CARD_HEIGHT = 3.5f;
    static public float CARD_WIDTH = 2f;

    [Header("Set Dynamically: CardRatatatCat")]
    public CardState state = CardState.drawPile;


    // Fields to store info the card will use to move and rotate
    public List<Vector3> bezierPts;
    public List<Quaternion> bezierRots;
    public float timeStart, timeDuration;
    public int eventualSortOrder;
    public string eventualSortLayer;

    // When the card is done moving, it will call reportFinishTo.SendMessage()
    public GameObject reportFinishTo = null;
    [System.NonSerialized]
    public RATCPlayer callbackPlayer = null;

    // MoveTo tells the card to interpolate to a new position and rotation
    public void MoveTo(Vector3 ePos, Quaternion eRot)
    {
        // Make new interpolation lists for the card.
        // Position and Rotation will each have only two points.
        bezierPts = new List<Vector3>();
        bezierPts.Add(transform.localPosition); // Current position
        bezierPts.Add(ePos); // Current rotation

        bezierRots = new List<Quaternion>();
        bezierRots.Add(transform.rotation); // New position
        bezierRots.Add(eRot); // New rotation

        if (timeStart == 0)
        {
            timeStart = Time.time;
        }
        // timeDuration always starts the same but can be overwritten later
        timeDuration = MOVE_DURATION;

        state = CardState.to;
    }

    public void MoveTo(Vector3 ePos)
    {
        MoveTo(ePos, Quaternion.identity);
    }

    private void Update()
    {
        switch (state)
        {
            case CardState.toDrawPile:
            case CardState.toDiscardPile:
            case CardState.toHand:
            case CardState.toOpponent:
            case CardState.toTarget:
            case CardState.toViewCard:
            case CardState.to:
                float u = (Time.time - timeStart) / timeDuration;
                float uC = Easing.Ease(u, MOVE_EASING);

                if (u < 0)
                {
                    transform.localPosition = bezierPts[0];
                    transform.rotation = bezierRots[0];
                    return;
                }
                else if (u >= 1)
                {
                    uC = 1;
                    // Move from the to... state to the proper next state
                    if (state == CardState.toHand) state = CardState.hand;
                    if (state == CardState.toTarget) state = CardState.target;
                    if (state == CardState.toDrawPile) state = CardState.drawPile;
                    if (state == CardState.to) state = CardState.idle;

                    // Move to the final position
                    transform.localPosition = bezierPts[bezierPts.Count - 1];
                    transform.rotation = bezierRots[bezierPts.Count - 1];

                    // Reset timeStart to 0 so it gets overwritten next time
                    timeStart = 0;

                    if (reportFinishTo != null)
                    {
                        reportFinishTo.SendMessage("CBCallback", this);
                        reportFinishTo = null;
                    }
                    else if (callbackPlayer != null)
                    {
                        // If there's a callback Player
                        // Call CBCallback directly on the Player
                        callbackPlayer.CBCallback(this);
                        callbackPlayer = null;
                    }
                    else
                    {
                        // If there is nothing to callback
                        // Just let it stay still.
                    }
                }
                else
                {
                    // Normal interpolation behavior (0 <= u < 1)
                    Vector3 pos = Utils.Bezier(uC, bezierPts);
                    transform.localPosition = pos;
                    Quaternion rotQ = Utils.Bezier(uC, bezierRots);
                    transform.rotation = rotQ;

                    if (u > 0.5f)
                    {
                        SpriteRenderer sRend = spriteRenderers[0];
                        if (sRend.sortingOrder != eventualSortOrder)
                        {
                            // Jump to the proper sort order
                            SetSortOrder(eventualSortOrder);
                        }
                        if (sRend.sortingLayerName != eventualSortLayer)
                        {
                            // Jump to the proper sort layer
                            SetSortingLayerName(eventualSortLayer);
                        }
                    }
                }
                break;
        }
    }

    // This allows the card to react to being clicked
    public override void OnMouseUpAsButton()
    {
        // Call the CardClicked method on the Bartok singleton
        RatatatCat.S.CardClicked(this);
        // Also call the base class (Card.cs) version of this method
        base.OnMouseUpAsButton();
    }
}
