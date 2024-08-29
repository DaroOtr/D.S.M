using System;
using UnityEngine;

/// <summary>
/// Class To Handle The Player Idle State
/// </summary>
public class Player_Idle_State : Player_Base_State
{
    private const string idle_Animation_Name = "Idle";

    public Player_Idle_State(Player_State_Machine playerSM, Player_Component player) : base(nameof(Player_Idle_State), playerSM, player) { }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayIdleAnimation();

        player.movement = Vector3.zero;
        player.input.OnPlayerMove += OnPlayerMove;
        player.input.OnPlayerJump += Input_OnPlayerJump;
        player.input.OnPlayerAttack += Input_OnPlayerAttack;
        player.input.OnPlayerBlock += Input_OnPlayerBlock;
        player.character_Health_Component.OnDecrease_Health += Character_Health_Component_OnDecrease_Health;
        player.character_Health_Component.OnInsufficient_Health += Character_Health_Component_OnInsufficient_Health;
    }

    private void Character_Health_Component_OnInsufficient_Health()
    {
        base.state_Machine.SetState(base.transitions[nameof(Player_Dead_State)]);
    }

    private void Character_Health_Component_OnDecrease_Health()
    {
        base.state_Machine.SetState(base.transitions[nameof(Player_Hit_State)]);
    }

    private void Input_OnPlayerBlock(bool obj)
    {
        base.state_Machine.SetState(base.transitions[nameof(Player_Block_State)]);
    }

    private void Input_OnPlayerAttack(bool obj)
    {
        base.state_Machine.SetState(base.transitions[nameof(Player_Attack_State)]);
    }

    private void OnPlayerMove(Vector2 newMovement)
    {
        player.movement = new Vector3(newMovement.x, 0f, newMovement.y).normalized;
        base.state_Machine.SetState(base.transitions[nameof(Player_Movement_State)]);
    }

    private void Input_OnPlayerJump(bool obj)
    {
        base.state_Machine.SetState(base.transitions[nameof(Player_Jump_State)]);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //float targetAngle = Mathf.Atan2(player.movement.x, player.movement.z) * Mathf.Rad2Deg + player.camera.eulerAngles.y;
        //
        //player.lastAngle = targetAngle;
        //
        //float angle = Mathf.SmoothDampAngle(player.GetComponent<Transform>().eulerAngles.y, targetAngle, ref player.turn_Smooth_Velocity, player.turnSmoothTime);
        //
        //player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, angle, 0f);

        PlayIdleAnimation();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    /// <summary>
    /// Play The Idle Animation For The Player
    /// </summary>
    public void PlayIdleAnimation() 
    {
        player.anim.Play(idle_Animation_Name);
    }

    public override void OnExit()
    {
        base.OnExit();
        player.input.OnPlayerMove -= OnPlayerMove;
        player.input.OnPlayerJump -= Input_OnPlayerJump;
        player.input.OnPlayerAttack -= Input_OnPlayerAttack;
        player.input.OnPlayerBlock -= Input_OnPlayerBlock;
        player.character_Health_Component.OnDecrease_Health -= Character_Health_Component_OnDecrease_Health;
        player.character_Health_Component.OnInsufficient_Health -= Character_Health_Component_OnInsufficient_Health;
    }

    public override void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}
