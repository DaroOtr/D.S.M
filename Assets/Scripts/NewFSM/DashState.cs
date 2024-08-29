using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState
{
    public DashState(PlayerController player, Animator animator) : base(player, animator) { }
    public override void OnEnter()
    {
        _animator.CrossFade(DashHash,crossFadeDuration);
        Debug.Log("DashState.OnEnter");
    }

    public override void FixedUpdate()
    {
        _player.HandleMovement();
    }
}
