/// <summary>
/// Base Class For The Player States
/// </summary>
public class Player_Base_State : State
{
    public Player_Component player;
    public Player_Base_State(string name, State_Machine state_Machine, Player_Component player) : base(name, state_Machine)
    {
        this.player = player;
    }
}
