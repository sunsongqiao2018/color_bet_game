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

    public static StateMachine Instance;
    public BaseGameState _currentState { private get; set; }
    GameStateFactory _stateFactory;

    //Public Fields
    public int playerBetAmount, playerChipStock;
    public bool gameResult { private get; set; }
    public bool playerBet { private get; set; }
    public event EventHandler GameFinished;
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
        playerBet = true;
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
        //play start
        if (_currentState.GetType() == typeof(IdleGameState))
        {
            _currentState.SwitchStates(_stateFactory.Play());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bet"></param>
    /// <param name="betAmount"></param>
    public void SetPlayGameInitData(bool bet, int betAmount)
    {
        playerBet = bet;
        playerBetAmount = betAmount;
        playerChipStock -= playerBetAmount;
    }

    internal void PlayCard()
    {
        StartCoroutine(WaitCardAnimation());
    }
    public void OnBroadcastingInfo()
    {
        BroadcastPlayerInfo?.Invoke(this, new PlayerInfoEventArgs(playerBet, playerChipStock));
    }
    private IEnumerator WaitCardAnimation()
    {
        BroadcastResult.Invoke(this, new BoolEventArgs(gameResult));
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
        return playerBet == gameResult;
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
