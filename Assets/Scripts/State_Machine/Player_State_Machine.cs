using UnityEngine;

/// <summary>
/// Class To Handle The Player State Machine
/// </summary>
public class Player_State_Machine : State_Machine
{
    [SerializeField] public Player_Component player;

    [SerializeField] private Player_Idle_State idleState;
    [SerializeField] private Player_Movement_State moveState;
    [SerializeField] private Player_Jump_State jumpState;
    [SerializeField] private Player_Attack_State attackState;
    [SerializeField] private Player_Block_State blockState;
    [SerializeField] private Player_Hit_State hitState;
    [SerializeField] private Player_Dead_State deadState;

    protected override void OnEnable()
    {
        idleState = new Player_Idle_State(this, player);
        moveState = new Player_Movement_State(this, player);
        jumpState = new Player_Jump_State(this, player);
        attackState = new Player_Attack_State(this, player);
        blockState = new Player_Block_State(this, player);
        hitState = new Player_Hit_State(this, player);
        deadState = new Player_Dead_State(this, player);

        idleState.AddStateTransitions(nameof(Player_Movement_State), moveState);
        idleState.AddStateTransitions(nameof(Player_Jump_State), jumpState);
        idleState.AddStateTransitions(nameof(Player_Attack_State), attackState);
        idleState.AddStateTransitions(nameof(Player_Block_State), blockState);
        idleState.AddStateTransitions(nameof(Player_Hit_State), hitState);
        idleState.AddStateTransitions(nameof(Player_Dead_State), deadState);

        moveState.AddStateTransitions(nameof(Player_Idle_State), idleState);
        moveState.AddStateTransitions(nameof(Player_Jump_State), jumpState);
        moveState.AddStateTransitions(nameof(Player_Attack_State), attackState);
        moveState.AddStateTransitions(nameof(Player_Block_State), blockState);
        moveState.AddStateTransitions(nameof(Player_Hit_State), hitState);
        moveState.AddStateTransitions(nameof(Player_Dead_State), deadState);

        jumpState.AddStateTransitions(nameof(Player_Movement_State), moveState);
        jumpState.AddStateTransitions(nameof(Player_Idle_State), idleState);
        jumpState.AddStateTransitions(nameof(Player_Hit_State), hitState);
        jumpState.AddStateTransitions(nameof(Player_Dead_State), deadState);

        attackState.AddStateTransitions(nameof(Player_Idle_State), idleState);
        attackState.AddStateTransitions(nameof(Player_Movement_State), moveState);
        attackState.AddStateTransitions(nameof(Player_Jump_State), jumpState);
        attackState.AddStateTransitions(nameof(Player_Block_State), blockState);
        attackState.AddStateTransitions(nameof(Player_Hit_State), hitState);
        attackState.AddStateTransitions(nameof(Player_Dead_State), deadState);

        blockState.AddStateTransitions(nameof(Player_Idle_State), idleState);
        blockState.AddStateTransitions(nameof(Player_Movement_State), moveState);
        blockState.AddStateTransitions(nameof(Player_Jump_State), jumpState);
        blockState.AddStateTransitions(nameof(Player_Attack_State), attackState);
        blockState.AddStateTransitions(nameof(Player_Dead_State), deadState);

        hitState.AddStateTransitions(nameof(Player_Idle_State), idleState);
        hitState.AddStateTransitions(nameof(Player_Movement_State), moveState);
        hitState.AddStateTransitions(nameof(Player_Dead_State), deadState);

        deadState.AddStateTransitions(nameof(Player_Idle_State), idleState);

        base.OnEnable();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bullet_Controller>(out var bullet) && currentState != blockState)
        {
            player.character_Health_Component.DecreaseHealth(bullet.damage);
            Destroy(other.gameObject);
        }
    }
    protected override State GetInitialState()
    {
        return idleState;

    }
}
