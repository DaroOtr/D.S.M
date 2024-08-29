using UnityEngine;

public class LocomotionState : BaseState
{
    public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        _animator.CrossFade(LocomotionHash,crossFadeDuration);
        Debug.Log("LocomotionState.OnEnter");
    }
    public override void FixedUpdate()
    {
         // call player controller move Logic
         _player.HandleMovement();
    }
}