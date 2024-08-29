using UnityEngine;

/// <summary>
/// Class For Controling The Player Animations Logic
/// </summary>
public class Player_Animations_Controller : MonoBehaviour
{
    [Header("Anim Setup")]
    [SerializeField] private Animator anim;
    private Player_Controller controller;
    private Player_Movement movement_Controller;
    private const string jump_Anim_Name = "IsJumping";
    private const string movement_Anim_Name = "VelocityX/Z";
    private const string jump_Anim_Name2 = "VelocityY";
    private const string run_Anim_Name = "IsRuning";
    private const string blocking_Anim_Name = "Blocking";
    private const string attack_Anim_Name = "Attacking";
    private const string hit_Anim_Name = "GetHit";
    private const string dead_Anim_Name = "Death";

    private void OnEnable()
    {
        controller ??= GetComponent<Player_Controller>();
        if (!controller)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(controller)} is null");
        }

        movement_Controller ??= GetComponent<Player_Movement>();
        if (!movement_Controller)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(movement_Controller)} is null");
        }

        anim ??= GetComponent<Animator>();
        if (!anim)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(anim)} is null");
        }

        controller.OnPlayerMove += Controller_OnPlayerMove;
        controller.OnPlayerJump += Controller_OnPlayerJump;
        controller.OnPlayerSprint += Controller_OnPlayerSprint;
        controller.OnPlayerAttack += Controller_OnPlayerAttack;
        controller.OnPlayerBlock += Controller_OnPlayerBlock;
        controller.OnPlayerTakeDamage += Controller_OnPlayerTakeDamage;
        controller.OnPlayerDead += Controller_OnPlayerDead;

        movement_Controller.OnPlayerJump += Controller_OnPlayerJump;
    }

    private void Update()
    {
        if (movement_Controller.isGrounded())
        {
            anim.SetBool(jump_Anim_Name, false);
        }
        else
        {
            anim.SetBool(jump_Anim_Name, true);
        }
    }

    /// <summary>
    /// Event to Play The Moving Animation
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerMove(Vector2 obj)
    {
        Vector3 pos = new Vector3(obj.x,0f,obj.y);
        anim.SetFloat(movement_Anim_Name, pos.magnitude - pos.y);
    }

    /// <summary>
    /// Event to Play The Jump Animation V0.1
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerJump(bool obj)
    {
        anim.SetBool(jump_Anim_Name, obj);
    }
    /// <summary>
    /// Event to Play The Jump Animation V0.2
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerJump(float obj)
    {
        anim.SetFloat(jump_Anim_Name2, obj);
    }
    /// <summary>
    /// Event to Play The Sprint Animation
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerSprint(bool obj)
    {
        anim.SetBool(run_Anim_Name, obj);
    }
    /// <summary>
    /// Event to Play The Block Animation
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerBlock(bool obj)
    {
        anim.SetBool(blocking_Anim_Name, obj);
    }
    /// <summary>
    /// Event to Play The Attack Animation
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerAttack(bool obj)
    {
        anim.SetBool(attack_Anim_Name, obj);
    }
    /// <summary>
    /// Event to Play The Take Damage Animation
    /// </summary>
    private void Controller_OnPlayerTakeDamage()
    {
        anim.Play(hit_Anim_Name);
    }
    /// <summary>
    /// Event to Play The Death Animation
    /// </summary>
    private void Controller_OnPlayerDead()
    {
        anim.Play(dead_Anim_Name);
    }


    private void OnDisable()
    {
        controller.OnPlayerMove -= Controller_OnPlayerMove;
        controller.OnPlayerJump -= Controller_OnPlayerJump;
        controller.OnPlayerSprint -= Controller_OnPlayerSprint;
        controller.OnPlayerAttack -= Controller_OnPlayerAttack;
        controller.OnPlayerBlock -= Controller_OnPlayerBlock;
        controller.OnPlayerTakeDamage -= Controller_OnPlayerTakeDamage;
        controller.OnPlayerDead -= Controller_OnPlayerDead;

        movement_Controller.OnPlayerJump -= Controller_OnPlayerJump;
    }

    private void OnDestroy()
    {
        controller.OnPlayerMove -= Controller_OnPlayerMove;
        controller.OnPlayerJump -= Controller_OnPlayerJump;
        controller.OnPlayerSprint -= Controller_OnPlayerSprint;
        controller.OnPlayerAttack -= Controller_OnPlayerAttack;
        controller.OnPlayerBlock -= Controller_OnPlayerBlock;
        controller.OnPlayerTakeDamage -= Controller_OnPlayerTakeDamage;
        controller.OnPlayerDead -= Controller_OnPlayerDead;

        movement_Controller.OnPlayerJump -= Controller_OnPlayerJump;
    }
}
