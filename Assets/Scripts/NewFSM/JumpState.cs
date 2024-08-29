using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        _animator.CrossFade(JumpHash,crossFadeDuration);
        Debug.Log("JumpState.OnEnter");
    }

    public override void FixedUpdate()
    {
       _player.HandleJump();
       _player.HandleMovement();
    }
}