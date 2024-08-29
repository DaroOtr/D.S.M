using System;
using UnityEngine;


/// <summary>
/// Class For The Enemy Animation Logic
/// </summary>
[Obsolete]public class Enemy_Animation_Controller : MonoBehaviour
{
    [Header("Anim Setup")]
    [SerializeField] private Animator animator;
    //TODO: TP2 - Syntax - Consistency in naming convention
    [SerializeField] private Enemy_Controller controller;

    public event Action OnBulletSpawn;

    private void Start()
    {
        

        controller.OnEnemyMove += Controller_OnEnemyMove;
        controller.OnEnemyAttack += Controller_OnEnemyAttack;
        controller.OnEnemyHit += Controller_OnEnemyHit;
        controller.OnEnemyDeath += Controller_OnEnemyDeath;
    }

    /// <summary>
    /// Function To Play The Death Animation
    /// </summary>
    private void Controller_OnEnemyDeath()
    {
        animator.Play("Standing React Death Backward");
    }

    /// <summary>
    /// Function To Play The Hit Animation
    /// </summary>
    private void Controller_OnEnemyHit()
    {
        animator.Play("Get_Hit");
    }

    /// <summary>
    /// Function To Play The Enemy Attack
    /// </summary>
    private void Controller_OnEnemyAttack()
    {
        animator.Play("Standing 1H Magic Attack 01");
    }

    /// <summary>
    /// Function To Play The Enemy Movement Animation
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnEnemyMove(Vector2 obj)
    {
        Vector3 pos = new Vector3(obj.x, 0f, obj.y);
        animator.SetFloat("Velocity_X/Z",pos.magnitude - pos.y);
    }

    /// <summary>
    /// Event To Spaen The Enemy Bullet
    /// </summary>
    public void Spawn_Bullet() 
    {
        OnBulletSpawn.Invoke();
    }

    private void OnDestroy()
    {
        controller.OnEnemyMove -= Controller_OnEnemyMove;
        controller.OnEnemyAttack -= Controller_OnEnemyAttack;
        controller.OnEnemyHit -= Controller_OnEnemyHit;
        controller.OnEnemyDeath -= Controller_OnEnemyDeath;
    }
}
