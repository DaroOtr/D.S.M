using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class For The Bullet Logic
/// </summary>
public class Bullet : MonoBehaviour
{
    private const string player_Tag = "Player";
    [SerializeField] private bool isShoot;
    [SerializeField] private Transform target;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        //TODO - Fix - Hardcoded value
        target ??= GameObject.FindGameObjectWithTag(player_Tag).transform;
        if (!target)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(target)} is null");
            enabled = false;
        }

        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(rb)} is null");
            enabled = false;
        }
    }


    private void Update()
    {
        if (isShoot)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0f);
            if (transform.position == target.position)
            {
                DestroyBullet();
            }
        }
    }

    /// <summary>
    /// Add Force To The Bullet RigidBody
    /// </summary>
    /// <param name="force"></param>
    /// <param name="mode"></param>
    public void AddBulletForce(Vector3 force, ForceMode mode)
    {
        rb.AddForce(force, mode);
    }

    /// <summary>
    /// Set If The Bullet Is Shoot
    /// </summary>
    /// <param name="value"></param>
    public void SetIsShoot(bool value) 
    {
        isShoot = value;
    }

    /// <summary>
    /// Return If The Bullet Was Shoot
    /// </summary>
    /// <returns></returns>
    public bool GetIsShoot() 
    {
        return isShoot;
    }

    /// <summary>
    /// Return The Bullet Prefab
    /// </summary>
    /// <returns></returns>
    public GameObject GetBulletPrefab() 
    {
        return bullet;
    }

    /// <summary>
    /// Set The Bullet RigidBody Velocity
    /// </summary>
    /// <param name="newVelocity"></param>
    public void SetRbVelocity(Vector3 newVelocity)
    {
        rb.velocity = newVelocity;
    }

    /// <summary>
    /// Set The Bullet Target
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// Set The Parent for The Bullet to Spawn From
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == target.tag)
        {
            Invoke(nameof(DestroyBullet), 0.5f);
        }
    }

    /// <summary>
    /// Destroy The Bullet GameObject
    /// </summary>
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
