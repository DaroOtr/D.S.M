using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class To Manage The Bullet Logic
/// </summary>
public class Bullet_Controller : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 5f;
    public float damage = 5f;

    private float timer;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// MOve The Bullet Foreward In the Current Direction
    /// </summary>
    public void Fire()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
