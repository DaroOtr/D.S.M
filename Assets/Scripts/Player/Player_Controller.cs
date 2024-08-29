using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class For Controling The PLayer Logic
/// </summary>
[Obsolete]public class Player_Controller : MonoBehaviour
{
    /// <summary>
    /// Action Event For The Player Movement
    /// </summary>
    public event Action<Vector2> OnPlayerMove;

    /// <summary>
    /// Action Event For The Player Jump
    /// </summary>
    public event Action<bool> OnPlayerJump;

    /// <summary>
    /// Action Event For The Player Sprint
    /// </summary>
    public event Action<bool> OnPlayerSprint;

    /// <summary>
    /// Action Event For The Player Attack
    /// </summary>
    public event Action<bool> OnPlayerAttack;

    /// <summary>
    /// Action Event For The Player Block
    /// </summary>
    public event Action<bool> OnPlayerBlock;

    /// <summary>
    /// Action Event For The Player Take Damage
    /// </summary>
    public event Action OnPlayerTakeDamage;

    /// <summary>
    /// Action Event For The Player Dead
    /// </summary>
    public event Action OnPlayerDead;

    /// <summary>
    /// Action Event For The Player PickUp
    /// </summary>
    public event Action OnPlayerPickUp;

    /// <summary>
    /// Action Event For The Player Drop
    /// </summary>
    public event Action OnPlayerDrop;

    /// <summary>
    /// Action Event For The Player Pause
    /// </summary>
    public event Action OnPlayerPause;

    public static Player_Controller playerPos;
    public Transform playerHolder;

    public Game_Manager _Manager;
    [SerializeField] private float health;
    [SerializeField] private Player_Setings setings;
    [SerializeField] private PlayerInput input;
    [SerializeField] private AudioClip swing;

    

    private void OnEnable()
    {
        playerPos = this;

        //_Manager.SetMaxHealth();
        //health = setings.health;
    }

    /// <summary>
    /// Get The Player Health
    /// </summary>
    /// <returns></returns>
    public float GetHealth() 
    {
        return health;
    }

    /// <summary>
    /// Get The Player Setings
    /// </summary>
    /// <returns></returns>
    public Player_Setings GetPlayerSetings()
    {
        return setings;
    }

    /// <summary>
    /// Triggers The Movement Event
    /// </summary>
    /// <param name="input"></param>
    public void OnMove(InputValue input)
    {
        if (OnPlayerMove != null)
        {
            OnPlayerMove.Invoke(input.Get<Vector2>());
        }
        else
            Debug.LogWarning($"On Move: event has no listeners");
    }

    /// <summary>
    /// Triggers The Jumping Event
    /// </summary>
    /// <param name="input"></param>
    public void OnJump(InputValue input)
    {
        if (OnPlayerJump != null)
            OnPlayerJump.Invoke(input.isPressed);
        else
            Debug.LogWarning($"On Jump: event has no listeners");
    }

    /// <summary>
    /// Triggers The Sprint Event
    /// </summary>
    /// <param name="input"></param>
    public void OnSprint(InputValue input)
    {
        if (OnPlayerSprint != null)
            OnPlayerSprint.Invoke(input.isPressed);
        else
            Debug.LogWarning($"On Sprint: event has no listeners");
    }

    /// <summary>
    /// Triggers The Block Event
    /// </summary>
    /// <param name="input"></param>
    public void OnR_Click(InputValue input)
    {
        if (OnPlayerBlock != null)
            OnPlayerBlock.Invoke(input.isPressed);
        else
            Debug.LogWarning($"OnR_Click: event has no listeners");
    }

    /// <summary>
    /// Triggers The Attack Event
    /// </summary>
    /// <param name="input"></param>
    public void OnL_Click(InputValue input)
    {
        if (OnPlayerAttack != null)
            OnPlayerAttack.Invoke(input.isPressed);
        if (input.isPressed)
            SoundManager.Instance.PlaySound(swing);

        else
            Debug.LogWarning($"OnL_Click: event has no listeners");
    }

    /// <summary>
    /// Triggers The PickUp Event
    /// </summary>
    /// <param name="input"></param>
    public void OnPickUp(InputValue input) 
    {
        if (input.isPressed) 
        {
            Debug.Log("OnPlayerPickUp");
            OnPlayerPickUp.Invoke();
        }

    }

    /// <summary>
    /// Triggers The Drop Event
    /// </summary>
    /// <param name="input"></param>
    public void OnDrop(InputValue input) 
    {
        if (input.isPressed) 
        {
            Debug.Log("OnPlayerDrop");
            OnPlayerDrop.Invoke();
        }
    }

    /// <summary>
    /// Triggers The Pause Event
    /// </summary>
    public void OnPause() 
    {
        OnPlayerPause.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO - Fix - Hardcoded value
        if (other.CompareTag("Bullet"))
        {
            if (!setings.isBlocking)
            {
                if (setings.health <= 0)
                    return;

                OnPlayerTakeDamage.Invoke();
                TakeDamage(other.GetComponent<Bullet_Controller>().damage);
                Destroy(other.gameObject);
            }
            else
            {
                return;
            }
        }
    }

    private void Update()
    {
        CheckHealth();
    }

    /// <summary>
    /// Check The Player Health
    /// </summary>
    private void CheckHealth()
    {
        if (health <= 0)
        {
            OnPlayerDead.Invoke();
        }
        _Manager.UpdateHealth();
    }

    /// <summary>
    /// Decrease The Player Health
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        //TODO: TP2 - FSM
        if (health <= 0)
            return;

        health -= damage;
        Debug.Log("Player Health " + health);
    }
}
