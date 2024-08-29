using System.Collections;
using UnityEngine;

/// <summary>
/// Class To Handle The Enemy Attack State
/// </summary>
public class Enemy_Attack_State : Enemy_Base_State
{
    private const string rangedEnemy_Attack_Animation_Name = "Standing 1H Magic Attack 01";
    private const string meleEnemy_Attack_Animation_Name = "Sword And Shield Slash";
    private float bulletSpawnDelay;
    private float attackDistance = 1f;

    public Enemy_Attack_State(Enemy_State_Machine enemySM, Enemy_Component enemy) : base(nameof(Enemy_Attack_State),
        enemySM, enemy)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        bulletSpawnDelay = 1.0f;
        enemy.character_Health_Component.OnDecrease_Health += Character_Health_Component_OnDecrease_Health;
        enemy.character_Health_Component.OnInsufficient_Health += Character_Health_Component_OnInsufficient_Health;
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
        else
            base.state_Machine.SetState(base.transitions[nameof(Enemy_Idle_State)]);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        float distance = Vector3.Distance(enemy.transform.position, enemy.target.position);
        if (distance <= enemy.lookRad)
        {
            if (distance <= enemy.stopDistance)
            {
                if (enemy.ready_To_Attack)
                {
                    if (enemy.IsRanged)
                        enemy.StartCoroutine(RangeAttackPlayer(bulletSpawnDelay, enemy.timeBetweenAttacks));
                    else
                        enemy.StartCoroutine(MeleAttackPlayer(enemy.timeBetweenAttacks));
                }

                enemy.rigidbody.velocity = new Vector3(0f, 0f, 0f);
            }
            else if (enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                base.state_Machine.SetState(base.transitions[nameof(Enemy_Move_State)]);
        }
        else if (enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            base.state_Machine.SetState(base.transitions[nameof(Enemy_Idle_State)]);
    }

    private void FaceTarget()
    {
        enemy.transform.LookAt(enemy.target.position);
    }

    private IEnumerator RangeAttackPlayer(float bulletSpawnDelay, float AttackCooldown)
    {
        enemy.ready_To_Attack = false;
        PlayAttackAnim();
        yield return new WaitForSeconds(bulletSpawnDelay);
        Spawn_Bullet();
        yield return new WaitForSeconds(AttackCooldown);
        enemy.ready_To_Attack = true;
    }
    
    private IEnumerator MeleAttackPlayer(float AttackCooldown)
    {
        enemy.ready_To_Attack = false;
        PlayAttackAnim();
        yield return new WaitForSeconds(bulletSpawnDelay);
        MeleAttack();
        yield return new WaitForSeconds(AttackCooldown);
        enemy.ready_To_Attack = true;
    }

    public void PlayAttackAnim()
    {
        if (enemy.IsRanged)
            enemy.anim.Play(rangedEnemy_Attack_Animation_Name);
        else
            enemy.anim.Play(meleEnemy_Attack_Animation_Name);
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.character_Health_Component.OnDecrease_Health -= Character_Health_Component_OnDecrease_Health;
        enemy.character_Health_Component.OnInsufficient_Health -= Character_Health_Component_OnInsufficient_Health;
    }

    public void Spawn_Bullet()
    {
        if (enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            GameObject bullet =
                GameObject.Instantiate(enemy.bulletPrefab, enemy.bulletSpawn.position, enemy.transform.rotation);
            Bullet_Controller bulletScript = bullet.GetComponent<Bullet_Controller>();
            bulletScript.Fire();
        }
    }
    
    public void MeleAttack()
    {
        Vector3 attackPos = enemy.transform.position + enemy.transform.forward;
        
        Collider[] hitEnemies = Physics.OverlapSphere(attackPos, attackDistance);

        foreach (var target in hitEnemies)
        {
            Debug.Log(target.name);
            if (target.CompareTag("Player"))
            {
                GameObject bullet =
                    GameObject.Instantiate(enemy.bulletPrefab, enemy.transform.position + enemy.transform.forward, enemy.transform.rotation);
                target.GetComponent<Health_Component>().DecreaseHealth(enemy.damage);
            }
        }
    }

    public override void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}