using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class For Controling The Player Movement Logic
/// </summary>
public class Player_Movement : MonoBehaviour
{

    /// <summary>
    /// Action Event For The Player Jump Event
    /// </summary>
    public event Action<float> OnPlayerJump;

    /// <summary>
    /// Action Event For The Player Attack Event
    /// </summary>
    public event Action<float> OnPlayerAttack;

    /// <summary>
    /// Action Event For The Player Block Event
    /// </summary>
    public event Action<float> OnPlayerBlock;

    private Player_Controller controller;

    [Header("SetUp")]
    [SerializeField] private Player_Setings setings;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform feet_Pivot;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float jumpBufferTimeCounter;
    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private float lastAngle;
    [SerializeField] private Coroutine _jumpCorutine;
    [Header("Movement")]
    [SerializeField] Vector3 _CurrentMovement;

    [SerializeField] private float initialSpeed;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool wasJumping;
    [SerializeField] private bool isSprinting;

    [Header("Coyote Time Setup")]
    [SerializeField] private float coyoteTimerCounter;

    private void OnEnable()
    {
        controller.OnPlayerMove += Controller_OnPlayerMove;
        controller.OnPlayerJump += Controller_OnPlayerJump;
        controller.OnPlayerSprint += Controller_OnPlayerSprint;
        controller.OnPlayerAttack += Controller_OnPlayerAttack;
        controller.OnPlayerBlock += Controller_OnPlayerBlock;

        rigidbody ??= GetComponent<Rigidbody>();
        if (!rigidbody)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(rigidbody)} is null");
            enabled = false;
        }

        feet_Pivot ??= GetComponent<Transform>();
        if (!feet_Pivot)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(feet_Pivot)} is null");
        }

        controller ??= GetComponent<Player_Controller>();
        if (!controller)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(controller)} is null");
        }

        setings = controller.GetPlayerSetings();

        isJumping = false;
        isSprinting = false;
        initialSpeed = setings.speed;
    }


    public void FixedUpdate()
    {
        if (isGrounded())
        {
            coyoteTimerCounter = setings.coyoteTime;
            jumpBufferTimeCounter = setings.jumpBufferTime;
        }
        else
        {
            coyoteTimerCounter -= Time.deltaTime;
            jumpBufferTimeCounter -= Time.deltaTime;
        }

        if (_CurrentMovement.magnitude >= 1f)
        {
            if (isGrounded())
            {
                float targetAngle = Mathf.Atan2(_CurrentMovement.x, _CurrentMovement.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                lastAngle = targetAngle;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, setings.turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                //rigidbody.drag = 0f;
                rigidbody.velocity = moveDir.normalized * setings.speed + Vector3.up * rigidbody.velocity.y;
            }
            else
            {
                Vector3 moveDir = Quaternion.Euler(0f, lastAngle, 0f) * Vector3.forward;
                rigidbody.velocity = moveDir.normalized * setings.speed + Vector3.up * rigidbody.velocity.y;
            }

        }

        if (isSprinting)
        {
            setings.speed = initialSpeed * 2;
        }
        else
        {
            setings.speed = initialSpeed;
        }
        OnPlayerJump.Invoke(rigidbody.velocity.y);
    }

    /// <summary>
    /// Function For The player Movemet Event
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerMove(Vector2 obj)
    {
        //TODO: TP2 - FSM
        if (!isGrounded())
            return;

        var movement = obj;
        _CurrentMovement = new Vector3(movement.x, 0f, movement.y).normalized;
    }

    /// <summary>
    /// Function For The player Jump Event
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerJump(bool obj)
    {

        if (_jumpCorutine != null)
            StopCoroutine(_jumpCorutine);
        _jumpCorutine = StartCoroutine(JumpCorutine(setings.jumpBufferTime));

        StopCoroutine(JumpCorutine(setings.jumpBufferTime));
        if (obj && rigidbody.velocity.y > 0f)
        {
            rigidbody.velocity = _CurrentMovement * setings.speed + Vector3.up * rigidbody.velocity.y * 0.5f;
            coyoteTimerCounter = 0f;
        }
    }

    /// <summary>
    /// Function For The player Sprint Event
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerSprint(bool obj)
    {
        //TODO: TP2 - Remove unused methods/variables
        //Debug.Log(obj);
        isSprinting = obj;
    }

    /// <summary>
    /// Function For The player Block Event
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerBlock(bool obj)
    {
        setings.isBlocking = obj;
    }

    /// <summary>
    /// Function For The player Attack Event
    /// </summary>
    /// <param name="obj"></param>
    private void Controller_OnPlayerAttack(bool obj)
    {
        setings.isAttacking = obj;
    }

    /// <summary>
    /// Function For The Jump Corutine
    /// </summary>
    /// <param name="bufferTime"></param>
    /// <returns></returns>
    private IEnumerator JumpCorutine(float bufferTime)
    {
        if (!feet_Pivot)
        {
            yield break;
        }

        float timeElapsed = 0;

        while (timeElapsed <= bufferTime)
        {
            yield return new WaitForFixedUpdate();

            if (coyoteTimerCounter > 0f && jumpBufferTimeCounter > 0f && !isJumping)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
                rigidbody.AddForce(Vector3.up * setings.jumpForce, ForceMode.Impulse);
                if (timeElapsed > 0)
                {
                    Debug.Log(message: $"{name}: buffer jump for {timeElapsed} seconds");
                }
                yield break;

            }

            timeElapsed += Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Function For The player Spawn
    /// </summary>
    /// <param name="pos"></param>
    public void SetSpawnPos(Vector3 pos) 
    {
        this.rigidbody.position = pos;

    }

    /// <summary>
    /// Check If The Player Is At Ground Level
    /// </summary>
    /// <returns></returns>
    public bool isGrounded()
    {
        return Physics.Raycast(feet_Pivot.position, Vector3.down, out var hit, setings.maxDistance) && hit.distance <= setings.minJumpDistance;
    }

    private void OnDisable()
    {
        controller.OnPlayerMove -= Controller_OnPlayerMove;
        controller.OnPlayerJump -= Controller_OnPlayerJump;
        controller.OnPlayerSprint -= Controller_OnPlayerSprint;
        controller.OnPlayerAttack -= Controller_OnPlayerAttack;
        controller.OnPlayerBlock -= Controller_OnPlayerBlock;
    }
    private void OnDestroy()
    {
        controller.OnPlayerMove -= Controller_OnPlayerMove;
        controller.OnPlayerJump -= Controller_OnPlayerJump;
        controller.OnPlayerSprint -= Controller_OnPlayerSprint;
        controller.OnPlayerAttack -= Controller_OnPlayerAttack;
        controller.OnPlayerBlock -= Controller_OnPlayerBlock;
    }
}
