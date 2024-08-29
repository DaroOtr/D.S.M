using System;
using UnityEngine;


/// <summary>
/// Scriptable Object For The Setting Of The Player
/// </summary>
[CreateAssetMenu(menuName = "Scripts/Player/Player Setings")]
[Obsolete]
public class Player_Setings : ScriptableObject
{
    [Header("Player SetUps")]
    public float health;
    [Range(0, 500)] public float speed;
    [Range(0, 500)] public float jumpForce;
    public float maxDistance;
    public float minJumpDistance;
    [SerializeField] private float turn_Smooth_Velocity;
    [Header("Player Jump Timers")]
    public float jumpBufferTime;
    public float turnSmoothTime;
    public float coyoteTime;

    public bool isBlocking { get; internal set; }
    public bool isAttacking { get; internal set; }
}
