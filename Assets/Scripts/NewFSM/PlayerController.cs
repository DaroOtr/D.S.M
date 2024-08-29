using System;
using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

public class PlayerController : ValidatedMonoBehaviour
{
    [Header("References")] 
    [SerializeField, Self] private Animator _animator;
    [SerializeField, Self] private Rigidbody _rigidbody;
    [SerializeField, Self] private Health_Component _healthComponent;
    [SerializeField, Anywhere] private GroundChecker groundChecker;
    [SerializeField, Anywhere] private CinemachineVirtualCamera _camera;
    [SerializeField, Anywhere] private InputReader _input;
    [SerializeField, Anywhere] private Player_Data_Source _playerDataSource;

    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 6.0f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float smoothTime = 0.2f;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float jumpCooldown = 0.0f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 10.0f;
    [SerializeField] private float dashDuration = 1.0f;
    [SerializeField] private float dashCooldown = 2.0f;
    
    [Header("Attack Settings")]
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float attackDistance = 1f;
    [SerializeField] int attackDamage = 10;
        
    Transform mainCam;
        
    private const float ZeroF = 0f;
    private float currentSpeed;
    private float velocity;
    private float jumpVelocity;
    private float dashVelocity = 1.0f;

    private List<Timer> _timers;
    private CountdownTimer jumpTimer;
    private CountdownTimer jumpCooldownTimer;
    private CountdownTimer dashTimer;
    private CountdownTimer dashCooldownTimer;
    private CountdownTimer attackTimer;

    private StateMachine _stateMachine;
    
    public Weapon_Stats current_Weapon;
    public Transform weaponHolder;
    
    //Animator parameters
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        mainCam = Camera.main.transform;
        SetUpPlayer();
        SetupStateMachine();
        SetupTimers();
    }

    private void SetUpPlayer()
    {
        _playerDataSource._player = this;
        _healthComponent._maxHealth = 100.0f;
        _healthComponent._health = _healthComponent._maxHealth;
        
        if (current_Weapon == null)
            current_Weapon = weaponHolder.GetComponentInChildren<Weapon_Stats>();
        else
            Debug.Log($"{name}: (log)){nameof(current_Weapon)} There is no Weapon");
        
        if (current_Weapon == null)
            current_Weapon = weaponHolder.GetComponentInChildren<Weapon_Stats>();
    }

    private void SetupStateMachine()
    {
        // State Machine
        _stateMachine = new StateMachine();

        // Declare states
        var locomotionState = new LocomotionState(this, _animator);
        var jumpState = new JumpState(this, _animator);
        var dashState = new DashState(this, _animator);
        var attackState = new AttackState(this, _animator);

        // Define transitions
        At(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
        At(locomotionState, dashState, new FuncPredicate(() => dashTimer.IsRunning));
        At(locomotionState, attackState, new FuncPredicate(() => attackTimer.IsRunning));
        At(attackState, locomotionState, new FuncPredicate(() => !attackTimer.IsRunning));
        Any(locomotionState, new FuncPredicate(ReturnToLocomotionState));

        // Set initial state
        _stateMachine.SetState(locomotionState);
    }

    private bool ReturnToLocomotionState()
    {
        return groundChecker.IsGrounded 
               && !attackTimer.IsRunning 
               && !jumpTimer.IsRunning 
               && !dashTimer.IsRunning;
    }
    
    private void SetupTimers() 
    {
        // Setup timers
        jumpTimer = new CountdownTimer(jumpDuration);
        jumpCooldownTimer = new CountdownTimer(jumpCooldown);

        jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

        dashTimer = new CountdownTimer(dashDuration);
        dashCooldownTimer = new CountdownTimer(dashCooldown);

        dashTimer.OnTimerStart += () => dashVelocity = dashForce;
        dashTimer.OnTimerStop += () => { dashVelocity = 1f; dashCooldownTimer.Start(); };

        attackTimer = new CountdownTimer(attackCooldown);

        _timers = new(5) {jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer, attackTimer};
    }

    private void At(Istate from,Istate to , Ipredicate condition) => _stateMachine.AddTransition(from,to,condition);
    private void Any(Istate to , Ipredicate condition) => _stateMachine.AddAnyTransition(to,condition);

    private void OnEnable()
    {
        _input.Jump += OnJump;
        _input.Dash += OnDash;
        _input.Attack += OnAttack;
    }
    private void OnDisable()
    {
        _input.Jump -= OnJump;
        _input.Dash -= OnDash;
        _input.Attack -= OnAttack;
    }

    private void Update()
    {
        _stateMachine.Update();
        
        HandleTimers();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
    private void UpdateAnimator()
    {
        _animator.SetFloat(Speed, currentSpeed);
    }
    
    private void OnJump(bool performed)
    {
        if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
        {
            jumpTimer.Start();
            jumpVelocity = jumpForce;
        }
        else if (!performed && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }
    }
    
    private void OnDash(bool performed)
    {
        if (performed && !dashTimer.IsRunning && !dashCooldownTimer.IsRunning)
        {
            dashTimer.Start();
        }
        else if (!performed && dashTimer.IsRunning)
        {
            dashTimer.Stop();
        }
    }
    
    void OnAttack() 
    {
        if (!attackTimer.IsRunning) {
            attackTimer.Start();
        }
    }

    
    private void HandleTimers()
    {
        foreach (var timer in _timers)
            timer.Tick(Time.deltaTime);
    }

    public void Attack()
    {
        Vector3 attackPos = transform.position + transform.forward;
        
        Collider[] hitEnemies = Physics.OverlapSphere(attackPos, attackDistance);

        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Health_Component>().DecreaseHealth(attackDamage);
            }
        }
    }

    public void HandleJump()
    {
        // If not jumping and grounded, keep jump velocity at 0
        if (!jumpTimer.IsRunning && groundChecker.IsGrounded) 
        {
            jumpVelocity = ZeroF;
            return;
        }
        
        // Gravity takes over
        if (!jumpTimer.IsRunning) 
            jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        
        // Apply velocity
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpVelocity, _rigidbody.velocity.z);
    }

    public void HandleMovement()
    {
        var movementDirection = new Vector3(_input.Direction.x, 0f, _input.Direction.y);
        // Rotate movement direction to match camera rotation
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;
            
        if (adjustedDirection.magnitude > ZeroF) {
            HandleRotation(adjustedDirection);
            HandleHorizontalMovement(adjustedDirection);
            SmoothSpeed(adjustedDirection.magnitude);
        } else {
            SmoothSpeed(ZeroF);
                
            // Reset horizontal velocity for a snappy stop
            _rigidbody.velocity = new Vector3(ZeroF, _rigidbody.velocity.y, ZeroF);
        }
    }

    void HandleHorizontalMovement(Vector3 adjustedDirection)
    {
        // Move the player
        Vector3 velocity = adjustedDirection * (moveSpeed * dashVelocity * Time.fixedDeltaTime);
        _rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);
    }

    void HandleRotation(Vector3 adjustedDirection)
    {
        // Adjust rotation to match movement direction
        var targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public Rigidbody GetPlayerRigidbody() => _rigidbody;
    public InputReader GetPlayerInputReader() => _input;
    public Health_Component GetPlayerHealthComponent() => _healthComponent;
    public void SetCurrentWeapon(Weapon_Stats weapon) => current_Weapon = weapon;

    void SmoothSpeed(float value)
    {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bullet_Controller>(out var bullet))
        {
            _healthComponent.DecreaseHealth(bullet.damage);
            Destroy(other.gameObject);
        }
    }
}