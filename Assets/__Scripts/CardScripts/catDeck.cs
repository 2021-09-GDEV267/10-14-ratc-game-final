using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class catDeck : MonoBehaviour {

    [Header("Set in Inspector")]
    public bool startFaceUp = false;
    [Header("Special Card Texts")]

    public Sprite DrawText;
    public Sprite PeekText;
    public Sprite SwapText;
    public Sprite[] faceSprites;
    public Sprite[] rankSprites;
    public Sprite cardBack;
    public Sprite cardFront;

    // Prefabs
    public GameObject prefabSprite;
    public GameObject prefabCardBg;
    public GameObject prefabCardFront;
    public GameObject[] prefabCardBgColors;

    [Header("Set Dynamically")]
    public PT_XMLReader xmlr;
    public List<string> cardNames;
    public List<Card> cards;
    public List<Decorator> decorators;
    public List<CardDefinition> cardDefs;
    public Transform deckAnchor;
    public Dictionary<string, Sprite> dictSuits;

    // InitDeck is called by Prospector when it is ready
    public void InitDeck(string deckXMLText)
    {
        // This creates an anchor for all the Card GameObjects in the Hierarchy
        if (GameObject.Find("_Deck") == null)
        {
            GameObject anchorGO = new GameObject("_Deck");
            deckAnchor = anchorGO.transform;
        }

        /*
         * 
        // Initialize the Dictionary of SuitSprites with necessary Sprites
        dictSuits = new Dictionary<string, Sprite>()
        {
            {"C", suitClub},
            {"D", suitDiamond},
            {"H", suitHeart},
            {"S", suitSpade}
        } 
        *
        */

        ReadDeck(deckXMLText);

        MakeCards();
    }

    // ReadDeck parses the XML file passed to it into CardDefinitions
    public void ReadDeck(string deckXMLText)
    {
        xmlr = new PT_XMLReader(); // Create a new PT_XMLReader
        xmlr.Parse(deckXMLText); // Use that PT_XMLReader to parse DeckXML

        // This prints a test line to show you how xmlr can be used.
        // For more information read about XML in the Useful Concepts Appendix
        string s = "xml[0] decorator[o]";
        s += "type=" + xmlr.xml["xml"][0]["decorator"][0].att("type");
        s += "x=" + xmlr.xml["xml"][0]["decorator"][0].att("x");
        s += "y=" + xmlr.xml["xml"][0]["decorator"][0].att("y");
        s += "scale=" + xmlr.xml["xml"][0]["decorator"][0].att("scale");
        print(s);

        // Read decorators for all Cards

        decorators = new List<Decorator>(); // Init the list of Decorators

        // Grab an PT_XMLHashList of all <decorator>s in the XML file
        PT_XMLHashList xDecos = xmlr.xml["xml"][0]["decorator"];

        Decorator deco;

        //Card info displayed in corners
        for (int i=0; i<xDecos.Count; i++)
        {
            // For each <decorator> in the XML
            deco = new Decorator(); // Make a new Decorator
            // Copy the attributes of the <decorator> to the Decorator
            deco.number = xDecos[i].att("number");
            // bool deco.flip is true if the text of the flip attribute is "1"
            deco.flip = (xDecos[i].att("flip") == "1");
            // floats need to be parsed from the attribute strings
            deco.scale = float.Parse(xDecos[i].att("scale"));
            // Vector3 loc initializes to [0,0,0], so we just need to modify it
            deco.loc.x = float.Parse(xDecos[i].att("x"));
            deco.loc.y = float.Parse(xDecos[i].att("y"));
            deco.loc.z = float.Parse(xDecos[i].att("z"));
            // Add the temporary deco to the List decorators
            decorators.Add(deco);
        }

        // Read card info
        cardDefs = new List<CardDefinition>(); // Init the List of Cards
        // Grab an PT_XMLHashList of all the <card>s in the XML file
        PT_XMLHashList xCardDefs = xmlr.xml["xml"][0]["card"];
        for (int i=0; i<xCardDefs.Count; i++)
        {
            // For each of the <card>s
            // Create a new CardDefinition
            CardDefinition cDef = new CardDefinition();
            // Parse the attribute values and add them to cDef
            cDef.rank = int.Parse(xCardDefs[i].att("rank"));
            // Grab an PT_XMLHashList of all the <pip>s on this <card>
            PT_XMLHashList xPips = xCardDefs[i]["pip"];
            
            
            //no pips in cat cards
            /*
            if(xPips != null)
            {
                for(int j=0; j<xPips.Count; j++)
                {
                    // Iterate through all the <pip>s
                    deco = new Decorator();
                    // <pip>s on the <card> are handled via the Decorator class
                //  deco.type = "pip";
                    deco.flip = (xPips[j].att("flip") == "1");
                    deco.loc.x = float.Parse(xPips[j].att("x"));
                    deco.loc.y = float.Parse(xPips[j].att("y"));
                    deco.loc.z = float.Parse(xPips[j].att("z"));
                    if (xPips[j].HasAtt("scale"))
                    {
                        deco.scale = float.Parse(xPips[j].att("scale"));
                    }
                   // cDef.pips.Add(deco);
                }
            }*/



            // all cards have a face attribute
            if (xCardDefs[i].HasAtt("face"))
            {
                cDef.face = xCardDefs[i].att("face");
            }
            cardDefs.Add(cDef);

            // power cards have special
            if (xCardDefs[i].HasAtt("special"))
            {
                cDef.face = xCardDefs[i].att("special");
            }
            cardDefs.Add(cDef);
        }
    }

    // Get the proper CardDefinition based on Rank (1 to 14 is Ace to King)
    public CardDefinition GetCardDefinitionByRank(int rnk)
    {
        // Search through all of the CardDefinitions
        foreach (CardDefinition cd in cardDefs)
        {
            // If the rank is correct, return this definition
            if (cd.rank == rnk)
            {
                print("testing getDefByRank card definifinition is correct for " + rnk);
                return (cd);
            }
        }
        return (null);
    }


    // Make the Card GameObjects
    public void MakeCards()
    {
        // cardNames will be the names of cards to build
        // Each suit goes from 1 to 14 (e.g., C1 to C14 for Clubs)
        cardNames = new List<string>();
        string[] _prefix = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "D", "P", "S" };
        string[] _suffix = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };

        //go through each prefix to add a suffix to it
        foreach (string p in _prefix)
        {
            // if p isnt 9
            if (p != "9")
            {
                //then 3 cards are made of each type unless its a 9
                for (int i = 0; i < 3; i++)
                {
                    cardNames.Add(p+_suffix[i]); //example  0a, 0b, 0c ...
                }
            }else //a 9 card is in place of p
                foreach (string s in _suffix)
                {
                    cardNames.Add(p + s); //goes through all the _suffix es
                }
        }

        // Make a list to hold all the cards
        cards = new List<Card>();

        // Iterate through all of the card names that were just made
        for (int i=0; i<cardNames.Count; i++)
        {
            // Make the card and add it to the cards Deck
            cards.Add(MakeCard(i));
        }
    }

    private Card MakeCard(int cardNum)
    {
        // Create a new Card GameObject with bg sprite
        GameObject cardObject = Instantiate(prefabCardBg) as GameObject;
        // Set the transform.parent of the new card to the anchor.
        cardObject.transform.parent = deckAnchor;
        Card card = cardObject.GetComponent<Card>(); // Get the Card Component

        // This line stacks the cards so that they're all in nice rows
        cardObject.transform.localPosition = new Vector3((cardNum % 13) * 3, cardNum / 13 * 4, 0);

        // Assign basic values to the Card
        card.name = cardNames[cardNum];
       // card.suit = card.name[0].ToString();
        card.rank = int.Parse(card.name.Substring(0)); //card rank is prefix
//        if(card.suit == "D" || card.suit == "H")
 //       {
  //          card.colS = "Red";
   //         card.color = Color.red;
    //    }
        // Pull the CardDefinition for this card
        card.def = GetCardDefinitionByRank(card.rank);

        AddDecorators(card);
    //    AddPips(card);
        AddFace(card);
        AddBack(card);

        return card;
    }

    // These private variables will be reused several times in helper methods
    private Sprite _tempSprite = null;
    private GameObject _tempObject = null;
    private SpriteRenderer _tempSpriteRenderer = null;

    private void AddDecorators(Card card)
    {
        // Add Decorators
        foreach(Decorator deco in decorators)
        { 
           
           // if(deco.type == "suit")
           // {
                // Instantiate a Sprite GameObject
                _tempObject = Instantiate(prefabSprite) as GameObject;
                // Get the SpriteRenderer Component
                _tempSpriteRenderer = _tempObject.GetComponent<SpriteRenderer>();
                // Set the Sprite to the proper suit
                _tempSpriteRenderer.sprite = rankSprites[card.rank];
           // }
           /**
            else
            {
                _tempObject = Instantiate(prefabSprite) as GameObject;
                _tempSpriteRenderer = _tempObject.GetComponent<SpriteRenderer>();
                // Get the proper Sprite to show this rank
                _tempSprite = rankSprites[card.rank];
                // Assign this rank Sprite to the SpriteRenderer
                _tempSpriteRenderer.sprite = _tempSprite;
                // Set the color of the rank to match the suit
                _tempSpriteRenderer.color = card.color;
            }
        
            
            Sprite tempSprite = rankSprites[card.rank];
            _tempObject = Instantiate(tempSprite) as GameObject;
            */
                

            //Make the deco Sprites render above the Card
    _tempSpriteRenderer.sortingOrder = 1;
            // Make the decorator Sprite a child of the Card
            _tempObject.transform.SetParent(card.transform);
            // Set the localPosition based on the location from DeckXML
            _tempObject.transform.localPosition = deco.loc;
            // Flip the decorator if needed
            if (deco.flip)
            {
                // An Euler rotation of 180 degrees around the Z-axis will flip it
                _tempObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            // Set the scale to keep decos from being too big
            if(deco.scale != 1)
            {
                _tempObject.transform.localScale = Vector3.one * deco.scale;
            }
            // Name this GameObject so it's easy to see
            _tempObject.name = deco.number;
            //Add this deco GameObject to the List card.decoGOs
            card.decoGOs.Add(_tempObject);
        }
    }
    /*
    private void AddPips(Card card)
    {
        // For each of the pips in the definition
        foreach(Decorator pip in card.def.pips)
        {
            // ...Instantiate a Sprite GameObject
            _tempObject = Instantiate(prefabSprite) as GameObject;
            // Set the parent to be the card GameObject
            _tempObject.transform.SetParent(card.transform);
            // Set the position to that specified in the XML
            _tempObject.transform.localPosition = pip.loc;
            // Flip it if necessary
            if (pip.flip)
            {
                _tempObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            // Scale it if necessary (only for the Ace)
            if(pip.scale != 1)
            {
                _tempObject.transform.localScale = Vector3.one * pip.scale;
            }
            // Give this GameObject a name
            _tempObject.name = "pip";
            // Get the SpriteRenderer Component
            _tempSpriteRenderer = _tempObject.GetComponent<SpriteRenderer>();
            // Set the Sprite to the proper suit
            _tempSpriteRenderer.sprite = dictSuits[card.suit];
            // Set sortingOrder so the pip is rendered above the Card_Front
            _tempSpriteRenderer.sortingOrder = 1;
            // Add this to the Card's list of pips
            card.pipGOs.Add(_tempObject);
        }
    }
    */
    private void AddFace(Card card)
    {
        if(card.def.face == "")
        {
            return; // No need to run this if it isn't a face card
        }

        _tempObject = Instantiate(prefabSprite) as GameObject;
        _tempSpriteRenderer = _tempObject.GetComponent<SpriteRenderer>();
        // Generate the right name and pass it to GetFace()
        _tempSprite = GetFace(card.def.face + card.suit);
        _tempSpriteRenderer.sprite = _tempSprite; // Assign this Sprite to _tSR
        _tempSpriteRenderer.sortingOrder = 1; // Set the sortingOrder
        _tempObject.transform.SetParent(card.transform);
        _tempObject.transform.localPosition = Vector3.zero;
        _tempObject.name = "face";
    }

    // Find the proper face card Sprite
    private Sprite GetFace(string faceS)
    {
        foreach(Sprite _tSP in faceSprites)
        {
            // If this SPrite has the right name...
            if(_tSP.name == faceS)
            {
                // ...then return the Sprite
                return (_tSP);
            }
        }
        // If nothing can be found, return null
        return (null);
    }

    private void AddBack(Card card)
    {
        // Add Card Back
        // The Card Back will be able to cover everything else on the Card
        _tempObject = Instantiate(prefabSprite) as GameObject;
        _tempSpriteRenderer = _tempObject.GetComponent<SpriteRenderer>();
        _tempSpriteRenderer.sprite = cardBack;
        _tempObject.transform.SetParent(card.transform);
        _tempObject.transform.localPosition = Vector3.zero;
        // This is a higher sortingOrder than anything else
        _tempSpriteRenderer.sortingOrder = 2;
        _tempObject.name = "back";
        card.back = _tempObject;
        // Default to face-up
        card.faceUp = startFaceUp; // Use the property faceUp of Card
    }

    static public void Shuffle(ref List<Card> oCards)
    {
        // Create a temporary List to hold the new shuffle order
        List<Card> tCards = new List<Card>();

        int ndx; // This will hold the index of the card to be moved
        tCards = new List<Card>(); // Initialize the temporary List
        // Repeat as long as there are cards in the original List
        while(oCards.Count > 0)
        {
            // Pick the index of a random card
            ndx = Random.Range(0, oCards.Count);
            // Add that card to the temporary List
            tCards.Add(oCards[ndx]);
            // And remove that card from the original List
            oCards.RemoveAt(ndx);
        }
        // Replace the original List with the temporary List
        oCards = tCards;
        // Because oCards is a reference (ref) parameter, the original argument
        // that was passed in is changed as well.
    }
}
