public abstract class PlayerBaseState 
{  protected PlayerStateManager context;
   protected PlayerStateFactory factory;

   public PlayerBaseState(PlayerStateManager currentContext, PlayerStateFactory playerFactory)
   {
      context = currentContext;
      factory = playerFactory;
   }
   public abstract void EnterState();
   public abstract void UpdateState();
   public abstract void ExitState();
   public abstract void CheckSwitchStates();

   void UpdateStates(){}
   public void SwitchState(PlayerBaseState newState)
   {
      ExitState();
      newState.EnterState();
      context.CurrentState = newState;
   }

}
