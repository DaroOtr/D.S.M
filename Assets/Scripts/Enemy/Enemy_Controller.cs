using System;
using UnityEngine;


/// <summary>
/// Class For The Enemy Logic
/// </summary>
[Obsolete]public class Enemy_Controller : Character_Component
{
    [SerializeField] private float lookRad = 20f;
    [SerializeField] private float stopDistance = 5f;
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    [SerializeField] private float destroyTime;
    [SerializeField] private float destroyTimer;
    [SerializeField] private bool ready_To_Attack;
    [SerializeField] private bool deathLoop;
    [SerializeField] private Transform target;
    [SerializeField] private Transform bulletSpawn;

    [SerializeField] private Player_Data_Source player_Source;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Enemy_Animation_Controller enemyAnimController;

    public event Action<Vector2> OnEnemyMove;
    public event Action OnEnemyAttack;
    public event Action OnEnemyHit;
    public event Action OnEnemyDeath;

    private void Start()
    {
        enemyAnimController.OnBulletSpawn += EnemyAnimController_OnBulletSpawn;

        ready_To_Attack = true;

        character_Health_Component._health = 100;

        if (target == null)
        {
            target = player_Source._player.transform;
        }
        if (!target)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(target)} is null");
            enabled = false;
        }
        
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        if (!rigidbody)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(rigidbody)} is null");
            enabled = false;
        }

        deathLoop = false;
    }


    /// <summary>
    /// Spawn A Bullet Prefab
    /// </summary>
    private void EnemyAnimController_OnBulletSpawn()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
        Bullet_Controller bulletScript = bullet.GetComponent<Bullet_Controller>();
        bulletScript.Fire();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if(distance <= lookRad || distance <= stopDistance)
        FaceTarget();

        CheckHealth();
    }

    private void FixedUpdate()
    {
        if (!deathLoop)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= lookRad)
            {
                //transform.Translate(Vector3.forward * Time.deltaTime * speed);
                rigidbody.velocity = gameObject.transform.forward * speed;
                if (distance <= stopDistance)
                {

                    rigidbody.velocity = new Vector3(0f, 0f, 0f);
                }

                Vector2 pos = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);
                OnEnemyMove.Invoke(pos); 
            }

        }
    }


    /// <summary>
    /// Checks If The Enemy Is Alive
    /// </summary>
    private void CheckHealth()
    {
        if (character_Health_Component._health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    
    /// <summary>
    /// Make The Enemy Loock At The Target
    /// </summary>
    private void FaceTarget()
    {
        transform.LookAt(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: TP2 - SOLID
        if (character_Health_Component._health >= 0)
        {
            if (other.CompareTag("Player_Weapon"))
            {
                TakeDamage(other.GetComponent<Weapon_Stats>().WeaponDamage);
            }
        }
    }

    /// <summary>
    /// Make The Enemy Take Damage Based On The Damage Param
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        character_Health_Component._health -= damage;
        OnEnemyHit.Invoke();
    }


    /// <summary>
    /// Destroy The Enemy After The Dead Animation
    /// </summary>
    private void DestroyEnemy()
    {
        if (!deathLoop)
        {
            OnEnemyDeath.Invoke();
            deathLoop = true;
        }

        destroyTimer += Time.deltaTime;
        if (destroyTimer >= destroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRad);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }

    private void OnDestroy()
    {
        enemyAnimController.OnBulletSpawn -= EnemyAnimController_OnBulletSpawn;
    }
}
