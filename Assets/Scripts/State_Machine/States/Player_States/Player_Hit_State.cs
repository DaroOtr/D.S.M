using UnityEngine;

/// <summary>
/// Class To Handle The Player Hit State
/// </summary>
public class Player_Hit_State : Player_Base_State
{
    private const string hit_Animation_Name = "GetHit";
    private float animTime = 1.5f;
    private float animTimer = 0.0f;
    public Player_Hit_State(Player_State_Machine playerSM, Player_Component player) : base(nameof(Player_Hit_State), playerSM, player) { }

    public override void OnEnter()
    {
        base.OnEnter();
        player.character_Health_Component.OnInsufficient_Health += Character_Health_Component_OnInsufficient_Health;
        player.input.OnPlayerMove += Input_OnPlayerMove;
        animTime = 0.5f;
        animTimer = 0.0f;
        PlayHitAnimation();
    }

    private void Input_OnPlayerMove(Vector2 obj)
    {
        base.state_Machine.SetState(base.transitions[nameof(Player_Movement_State)]);
    }

    private void Character_Health_Component_OnInsufficient_Health()
    {
        base.state_Machine.SetState(base.transitions[nameof(Player_Dead_State)]);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (animTimer >= animTime)
            base.state_Machine.SetState(base.transitions[nameof(Player_Idle_State)]);
        else
            animTimer += Time.deltaTime;
    }

    /// <summary>
    /// Play The Hit Animation For The Player
    /// </summary>
    public void PlayHitAnimation()
    {
        GameObject.Instantiate(player.hit_Particles,player.transform);
    }

    public override void OnExit()
    {
        base.OnExit();
        player.character_Health_Component.OnInsufficient_Health -= Character_Health_Component_OnInsufficient_Health;
        player.input.OnPlayerMove -= Input_OnPlayerMove;
    }

    public virtual void AddStateTransitions(string transitionName, State transitionState)
    {
        base.AddStateTransitions(transitionName, transitionState);
    }
}
