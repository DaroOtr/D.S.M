using System;
using UnityEngine;

/// <summary>
/// Class To Handle The Player State Machine
/// </summary>
public class Enemy_State_Machine : State_Machine
{
    [SerializeField] public Enemy_Component enemy;

    [SerializeField] private Enemy_Idle_State idle_State;
    [SerializeField] private Enemy_Move_State move_State;
    [SerializeField] private Enemy_Attack_State attack_State;
    [SerializeField] private Enemy_Hit_State hit_State;
    [SerializeField] private Enemy_Dead_State dead_State;

    protected override void OnEnable()
    {
        if (enemy == null)
        {
            enemy = GetComponent<Enemy_Component>();
        }
        if (!enemy)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(enemy)} is null");
            enabled = false;
        }

        idle_State = new Enemy_Idle_State(this,enemy);
        move_State = new Enemy_Move_State(this,enemy);
        attack_State = new Enemy_Attack_State(this,enemy);
        hit_State = new Enemy_Hit_State(this,enemy);
        dead_State = new Enemy_Dead_State(this,enemy);

        idle_State.AddStateTransitions(nameof(Enemy_Move_State), move_State);
        idle_State.AddStateTransitions(nameof(Enemy_Hit_State), hit_State);
        idle_State.AddStateTransitions(nameof(Enemy_Dead_State), dead_State);

        move_State.AddStateTransitions(nameof(Enemy_Idle_State), idle_State);
        move_State.AddStateTransitions(nameof(Enemy_Attack_State), attack_State);
        move_State.AddStateTransitions(nameof(Enemy_Hit_State), hit_State);
        move_State.AddStateTransitions(nameof(Enemy_Dead_State), dead_State);

        attack_State.AddStateTransitions(nameof(Enemy_Idle_State), idle_State);
        attack_State.AddStateTransitions(nameof(Enemy_Move_State), move_State);
        attack_State.AddStateTransitions(nameof(Enemy_Hit_State), hit_State);
        attack_State.AddStateTransitions(nameof(Enemy_Dead_State), dead_State);

        hit_State.AddStateTransitions(nameof(Enemy_Idle_State), idle_State);
        hit_State.AddStateTransitions(nameof(Enemy_Move_State), move_State);
        hit_State.AddStateTransitions(nameof(Enemy_Dead_State), dead_State);

        dead_State.AddStateTransitions(nameof(Enemy_Idle_State), idle_State);
        base.OnEnable();
    }

    protected override State GetInitialState()
    {
        base.GetInitialState();
        return idle_State;
    }
}
