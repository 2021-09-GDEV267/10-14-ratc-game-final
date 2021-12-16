using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Darc_CardDefinition
{
    public string cardName;
    public int cardValue;
    public Sprite cardImage;
    public bool isPowerCard => powerType != PowerType.None;
    public PowerType powerType;
    public enum PowerType
    {
        None,
        Swap,
        Peek,
        DrawTwo
    }
}

[System.Serializable]
public class Darc_CardRATC : MonoBehaviour
{
    public Darc_CardDefinition cardDefinition;
    public int currentOwnerNumber = 0;
    public int currentSlotNumber = 0;
}
