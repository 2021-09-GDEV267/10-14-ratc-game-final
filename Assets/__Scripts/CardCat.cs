using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CCState
{
    toDrawpile,
    drawpile,
    toHand,
    hand,
    toTarget,
    target,
    discard,
    to,
    idle
}

public class CardCat : Card
{
    static public float MOVE_DURATION = 0.5f;
    static public string MOVE_EASING = Easing.InOut;
    static public float CARD_HEIGHT = 3.5f;
    static public float CARD_WIDTH = 2f;

    [Header("Set Dynamically: CardCat")]
    public CCState state = CCState.drawpile;
    public List<Vector3> bezierPts;
    public List<Quaternion> bezierRots;
    public float timeStart, timeDuration;
    public string eventualSortLayer;
    public int eventualSortOrder;
    public int handIndex = -1;

    public GameObject reportFinishTo = null;

    [System.NonSerialized]
    public CatPlayer callbackPlayer = null;
    //public SlotDef slotDef;

    public void MoveTo(Vector3 ePos, Quaternion eRot)
    {
        bezierPts = new List<Vector3>();
        bezierPts.Add(transform.localPosition);
        bezierPts.Add(ePos);

        bezierRots = new List<Quaternion>();
        bezierRots.Add(transform.rotation);
        bezierRots.Add(eRot);

        if (timeStart == 0)
        {
            timeStart = Time.time;
        }

        timeDuration = MOVE_DURATION;

        state = CCState.to;
    }


    public void MoveTo(Vector3 ePos)
    {
        MoveTo(ePos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CCState.toHand:
            case CCState.toTarget:
            case CCState.toDrawpile:
            case CCState.to:
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
                    if (state == CCState.toHand) state = CCState.hand;
                    if (state == CCState.toTarget) state = CCState.target;
                    if (state == CCState.toDrawpile) state = CCState.drawpile;
                    if (state == CCState.to) state = CCState.idle;

                    transform.localPosition = bezierPts[bezierPts.Count - 1];
                    transform.rotation = bezierRots[bezierPts.Count - 1];

                    timeStart = 0;

                    if (reportFinishTo != null)
                    {
                        reportFinishTo.SendMessage("CCCallback", this);
                        reportFinishTo = null;
                    }
                    else if (callbackPlayer != null)
                    {
                        callbackPlayer.CCCallback(this);
                        callbackPlayer = null;
                    }
                    else
                    {

                    }
                }
                else
                {
                    Vector3 pos = Utils.Bezier(uC, bezierPts);
                    transform.localPosition = pos;
                    Quaternion rotQ = Utils.Bezier(uC, bezierRots);
                    transform.rotation = rotQ;

                    if (u > 0.5f)
                    {
                        SpriteRenderer sRend = spriteRenderers[0];
                        if (sRend.sortingOrder != eventualSortOrder)
                        {
                            SetSortOrder(eventualSortOrder);
                        }
                        if (sRend.sortingLayerName != eventualSortLayer)
                        {
                            SetSortingLayerName(eventualSortLayer);
                        }
                    }
                }
                break;
        }
    }

    //override public void OnMouseUpAsButton()
    //{
    //    RatatatCat.S.CardClicked(this);
    //    base.OnMouseUpAsButton();
    //}
}
