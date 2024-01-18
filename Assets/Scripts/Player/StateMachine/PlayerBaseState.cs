public abstract class PlayerBaseState 
{
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) {
        ctx = currentContext;
        factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();
     
    void UpdateStates() { }

    protected void SwitchState(PlayerBaseState newState) {
        //current state exits state
        ExitState();

        //new state enters state
        newState.EnterState();

        //switch current state of context
        ctx.CurrentSate = newState;

    }

    protected void SetSuperSate() { }

    protected void SetSubState() { }

}
