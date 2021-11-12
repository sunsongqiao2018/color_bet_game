using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Idle states ranges from previous game result shown to before player clicked Ready button.
/// </summary>
public class IdleGameState : BaseGameState
{
    public IdleGameState(StateMachine machine, GameStateFactory factory) : base(machine, factory) { }

    public override void EnterState()
    {
        Debug.Log("Enter Idle");
    }

    public override void ExitState()
    {
        Debug.Log("Exit Idle");
    }

    public override void UpdateState()
    {
        sMachine.OnBroadcastingInfo();
       // Debug.Log("you are in idle state");
    }
}
