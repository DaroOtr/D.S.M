public class Transition : ITransition
{
    public Istate To { get; }
    public Ipredicate Condition { get; }

    public Transition(Istate To , Ipredicate Condition)
    {
        this.To = To;
        this.Condition = Condition;
    }
}