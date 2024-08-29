using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class To Handle The Player Movement State
/// </summary>
public class Player_Movement_State : Player_Base_State
{
    private const string movement_Animation_Name = "VelocityX/Z";

    public Player_Movement_State(Player_State_Machine playerSM, Player_Component player) : base(
        nameof(Player_Movement_State), playerSM, player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.input.OnPlayerMove += Player_Input_OnPlayerMove;
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

    private void Input_OnPlayerJump(bool obj)
    {
        base.state_Machine.SetState(base.transitions[nameof(Player_Jump_State)]);
    }

    private void Player_Input_OnPlayerMove(Vector2 newMovement)
    {
        player.movement = new Vector3(newMovement.x, 0f, newMovement.y).normalized;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (player.movement == Vector3.zero)
            base.state_Machine.SetState(base.transitions[nameof(Player_Idle_State)]);

        PlayMovementAnimation();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();


        float targetAngle = Mathf.Atan2(player.movement.x, player.movement.z) * Mathf.Rad2Deg;

        player.lastAngle = targetAngle;
        

        float angle = Mathf.SmoothDampAngle(player.GetComponent<Transform>().eulerAngles.y, targetAngle,
            ref player.turn_Smooth_Velocity, player.turnSmoothTime);

        player.GetComponent<Transform>().rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        player.rigidbody.velocity = moveDir.normalized * player.speed + Vector3.up * player.rigidbody.velocity.y;
    }

    /// <summary>
    /// Play The Movement Animation
    /// </summary>
    public void PlayMovementAnimation()
    {
        player.anim.SetFloat(movement_Animation_Name, player.movement.magnitude - player.movement.y);
    }

    public override void OnExit()
    {
        base.OnExit();
        player.movement = Vector3.zero;
        player.input.OnPlayerMove -= Player_Input_OnPlayerMove;
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