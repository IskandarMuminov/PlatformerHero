using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        :base (currentContext, playerStateFactory) { }


    public override void CheckSwitchStates()
    {
        if (ctx.CapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && (Input.GetButtonDown("Jump"))) {
            SwitchState(factory.Jump());
        }
    }

    public override void EnterState()
    {
        
        Debug.Log("Grounded");

    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
       
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        ctx.WasOnTheGroundRemember = ctx.WasOnTheGroundRememberTime;

    }
}
