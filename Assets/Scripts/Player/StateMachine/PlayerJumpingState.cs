using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public PlayerJumpingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
       : base(currentContext, playerStateFactory) { }

    public override void CheckSwitchStates()
    {
        if (ctx.CapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Debug.Log("Touching the ground");
            SwitchState(factory.Grounded());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Jumping");
        Jump();
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
    }


    public void Jump()
    {
        
        ctx.JumpPressedRemember = ctx.JumpPressedRememberTime;

        ctx.BonusJumpsLeft = ctx.StartJumpsAmount;
        

        //Cut Jump height when Jump wans't pressed completely
/*        if (Input.GetButtonUp("Jump"))
        {
            if (ctx.Rb.velocity.y > 0)
            {
                ctx.Rb.velocity = new Vector2(ctx.Rb.velocity.x, ctx.Rb.velocity.y * ctx.CutJumpForce);
            }

        }*/
        //If time since Jump button was pressed is bigger then 0 still do the jump
        //This creates Double Jump NEED TO FIX!!
        //JUMP
        if ((ctx.JumpPressedRemember > 0) && (ctx.WasOnTheGroundRemember > 0))
        {
            ctx.JumpPressedRemember = 0;
            ctx.WasOnTheGroundRemember = 0;
            ctx.Animator.SetBool("isRunning", false);
            ctx.Animator.SetBool("isJumping", true);
            ctx.Rb.velocity += new Vector2(ctx.Rb.velocity.x, ctx.JumpForce);
        }
        //Double Jump
        //LOOKS LIKE OVERRIDING MB FIX LATER ?
/*        else if (Input.GetButtonDown("Jump") && ctx.BonusJumpsLeft > 0)
        {
            ctx.BonusJumpsLeft--;
            //ctx.Animator.SetBool("isJumping", false);
            //ctx.Animator.SetBool("isDoubleJumping", true);
            ctx.Rb.velocity += new Vector2(ctx.Rb.velocity.x, ctx.JumpForce);
        }


        else { return; }*/
    }

    /* //Very simple checker, needs to be enhanced
     public void AnimateJump()
     {
         if (!ctx.CapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) &&
             !ctx.CapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
         {
             ctx.Animator.SetBool("isRunning", false);
             ctx.Animator.SetBool("isJumping", true);
         }
         else
         {
             ctx.Animator.SetBool("isJumping", false);
             ctx.Animator.SetBool("isDoubleJumping", false);
         }
     }*/

}
