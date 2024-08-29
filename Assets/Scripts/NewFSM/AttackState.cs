using System;
using UnityEngine;

public class AttackState : BaseState 
{
    public AttackState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter() 
    {
        _animator.CrossFade(AttackHash, crossFadeDuration);
        _player.Attack();
    }

    public override void FixedUpdate() 
    {
        _player.HandleMovement();
    }
}