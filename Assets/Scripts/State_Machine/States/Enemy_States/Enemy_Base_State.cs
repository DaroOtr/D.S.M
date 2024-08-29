/// <summary>
/// Base State For The Enemy
/// </summary>
public class Enemy_Base_State : State
{
    public Enemy_Component enemy;

    public Enemy_Base_State(string name, State_Machine state_Machine , Enemy_Component enemy) : base(name, state_Machine)
    {
        this.enemy = enemy;
    }
}
