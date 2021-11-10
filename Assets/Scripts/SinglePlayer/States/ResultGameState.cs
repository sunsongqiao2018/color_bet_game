using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// from the frame after show card result to all game info updated.
/// </summary>
public class ResultGameState : BaseGameState
{
    public ResultGameState(StateMachine machine, GameStateFactory factory) : base(machine, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter Result state");
        sMachine.CompareResult();
    }

    public override void ExitState()
    {
        sMachine.PlayEndCleanup();
        Debug.Log("Exit Result state");

    }

    public override void UpdateState()
    {
    }
}
