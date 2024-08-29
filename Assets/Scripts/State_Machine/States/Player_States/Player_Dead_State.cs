/// <summary>
/// Class To Handle The Player Dead State
/// </summary>
public class Player_Dead_State : Player_Base_State
{
    private const string dead_Animation_Name = "Death";

    public Player_Dead_State(Player_State_Machine playerSM, Player_Component player) : base(nameof(Player_Dead_State), playerSM, player) { }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayDeadAnimation();
    }

    /// <summary>
    /// Play The Dead Animation for the Player
    /// </summary>
    public void PlayDeadAnimation()
    {
        player.anim.Play(dead_Animation_Name);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public virtual void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}
