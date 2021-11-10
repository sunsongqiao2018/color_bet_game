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

    //Private Fields

    [SerializeField] Animator cardAnim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //set up states
        _stateFactory = new GameStateFactory();
        //set currentState to be Idle
        _currentState = _stateFactory.Idle();
        _currentState.EnterState();
        playerChipStock = defaultStartChip;
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

            cardAnim.SetBool("InPlay", true);
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

    private IEnumerator WaitCardAnimation()
    {
        //do some animation
        yield return new WaitForSeconds(1f);

        cardAnim.SetInteger("Result", gameResult ? 0 : 1);
        _currentState.SwitchStates(_stateFactory.Result());
    }
    private IEnumerator WaitResultAnimation()
    {
        //do some animation
        yield return new WaitForSeconds(1f);

        cardAnim.SetInteger("Result", -1);
        cardAnim.SetBool("InPlay", false);

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
    // Update is called once per frame
    void Update()
    {
        // _currentState.UpdateState();
    }
}
