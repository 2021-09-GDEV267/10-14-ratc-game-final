using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager S;
    [SerializeField] Darc_GameRATC Game;
    [SerializeField] GameObject confirmationPanel;
    [SerializeField] GameObject dealCardScreen;
    [SerializeField] GameObject singlePlayerView;
    [SerializeField] GameObject allPlayerView;
    [SerializeField] GameObject actionIndicator;
    [SerializeField] GameObject transitionScreen;
    [SerializeField] GameObject playersScreen;
    [SerializeField] float dealTime;
    bool isFirstRound = true;
    bool isFinalRound = false;
    bool isPassing = true;
    [SerializeField] Darc_PlayerRATC currentPlayer;
    [SerializeField] Text currentPlayerText;
    [SerializeField] Text playerTurnText;
    [SerializeField] Text actionText;
    [SerializeField] GameObject actionPanel;
    [SerializeField] GameState currentGameState;
    [SerializeField] ScreenState currentScreenState;
    [SerializeField] float[] actionIndicatorLocations;
    [SerializeField] ActionState currentActionState;
    Vector2[] finalIndicatorLocations;

    void Awake() => S = this;

    void Start() {
        finalIndicatorLocations = new Vector2[actionIndicatorLocations.Length];
        for (int i = 0; i < actionIndicatorLocations.Length; i++)
            finalIndicatorLocations[i] = new Vector2(actionIndicator.transform.localPosition.x, actionIndicatorLocations[i]);
    }

    public enum GameState
    {
        None,
        Setup,
        Playing,
        Result
    }

    public enum ScreenState
    {
        Passing,
        SingleView,
        MultiView,
    }

    public enum ActionState
    {
        None,
        Swap,
        Peek,
        Discard,
        RatATatCat
    }

    void Update()
    {
        switch (currentGameState)
        {
            case GameState.Setup:
                StartCoroutine(ShowDeal());
                break;
            case GameState.Playing:
                switch (currentScreenState)
                {
                    case ScreenState.Passing:
                        ShowTransitionScreen();
                        if (currentPlayer != null)
                            currentPlayerText.text = $"Pass To Player: {currentPlayer.playerNumber}";
                        if (Input.GetMouseButtonDown(0))
                            currentScreenState = ScreenState.SingleView;
                        break;
                    case ScreenState.SingleView:
                        if (currentPlayer != null)
                            playerTurnText.text = $"Player {currentPlayer.playerNumber} Turn!";
                            ShowSingleView();
                        break;
                    case ScreenState.MultiView:
                        ShowMultiView();
                        if (currentPlayer != null)
                            actionIndicator.transform.localPosition = finalIndicatorLocations[currentPlayer.playerNumber - 1];
                        ShowActionIndicator();
                        ShowActionText();
                        break;
                }
                switch (currentActionState)
                {
                    case ActionState.Swap:
                        break;
                    case ActionState.Peek:
                        playerTurnText.text = $"Player {currentPlayer.playerNumber} {currentActionState}";
                        ShowSingleView();
                        break;
                    case ActionState.Discard:
                        break;
                    case ActionState.RatATatCat:
                        isFinalRound = true;
                        break;
                }
                break;
            case GameState.Result:
                break;
        }
    }

    [ContextMenu("Show Single View")]
    void ShowSingleView()
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

    public bool IsPassing() => isPassing;

    [ContextMenu("Start New Game")]
    public void StartNewGame()
    {
        currentGameState = GameState.Setup;
        currentActionState = ActionState.None;
    }

    [ContextMenu("Show Deal")]
    IEnumerator ShowDeal()
    {
        allPlayerView.SetActive(false);
        actionIndicator.SetActive(false);
        singlePlayerView.SetActive(false);
        transitionScreen.SetActive(false);
        playersScreen.SetActive(false);
        dealCardScreen.SetActive(true);
        yield return new WaitForSeconds(dealTime);
        currentGameState = GameState.Playing;
    }
    
    void ShowActionText()
    {
        if (currentPlayer != null)
            actionText.text = $"Player {currentPlayer.playerNumber} {currentActionState}";
        actionPanel.SetActive(true);         
        actionText.gameObject.SetActive(true);   
    }

    void HideActionText()
    {
        actionPanel.SetActive(false);
        actionText.gameObject.SetActive(false);
    }

    [ContextMenu("Show Action Indicator")]
    void ShowActionIndicator()
    {
        if (!allPlayerView.activeSelf) return;
        actionIndicator.SetActive(true);
    }
    [ContextMenu("Hide Action Indicator")]
    void HideActionIndicator() => actionIndicator.SetActive(false);

    [ContextMenu("Show Transition Screen")]
    void ShowTransitionScreen()
    {
        allPlayerView.SetActive(false);
        actionIndicator.SetActive(false);
        singlePlayerView.SetActive(false);
        dealCardScreen.SetActive(false);
        playersScreen.SetActive(false);
        transitionScreen.SetActive(true);
    }

    void SingleView()
    {
        allPlayerView.SetActive(false);
        dealCardScreen.SetActive(false);
        actionIndicator.SetActive(false);
        transitionScreen.SetActive(false);
        playersScreen.SetActive(false);
        singlePlayerView.SetActive(true);
    }

    [ContextMenu("Show All View")]
    void ShowMultiView()
    {
        dealCardScreen.SetActive(false);
        actionIndicator.SetActive(false);
        transitionScreen.SetActive(false);
        singlePlayerView.SetActive(false);
        allPlayerView.SetActive(true);
        playersScreen.SetActive(true);
    }
}
