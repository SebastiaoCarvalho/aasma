public class Action
{
    public virtual void Execute() { }
    public virtual bool IsDone() { return true; }

    public virtual float Utility() { return 0; }
}