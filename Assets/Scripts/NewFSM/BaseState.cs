using UnityEngine;

public class BaseState : Istate
{
    protected readonly PlayerController _player;
    protected readonly Animator _animator;

    // This in an int that Represents a animation in the Animator
    protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    protected static readonly int JumpHash = Animator.StringToHash("Jump");
    protected static readonly int DashHash = Animator.StringToHash("Dash");
    protected static readonly int AttackHash = Animator.StringToHash("Attack");

    protected const float crossFadeDuration = 0.01f;

    protected BaseState(PlayerController player , Animator animator)
    {
        _player = player;
        _animator = animator;
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void OnExit()
    {
        Debug.Log("BaseState.OnExit");
    }
}
