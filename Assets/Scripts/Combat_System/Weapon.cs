using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private string weaponName;
    [SerializeField] private float weaponDamage;
    [SerializeField] private BoxCollider weaponCollider;
    
    public string name { get => weaponName; set => weaponName = value; }
    public float damage { get => weaponDamage; set => weaponDamage = value; }

    private void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            
        }
    }

    public void EnableTriggerBox() { weaponCollider.enabled = true; }
    public void DisableTriggerBox() { weaponCollider.enabled = false; }
}
