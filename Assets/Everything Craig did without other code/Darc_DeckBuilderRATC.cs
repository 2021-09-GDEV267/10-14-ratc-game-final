using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darc_DeckBuilderRATC : MonoBehaviour
{
    public Darc_CardDefinition[] typesOfCards;
    public int[] amountPerType;
    [HideInInspector]
    public List<Darc_CardRATC> allCards = new List<Darc_CardRATC>();

    [ContextMenu("Build Cards")]
    public void BuildCards()
    {
        for (int i = 0; i < typesOfCards.Length; i++)
        {
            for (int j = 0; j < amountPerType[i]; j++)
            {
                GameObject newCardGO = new GameObject($"Name: {typesOfCards[i].cardName} Value: {typesOfCards[i].cardValue}");
                newCardGO.transform.parent = transform;
                Darc_CardRATC newCard = newCardGO.AddComponent<Darc_CardRATC>();
                newCard.cardDefinition = new Darc_CardDefinition();
                newCard.cardDefinition.cardName = typesOfCards[i].cardName;
                newCard.cardDefinition.cardImage = typesOfCards[i].cardImage;
                newCard.cardDefinition.cardValue = typesOfCards[i].cardValue;
                newCard.cardDefinition.powerType = typesOfCards[i].powerType;
                allCards.Add(newCard);
            }
        }
    }
}
