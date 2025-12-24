public class PlayerStateFactory
{
    PlayerStateManager context;

    public PlayerStateFactory(PlayerStateManager currentContext)
    {
        context = currentContext;
    }
    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(context, this);
    }
    public PlayerBaseState Run()
    {
        return new PlayerRunState(context, this);
    }

    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(context, this);
    }

    public PlayerBaseState Slash()
    {
        return new PlayerSlashState(context, this);
    }

    public PlayerBaseState Hurt()
    {
        return new PlayerHurtState(context, this);
    }

}
