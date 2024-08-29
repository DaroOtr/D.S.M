using UnityEngine;

/// <summary>
/// Class To Handle The Enemy Dead State
/// </summary>
public class Enemy_Dead_State : Enemy_Base_State
{
    private const string dead_Animation_Name = "Standing React Death Backward";
    private float destroyTimer;
    private float destroyTime;

    public Enemy_Dead_State(Enemy_State_Machine enemySM, Enemy_Component enemy) : base(nameof(Enemy_Dead_State), enemySM, enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayDeathAnimation();
        DestroyEnemy();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    /// <summary>
    /// Destroy The ENemy GameObject
    /// </summary>
    private void DestroyEnemy()
    {

        destroyTimer += Time.deltaTime;
        if (destroyTimer >= destroyTime)
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }

    /// <summary>
    /// Function to Play the Dead Animation For The Enemy
    /// </summary>
    public void PlayDeathAnimation() 
    {
         enemy.anim.Play(dead_Animation_Name);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}
