using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    LeftOuter = 0,
    LeftInner = 1,
    RightInner = 2,
    RightOuter = 3,
    Deck = 4,
    Discard = 5,
}
public class Turn : MonoBehaviour
{
    private bool debug = false;
    int player;
    CardCat cardIn;
    Location source;
    Location destination;
    CardCat cardOut;

  

    public Turn(int newPlayer, CardCat newIn, Location newSource, Location newDest, CardCat newOut)
    {
        player = newPlayer;
        cardIn = newIn;
        source = newSource;
        destination = newDest;
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
        switch (destination)
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


    public void setDebug(bool newDebug)
    {
        debug = newDebug;
    }
}
