using UnityEngine;

/// <summary>
/// Class To Handle The Enemy Idle State
/// </summary>
public class Enemy_Idle_State : Enemy_Base_State
{
    private const string enemy_Idle_Animation_Name = "Squeleton_Idle";

    public Enemy_Idle_State(Enemy_State_Machine enemySM, Enemy_Component enemy) : base(nameof(Enemy_Idle_State), enemySM, enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.character_Health_Component.OnDecrease_Health += Character_Health_Component_OnDecrease_Health;
        enemy.character_Health_Component.OnInsufficient_Health += Character_Health_Component_OnInsufficient_Health;
        PlayIdleAnim();
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
            base.state_Machine.SetState(base.transitions[nameof(Enemy_Move_State)]);
        }
    }

    private void FaceTarget()
    {
        enemy.transform.LookAt(enemy.target.position);
    }

    public void PlayIdleAnim() 
    {
        enemy.anim.Play(enemy_Idle_Animation_Name);
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.character_Health_Component.OnDecrease_Health -= Character_Health_Component_OnDecrease_Health;
        enemy.character_Health_Component.OnInsufficient_Health -= Character_Health_Component_OnInsufficient_Health;
    }

    public override void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}
