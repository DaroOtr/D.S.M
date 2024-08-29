using UnityEngine;

/// <summary>
/// Class To Handle The Enemy Movement State
/// </summary>
public class Enemy_Move_State : Enemy_Base_State
{
    public Enemy_Move_State(Enemy_State_Machine enemySM, Enemy_Component enemy) : base(nameof(Enemy_Move_State), enemySM, enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.character_Health_Component.OnDecrease_Health += Character_Health_Component_OnDecrease_Health;
        enemy.character_Health_Component.OnInsufficient_Health += Character_Health_Component_OnInsufficient_Health;
        enemy.rigidbody.velocity = enemy.gameObject.transform.forward * enemy.speed;
    }

    private void Character_Health_Component_OnInsufficient_Health()
    {
        base.state_Machine.SetState(base.transitions[nameof(Enemy_Dead_State)]);
    }

    private void Character_Health_Component_OnDecrease_Health()
    {
        base.state_Machine.SetState(base.transitions[nameof(Enemy_Hit_State)]);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        float distance = Vector3.Distance(enemy.transform.position, enemy.target.position);
        if (distance <= enemy.lookRad || distance <= enemy.stopDistance)
            FaceTarget();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        float distance = Vector3.Distance(enemy.transform.position, enemy.target.position);
        if (distance <= enemy.lookRad)
        {
            enemy.rigidbody.velocity = enemy.gameObject.transform.forward * enemy.speed;

            if (distance <= enemy.stopDistance)
            {
                base.state_Machine.SetState(base.transitions[nameof(Enemy_Attack_State)]);
            }
        }
        else
            base.state_Machine.SetState(base.transitions[nameof(Enemy_Idle_State)]);
    }
    public void PlayMoveAnimation()
    {
        enemy.anim.SetFloat("Velocity_X/Z", enemy.movement.magnitude - enemy.movement.y);
    }

    private void FaceTarget()
    {
        enemy.transform.LookAt(enemy.target.position);
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.rigidbody.velocity = new Vector3(0f, 0f, 0f);
        enemy.character_Health_Component.OnDecrease_Health -= Character_Health_Component_OnDecrease_Health;
        enemy.character_Health_Component.OnInsufficient_Health -= Character_Health_Component_OnInsufficient_Health;
    }

    public override void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}
