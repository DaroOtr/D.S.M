using UnityEngine;

/// <summary>
/// Class To Handle The Enemy Hit State
/// </summary>
public class Enemy_Hit_State : Enemy_Base_State
{
    private const string hit_Animation_State = "Get_Hit";
    private float animTime = 1.5f;
    private float animTimer = 0.0f;
    private bool IsAttacking;

    public Enemy_Hit_State(Enemy_State_Machine enemySM, Enemy_Component enemy) : base(nameof(Enemy_Hit_State), enemySM,
        enemy)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.character_Health_Component.OnInsufficient_Health += Character_Health_Component_OnInsufficient_Health;
        animTime = 1.5f;
        animTimer = 0.0f;
        IsAttacking = true;
        PlayHitAnimation();
    }

    private void Character_Health_Component_OnInsufficient_Health()
    {
            base.state_Machine.SetState(base.transitions[nameof(Enemy_Dead_State)]);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (animTimer >= animTime)
        {
            if (enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
                base.state_Machine.SetState(base.transitions[nameof(Enemy_Idle_State)]);
        }
        else
            animTimer += Time.deltaTime;
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    /// <summary>
    /// Function to Play the Hit Animation For The Enemy
    /// </summary>
    public void PlayHitAnimation()
    {
        GameObject.Instantiate(enemy.hit_Particles, enemy.hit_Particles_spawn.transform.position, Quaternion.identity,
            enemy.transform);
        enemy.anim.Play(hit_Animation_State);
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.character_Health_Component.OnInsufficient_Health -= Character_Health_Component_OnInsufficient_Health;
        animTime = 0.0f;
        animTimer = 0.0f;
    }

    public override void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}