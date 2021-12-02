using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    Deck = 0,
    Discard = 1,
    LeftOuter = 2,
    LeftInner = 3,
    RightInner = 4,
    RightOuter = 5
}
public class Turn : MonoBehaviour
{
    private bool debug = false;
    int player;
    CardCat cardIn;
    Location source;
    CardCat cardOut;
    Location inDestination;
    Location outDesitnation;

    public Turn(int newPlayer, CardCat newIn, Location newSource, Location newInDest, CardCat newOut)
    {
        player = newPlayer;
        cardIn = newIn;
        source = newSource;
        inDestination = newInDest;
        cardOut = newOut;
    }

    public Turn(int player, CardCat newIn, Location newSource, Location newInDest)
    {
        new Turn(player, newIn, newSource, newInDest, null);
    }

    public void discard(CardCat newOut)
    {
        cardOut = newOut;
    }
    public string toString()
    {
        string message = string.Format("Player %d drew a ", player);
        if (debug || source.Equals(Location.Discard))
        {
            message += cardIn.rank;
        }
        else
        {
            message += "card ";
        }
        switch (source)
        {
            case Location.Deck:
                message += " from the Deck,";
                break;
            case Location.Discard:
                message += " from the Discard Pile,";
                break;
            default:
                message += "ERROR";
                break;
        }
        switch (inDestination)
        {
            case Location.Discard:
                message += " and discarded it.";
                break;
            case Location.LeftInner:
                message += " replaced their inner left card,";
                break;
            case Location.LeftOuter:
                message += " replaced their outer left card,";
                break;
            case Location.RightInner:
                message += " replaced their inner right card,";
                break;
            case Location.RightOuter:
                message += " replaced their outer right card,";
                break;

        }
        message += " and discarded a " + cardOut.rank + ".\n";
        return message;
    }


    public void debugSwitch()
    {
        debug = !debug;
    }
}
