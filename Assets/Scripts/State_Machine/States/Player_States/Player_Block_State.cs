using UnityEngine;

/// <summary>
/// Class To Handle The Player Block State
/// </summary>
public class Player_Block_State : Player_Base_State
{
    private const string block_Animation_Name = "Blocking";
    private bool blocking;
    public Player_Block_State(Player_State_Machine playerSM, Player_Component player) : base(nameof(Player_Block_State), playerSM, player) { }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayBlockAnimation();

        blocking = true;

        player.input.OnPlayerBlock += Input_OnPlayerBlock;
        player.input.OnPlayerAttack += Input_OnPlayerAttack;
        player.input.OnPlayerMove += Input_OnPlayerMove;
        player.input.OnPlayerJump += Input_OnPlayerJump;
    }

    private void Input_OnPlayerBlock(bool obj)
    {
        blocking = obj;
    }

    private void Input_OnPlayerJump(bool obj)
    {
        if (!blocking)
            base.state_Machine.SetState(base.transitions[nameof(Player_Jump_State)]);
    }

    private void Input_OnPlayerMove(Vector2 obj)
    {
        if (!blocking)
        {
            player.movement = new Vector3(obj.x, 0f, obj.y).normalized;
            base.state_Machine.SetState(base.transitions[nameof(Player_Movement_State)]);
        }
    }

    private void Input_OnPlayerAttack(bool obj)
    {
        if (!blocking)
            base.state_Machine.SetState(base.transitions[nameof(Player_Attack_State)]);
    }

    public override void UpdateLogic()
    {
        if (!blocking)
        {
            base.state_Machine.SetState(base.transitions[nameof(Player_Idle_State)]);
        }
    }

    public override void UpdatePhysics()
    {

    }

    /// <summary>
    /// Play The Block Animation for the Player
    /// </summary>
    public void PlayBlockAnimation()
    {
        player.anim.SetBool(block_Animation_Name, blocking);
    }

    public override void OnExit()
    {
        base.OnExit();
        blocking = false;
        PlayBlockAnimation();
        player.input.OnPlayerAttack -= Input_OnPlayerAttack;
        player.input.OnPlayerMove -= Input_OnPlayerMove;
        player.input.OnPlayerJump -= Input_OnPlayerJump;
    }

    public virtual void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}
