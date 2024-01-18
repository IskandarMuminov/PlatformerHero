public class PlayerStateFactory 
{
    PlayerStateMachine context;
    public PlayerStateFactory(PlayerStateMachine currentContext) { 
        context = currentContext;
    }

    public PlayerBaseState Idle() {
        return new PlayerIdlingState(context, this);
    }
    public PlayerBaseState Run() {
        return new PlayerRunningState(context, this);
    }
    public PlayerBaseState Jump() {
        return new PlayerJumpingState(context, this);
    }
    public PlayerBaseState Climb() { 
        return new PlayerClimbingState(context, this);
    }
    public PlayerBaseState Grounded() {
        return new PlayerGroundedState(context, this);
    }


}
