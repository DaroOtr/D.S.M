public interface ITransition
{
    Istate To { get; }
    Ipredicate Condition { get;}
}