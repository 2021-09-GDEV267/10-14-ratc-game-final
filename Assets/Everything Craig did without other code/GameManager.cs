using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager S;
    [SerializeField] private Darc_PlayerRATC currentPlayer;
    [SerializeField] private Image currentDiscardCard;

    [Space(10)]
    [SerializeField] private Darc_DeckBuilderRATC deckBuilder;
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject dealCardScreen;
    [SerializeField] private GameObject singlePlayerView;
    [SerializeField] private GameObject allPlayerView;
    [SerializeField] private GameObject actionIndicator;
    [SerializeField] private GameObject transitionScreen;
    [SerializeField] private GameObject playersScreen;
    [SerializeField] private GameObject drawCardScreen;
    [SerializeField] private GameObject discardCardScreen;
    [SerializeField] private GameObject dealToPlayerScreen;
    [SerializeField] private float dealTime;
    private bool isFirstRound = true;
    [SerializeField] private bool isFinalRound = false;
    private bool isPassing = true;
    [SerializeField] private Text currentPlayerText;
    [SerializeField] private Text playerTurnText;
    [SerializeField] private Text actionText;
    [SerializeField] private Text[] playersNameTexts;
    [SerializeField] private Text[] playerScoreTexts;
    [SerializeField] private Text[] extraTexts;
    [SerializeField] private Darc_PlayerRATC[] players = new Darc_PlayerRATC[4];
    [SerializeField] private GameObject actionPanel;
    [SerializeField] private GameState currentGameState = GameState.None;
    [SerializeField] private PickState currentPickState = PickState.None;
    [SerializeField] private ScreenState currentScreenState = ScreenState.None;
    [SerializeField] private float[] actionIndicatorLocations;
    [SerializeField] private ActionState currentActionState = ActionState.None;
    [SerializeField] private Image singleSlot1, singleSlot2, singleSlot3, singleSlot4;
    [SerializeField] private Image[] multiViewCards;
    [SerializeField] private Image drawCard, discardCard;
    [SerializeField] private Sprite catCardBack;
    [SerializeField] private List<Darc_CardRATC> allGameCards;
    [SerializeField] private List<Darc_CardRATC> discardPile, drawPile = new List<Darc_CardRATC>();
    [SerializeField] private Color winPanelColor;
    [SerializeField] private Darc_CardRATC cardSelected_1;
    [SerializeField] private Darc_CardRATC cardSelected_2;
    private Vector2[] finalIndicatorLocations;
    private bool confirmationValue;
    private bool hasPickedPlayer = false;
    bool hasPickedCard = false;
    [SerializeField] int turns = 0;

    private void Awake() => S = this;

    private void Start()
    {
        deckBuilder.BuildCards();
        allGameCards = deckBuilder.allCards;
        finalIndicatorLocations = new Vector2[actionIndicatorLocations.Length];
        for (int i = 0; i < actionIndicatorLocations.Length; i++)
            finalIndicatorLocations[i] = new Vector2(actionIndicator.transform.localPosition.x, actionIndicatorLocations[i]);
        confirmationPanel.SetActive(false);
        dealCardScreen.SetActive(false);
        singlePlayerView.SetActive(false);
        allPlayerView.SetActive(false);
        actionIndicator.SetActive(false);
        transitionScreen.SetActive(false);
        playersScreen.SetActive(false);
        drawCardScreen.SetActive(false);
        discardCardScreen.SetActive(false);
        singleSlot2.sprite = catCardBack;
        singleSlot3.sprite = catCardBack;
        foreach (var c in multiViewCards)
            c.sprite = catCardBack;
        Darc_CardRATC[] cards = new Darc_CardRATC[allGameCards.Count];
        allGameCards.CopyTo(cards);
        foreach (var p in players)
        {
            p.slot1 = GetRandomStartingCard(cards);
            p.slot2 = GetRandomStartingCard(cards);
            p.slot3 = GetRandomStartingCard(cards);
            p.slot4 = GetRandomStartingCard(cards);
        }
        discardPile.Add(GetRandomStartingCard(cards));
        foreach (var c in cards)
        {
            if (c != null)
                drawPile.Add(c);
        }
        for (int i = 0; i < players.Length; i++)
            playersNameTexts[i].text = players[i].playerName;
    }

    Darc_CardRATC GetRandomStartingCard(Darc_CardRATC[] cards)
    {
        int ranIndex = Random.Range(0, cards.Length - 1);
        int tries = 0;
        if (cards[ranIndex] != null)
        {
            Darc_CardRATC pickedCard = cards[ranIndex];
            cards[ranIndex] = null;
            return pickedCard;
        }
        else if (cards[ranIndex] == null && tries < 101)
        {
            tries++;
            return GetRandomStartingCard(cards);
        }
        return null;
    }

    public enum GameState
    {
        None,
        Setup,
        Playing,
        Result
    }

    public enum PickState
    {
        None,
        Draw,
        Discard
    }

    public enum ScreenState
    {
        None,
        Passing,
        SingleView,
        MultiView,
    }

    public enum ActionState
    {
        None,
        DrawTwo,
        Swap,
        Peek,
        Discard,
        RatATatCat
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.Setup:
                ShowDeal();
                if (!hasPickedPlayer)
                    dealToPlayerScreen.SetActive(true);
                else if (hasPickedPlayer)
                {
                    if (confirmationValue)
                    {
                        confirmationValue = false;
                        confirmationPanel.SetActive(false);
                        currentScreenState = ScreenState.Passing;
                        currentGameState = GameState.Playing;
                    }
                }
                break;

            case GameState.Playing:
                dealToPlayerScreen.SetActive(false);
                switch (currentScreenState)
                {
                    case ScreenState.None:
                        break;
                    case ScreenState.Passing:
                        ShowTransitionScreen();
                        if (currentPlayer != null)
                            currentPlayerText.text = $"Pass To Player: {currentPlayer.playerNumber}";
                        if (Mouse.current.leftButton.isPressed)
                            if (!confirmationValue)
                                ShowConfirmation();
                        if (confirmationValue)
                        {
                            confirmationPanel.SetActive(false);
                            confirmationValue = false;
                            currentScreenState = ScreenState.SingleView;
                        }
                        break;

                    case ScreenState.SingleView:
                        isFirstRound = turns < 4;
                        if (currentPlayer != null)
                        {
                            playerTurnText.text = $"Player {currentPlayer.playerNumber} Turn!";
                            if (isFirstRound)
                            {
                                singleSlot1.sprite = currentPlayer.slot1.cardDefinition.cardImage;
                                singleSlot4.sprite = currentPlayer.slot4.cardDefinition.cardImage;
                            }
                            else
                            {
                                singleSlot1.sprite = catCardBack;
                                singleSlot4.sprite = catCardBack;
                            }
                            if (discardPile.Count > 0)
                                currentDiscardCard.sprite = discardPile[0].cardDefinition.cardImage;
                            if (hasPickedCard)
                            {
                                switch (currentPickState)
                                {
                                    case PickState.None:
                                        break;
                                    case PickState.Draw:
                                        discardCardScreen.SetActive(false);
                                        drawCardScreen.SetActive(true);
                                        break;
                                    case PickState.Discard:
                                        drawCardScreen.SetActive(false);
                                        discardCardScreen.SetActive(true);
                                        break;
                                }
                            }
                            switch (currentActionState)
                            {
                                case ActionState.None:
                                    if (isFirstRound)
                                        playerTurnText.text = $"Player {currentPlayer.playerNumber} FIRST turn!";
                                    if (isFinalRound)
                                        playerTurnText.text = $"Player {currentPlayer.playerNumber} FINAL turn!";
                                    ShowSingleView();
                                    break;
                                case ActionState.Swap:
                                    playerTurnText.text = $"Player {currentPlayer.playerNumber} {currentActionState}";
                                    break;
                                case ActionState.Peek:
                                    playerTurnText.text = $"Player {currentPlayer.playerNumber} {currentActionState}";
                                    ShowSingleView();
                                    break;
                                case ActionState.DrawTwo:
                                    playerTurnText.text = $"Player {currentPlayer.playerNumber} {currentActionState}";
                                    break;

                                case ActionState.Discard:
                                    playerTurnText.text = $"Player {currentPlayer.playerNumber} {currentActionState}";
                                    break;

                                case ActionState.RatATatCat:
                                    isFinalRound = true;
                                    break;
                            }
                        }
                        break;

                    case ScreenState.MultiView:
                        drawCardScreen.SetActive(false);
                        discardCardScreen.SetActive(false);
                        ShowMultiView();
                        if (currentPlayer != null)
                            actionIndicator.transform.localPosition = finalIndicatorLocations[currentPlayer.playerNumber - 1];
                        ShowActionIndicator();
                        ShowActionText();
                        break;
                }
                if (cardSelected_1 == null && cardSelected_2 == null)
                    currentActionState = ActionState.None;
                if (cardSelected_1 != null)
                {
                    if (cardSelected_1.cardDefinition.isPowerCard)
                    {
                        Darc_CardDefinition.PowerType pType = cardSelected_1.cardDefinition.powerType;
                        switch (pType)
                        {
                            case Darc_CardDefinition.PowerType.DrawTwo:
                                currentActionState = ActionState.DrawTwo;
                                break;
                            case Darc_CardDefinition.PowerType.Peek:
                                currentActionState = ActionState.Peek;
                                break;
                            case Darc_CardDefinition.PowerType.Swap:
                                currentActionState = ActionState.Swap;
                                break;
                        }
                    }
                }
                break;

            case GameState.Result:
                int currentLowestScore = Mathf.Min(players[0].score.value, players[1].score.value, players[2].score.value, players[3].score.value);
                for (int i = 0; i < players.Length; i++)
                {
                    playerScoreTexts[i].text = players[i].score.value.ToString();
                    extraTexts[i].text = "Score:";
                    if (players[i].score.value == currentLowestScore)
                        players[i].panel.color = winPanelColor;
                    else
                        players[i].panel.color = Color.white;
                        
                }
                break;
        }
    }

    public void PickRandomPlayerForStart()
    {
        currentPlayer = players[Random.Range(0, players.Length - 1)];
        hasPickedPlayer = true;
    }

    public void PickPlayerForStart(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                currentPlayer = players[0];
                break;

            case 2:
                currentPlayer = players[1];
                break;

            case 3:
                currentPlayer = players[2];
                break;

            case 4:
                currentPlayer = players[3];
                break;
        }
        hasPickedPlayer = true;
    }

    public void ClickedSlot()
    {

    }

    public void PickFromDiscardPile()
    {
        if (!hasPickedCard)
        {
            if (cardSelected_1 != null && cardSelected_2 != null) return;
            if (cardSelected_1 == null)
                cardSelected_1 = discardPile[0];
            else
                cardSelected_2 = discardPile[0];
            currentPickState = PickState.Discard;
            discardCard.sprite = discardPile[0].cardDefinition.cardImage;
            hasPickedCard = true;
            Debug.Log($"Picked from the discard pile");
        }
    }

    public void PickFromDrawPile()
    {
        if (hasPickedCard) return;
        if (drawPile.Count <= 0) return;
        if (drawCardScreen.activeSelf) return;
        Darc_CardRATC cardPicked = drawPile[Random.Range(0, drawPile.Count - 1)];
        drawCard.sprite = cardPicked.cardDefinition.cardImage;
        currentPickState = PickState.Draw;
        hasPickedCard = true;
        drawPile.Remove(cardPicked);
        cardSelected_1 = cardPicked;
        Debug.Log($"Picked from the draw pile");
    }

    [ContextMenu("Show Single View")]
    private void ShowSingleView()
    {
        SingleView();
        HideActionIndicator();
        HideActionText();
    }

    public void ChangeView()
    {
        if (currentScreenState == ScreenState.SingleView)
            currentScreenState = ScreenState.MultiView;
        else
            currentScreenState = ScreenState.SingleView;
    }

    public void ShowConfirmation() => confirmationPanel.SetActive(true);

    public void CheckConfirmation(bool buttonPressed)
    {
        if (buttonPressed.Equals(false))
        {
            confirmationPanel.SetActive(false);
            confirmationValue = false;
            return;
        }
        confirmationValue = true;
    }

    public bool IsPassing() => isPassing;

    [ContextMenu("Start New Game")]
    public void StartNewGame()
    {
        currentGameState = GameState.Setup;
        currentActionState = ActionState.None;
    }

    private void ShowDeal()
    {
        allPlayerView.SetActive(false);
        actionIndicator.SetActive(false);
        singlePlayerView.SetActive(false);
        transitionScreen.SetActive(false);
        playersScreen.SetActive(false);
        dealCardScreen.SetActive(true);
    }

    private void ShowActionText()
    {
        if (currentPlayer != null)
            actionText.text = $"Player {currentPlayer.playerNumber} {currentActionState}";
        actionPanel.SetActive(true);
        actionText.gameObject.SetActive(true);
    }

    private void HideActionText()
    {
        actionPanel.SetActive(false);
        actionText.gameObject.SetActive(false);
    }

    [ContextMenu("Show Action Indicator")]
    private void ShowActionIndicator()
    {
        if (!allPlayerView.activeSelf) return;
        actionIndicator.SetActive(true);
    }

    [ContextMenu("Hide Action Indicator")]
    private void HideActionIndicator() => actionIndicator.SetActive(false);

    [ContextMenu("Show Transition Screen")]
    private void ShowTransitionScreen()
    {
        allPlayerView.SetActive(false);
        actionIndicator.SetActive(false);
        singlePlayerView.SetActive(false);
        dealCardScreen.SetActive(false);
        playersScreen.SetActive(false);
        transitionScreen.SetActive(true);
    }

    private void SingleView()
    {
        allPlayerView.SetActive(false);
        dealCardScreen.SetActive(false);
        actionIndicator.SetActive(false);
        transitionScreen.SetActive(false);
        playersScreen.SetActive(false);
        singlePlayerView.SetActive(true);
    }

    [ContextMenu("Show All View")]
    private void ShowMultiView()
    {
        dealCardScreen.SetActive(false);
        actionIndicator.SetActive(false);
        transitionScreen.SetActive(false);
        singlePlayerView.SetActive(false);
        allPlayerView.SetActive(true);
        playersScreen.SetActive(true);
    }
}