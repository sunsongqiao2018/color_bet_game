using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Play state includes the range from the frame after player click on ready button till the frame after show the result.
/// </summary>
public class PlayGameState : BaseGameState
{
    public PlayGameState(StateMachine machine, GameStateFactory factory) : base(machine, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entered play state");

        sMachine.gameResult = FlipCoin();
        sMachine.PlayCard();
    }

    //move to result state
    public override void ExitState()
    {
        Debug.Log("Exit play state");
    }
    public override void UpdateState()
    {
        Debug.Log("Play State");
    }
    private bool FlipCoin()
    {
        int coin = Random.Range(0, 2);
        return coin == 0;
    }
}
