using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// hierarchical state machine
/// </summary>
public class StateMachine : MonoBehaviour
{
    private const int defaultStartChip = 100;
    GameStateFactory _stateFactory;


    public static StateMachine Instance;
    //Public Fields
    public BaseGameState _currentState { private get; set; }
    public int playerBetAmount, playerChipStock;
    public bool GameResult { private get; set; }
    public bool PlayersBet { private get; set; }

    public bool ResultReceived { private get; set; }
    public event EventHandler GameFinished, GameStarted;
    public event EventHandler<BoolEventArgs> BroadcastResult, CardRevealed;
    public event EventHandler<PlayerInfoEventArgs> BroadcastPlayerInfo;
    //Private Fields


    [SerializeField] Animator cardAnim;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //load up satates
        _stateFactory = new GameStateFactory();
        //set currentState to be Idle
        _currentState = _stateFactory.Idle();
        _currentState.EnterState();
        playerChipStock = defaultStartChip;
        PlayersBet = true;
    }

    internal void CompareResult()
    {
        if (PlayerWon())
        {
            int multiplier = 2;
            playerChipStock += playerBetAmount * multiplier;
            Debug.Log("Player Win:" + playerBetAmount * multiplier);
        }
        else
        {
            //restock if empty
            if (playerChipStock == 0) playerChipStock = defaultStartChip;
            Debug.Log("Player Lose:" + playerChipStock);

        }
        StartCoroutine(WaitResultAnimation());
    }

    public void OnPlayGame()
    {

        if (_currentState.GetType() == typeof(IdleGameState))
        {
            //starts game if in idle state
            ResultReceived = false;
            _currentState.SwitchStates(_stateFactory.Play());
            GameStarted?.Invoke(this, new EventArgs());
        }
    }


    /// <summary>
    /// set player bet and calculates remaining total chips
    /// </summary>
    /// <param name="bet"></param>
    public void SetPlayGameInitData(bool bet)
    {
        SetPlayerBet(bet);
        playerChipStock -= playerBetAmount;
    }
    public void SetPlayerBet(bool bet)
    {
        PlayersBet = bet;
    }
    public void SetPlayerBetAmount(int betAmount)
    {
        playerBetAmount = betAmount;
    }
    internal void PlayCard()
    {
        StartCoroutine(WaitCardAnimation());
    }
    /// <summary>
    /// broadcast all player hud info to other player.
    /// </summary>
    public void CallBroadcastPlayerInfo()
    {
        BroadcastPlayerInfo?.Invoke(this, new PlayerInfoEventArgs(PlayersBet, playerChipStock, playerBetAmount));
    }
    private IEnumerator WaitCardAnimation()
    {
        Debug.LogWarning("BroadCasting Play Result" + (GameResult ? "true" : "false"));
        yield return new WaitUntil(() => ResultReceived);
        BroadcastResult.Invoke(this, new BoolEventArgs(GameResult));
        yield return new WaitForSeconds(.5f);
        _currentState.SwitchStates(_stateFactory.Result());
    }
    private IEnumerator WaitResultAnimation()
    {
        //reveal card
        cardAnim.SetBool("InPlay", true);
        yield return new WaitForSeconds(2f);
        CardRevealed.Invoke(this, new BoolEventArgs(PlayerWon()));
        //result pop up
        cardAnim.SetBool("InPlay", false);
        yield return new WaitForSeconds(2f);
        _currentState.SwitchStates(_stateFactory.Idle());
    }

    private bool PlayerWon()
    {
        return PlayersBet == GameResult;
    }

    internal void PlayEndCleanup()
    {
        GameFinished.Invoke(this, new EventArgs());
    }
    private void Update()
    {
        _currentState.UpdateState();
    }
}
