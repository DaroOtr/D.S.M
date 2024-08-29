using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class for The Management of the Interaction Between the Player && the Weapon
/// </summary>
public class Weapon_Interaction_Controller : MonoBehaviour
{
    private static bool isSlotFull;
    [SerializeField] private bool isEquiped;

    [SerializeField] private Player_Data_Source player_Source;
    [SerializeField] private Weapon_Stats weapon;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider coll;
    [SerializeField] private Transform player;
    [SerializeField] private Transform weapon_Container;

    [SerializeField] private float pickUp_Range;
    [SerializeField] private float dropForwardForce;
    [SerializeField] private float dropUpwardForce;

    private void Start()
    {
        isEquiped = false;
        isSlotFull = false;

        //if (player == null)
        //{
        //    player = player_Source._player.transform;
        //}
        //if (!player)
        //{
        //    Debug.LogError(message: $"{name}: (logError){nameof(player)} is null");
        //    enabled = false;
        //}

        //if (weapon_Container == null)
        //{
        //    weapon_Container = player_Source._player.weaponHolder;
        //
        //}
        //if (!weapon_Container)
        //{
        //    Debug.LogError(message: $"{name}: (logError){nameof(weapon_Container)} is null");
        //    enabled = false;
        //}

        if (!isEquiped)
        {
            weapon.enabled = false;
            coll.isTrigger = false;
        }
        else
        {
            weapon.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            isSlotFull = true;
        }


        if (player_Source._player == null)
            return;

        player_Source._player.GetPlayerInputReader().PickUp += Input_OnPlayerPickUp;
        player_Source._player.GetPlayerInputReader().Drop += Input_OnPlayerDrop;
    }

    /// <summary>
    /// Event for The Player Drop Input
    /// </summary>
    private void Input_OnPlayerDrop()
    {
        if (isEquiped)
        {
            Drop_Weapon();
        }
    }

    /// <summary>
    /// Event for The Player PickUp Input
    /// </summary>
    private void Input_OnPlayerPickUp()
    {
        Vector3 distance = player.position - transform.position;
        if (!isEquiped && distance.magnitude <= pickUp_Range && !isSlotFull)
        {
            PickUp_Weapon();
        }
    }

    private void Update()
    {
        if (isEquiped)
        {
            UpdateEquipedPos();
        }
    }

    /// <summary>
    /// Update The Position of The Equipped Weapon
    /// </summary>
    private void UpdateEquipedPos() 
    {
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// PickUp The Current Weapon
    /// </summary>
    private void PickUp_Weapon()
    {
        isEquiped = true;
        isSlotFull = true;

        player_Source._player.SetCurrentWeapon(this.weapon);

        rb.isKinematic = true;
        coll.isTrigger = true;
        weapon.enabled = true;


        transform.SetParent(weapon_Container);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    /// <summary>
    /// Drop The Current Weapon
    /// </summary>
    private void Drop_Weapon()
    {
        isEquiped = false;
        isSlotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(player.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(player.up * dropUpwardForce, ForceMode.Impulse);

        float rand = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(rand, rand, rand) * 10f);
        weapon.enabled = false;
    }

    private void OnDisable()
    {
        if (player_Source._player == null)
            return;

        player_Source._player.GetPlayerInputReader().PickUp -= Input_OnPlayerPickUp;
        player_Source._player.GetPlayerInputReader().Drop -= Input_OnPlayerDrop;
    }

    private void OnDrawGizmos()
    {
        if (!isEquiped)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, pickUp_Range);
        }
    }
}
