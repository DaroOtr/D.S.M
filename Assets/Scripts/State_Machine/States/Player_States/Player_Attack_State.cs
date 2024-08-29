using UnityEditor;
using UnityEngine;
/// <summary>
/// Class To Handle The Player Attack State
/// </summary>
public class Player_Attack_State : Player_Base_State
{
    private const string attack_Animation_Name = "Sword And Shield Slash";
    private float attkCounter;
    private float attkTimer;
    private LayerMask layerMask;
    public Player_Attack_State(Player_State_Machine playerSM, Player_Component player) : base(nameof(Player_Attack_State), playerSM, player) { }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayAttackAnimation();

        layerMask = 0;

        attkCounter = 1.5f;
        attkTimer = 0.0f;

        player.isPlayer_Attacking = true;

        player.input.OnPlayerAttack += Input_OnPlayerAttack;
        player.input.OnPlayerMove += Input_OnPlayerMove;
        player.input.OnPlayerJump += Input_OnPlayerJump;
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
        if (attkTimer >= attkCounter)
            base.state_Machine.SetState(base.transitions[nameof(Player_Block_State)]);
    }

    private void Input_OnPlayerJump(bool obj)
    {
        if (attkTimer >= attkCounter)
            base.state_Machine.SetState(base.transitions[nameof(Player_Jump_State)]);
    }

    private void Input_OnPlayerMove(Vector2 obj)
    {
        if (attkTimer >= attkCounter)
        {
            player.movement = new Vector3(obj.x + 1.0f, 0f, obj.y + 1.0f).normalized;
            base.state_Machine.SetState(base.transitions[nameof(Player_Movement_State)]);
        }
    }

    private void Input_OnPlayerAttack(bool obj)
    {
        player.isPlayer_Attacking = obj;
        PlayAttackAnimation();
    }

    public override void UpdateLogic()
    {
        if (attkTimer >= attkCounter)
        {
            player.isPlayer_Attacking = false;
            base.state_Machine.SetState(base.transitions[nameof(Player_Idle_State)]);
        }
        else
        {
            attkTimer += Time.deltaTime;
        }
    }

    public override void UpdatePhysics()
    {

    }

    /// <summary>
    /// Play The Attack Animation for the Player
    /// </summary>
    public void PlayAttackAnimation()
    {
        if (Physics.SphereCast(player.transform.position,player.current_Weapon_Rad, player.transform.forward,out var hit,player.current_Weapon_MaxDistance, layerMask, QueryTriggerInteraction.UseGlobal)) 
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.TryGetComponent<Enemy_Component>(out var enemy_Component)) 
            {
                enemy_Component.character_Health_Component.DecreaseHealth(player.current_Weapon.WeaponDamage);
            }
        }
        player.anim.Play(attack_Animation_Name);
    }

    public override void OnExit()
    {
        player.isPlayer_Attacking = false;

        player.input.OnPlayerAttack -= Input_OnPlayerAttack;
        player.input.OnPlayerMove -= Input_OnPlayerMove;
        player.input.OnPlayerJump -= Input_OnPlayerJump;
        player.input.OnPlayerBlock -= Input_OnPlayerBlock;
        player.character_Health_Component.OnDecrease_Health -= Character_Health_Component_OnDecrease_Health;
        player.character_Health_Component.OnInsufficient_Health -= Character_Health_Component_OnInsufficient_Health;
    }

    public virtual void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}
